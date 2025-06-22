using AutoMapper;
using Microsoft.EntityFrameworkCore; 
using Rtc.Appilcation.InterfacesServices;
using Rtc.Domain.Dtos;
using Rtc.Domain.Entities;
using Rtc.Domain.InterfacesRepo;

namespace Rtc.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }
     
        public async Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync()
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CurrencyDto>>(currencies); 
        }

        public async Task<CurrencyDto> GetCurrencyByIdxAsync(int idx)
        {
            var currency = await _currencyRepository.GetByIdAsync(idx);
            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task<CurrencyDto> GetCurrencyByCodeAsync(string currencyCode)
        {
            var currency = await _currencyRepository.GetByCurrencyCodeAsync(currencyCode);
            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task<CurrencyDto> CreateCurrencyAsync(CurrencyDto currencyDto)
        {
            var currency = _mapper.Map<Currency>(currencyDto);
            var existingCurrency = await _currencyRepository.GetByCurrencyCodeAsync(currency.CurrencyCode);
            if (existingCurrency != null)
            {
                throw new InvalidOperationException($"Currency with code '{currency.CurrencyCode}' already exists.");
            }
            currency.CreatedDate = DateTime.UtcNow;
            currency.CreatedBy = "API_User"; 
            await _currencyRepository.AddAsync(currency);
            await _currencyRepository.SaveChangesAsync();
            return _mapper.Map<CurrencyDto>(currency);
        }
        public async Task<bool> UpdateCurrencyAsync(CurrencyDto currencyDto)
        {
            var existingCurrency = await _currencyRepository.GetByIdAsync((int)currencyDto.Idx);
            if (existingCurrency == null)
            {
                return false; 
            }

            var duplicateCode = await _currencyRepository.GetByCurrencyCodeAsync(currencyDto.CurrencyCode);
            if (duplicateCode != null && duplicateCode.Idx != currencyDto.Idx)
            {
                throw new InvalidOperationException($"Currency with code '{currencyDto.CurrencyCode}' already exists for another entry.");
            }

            _mapper.Map(currencyDto, existingCurrency);

            existingCurrency.UpdateDate = DateTime.UtcNow;
            existingCurrency.UpdateBy = "API_User";

            return true;
        }

        public async Task<bool> DeleteCurrencyAsync(int idx)
        {
            var currencyToDelete = await _currencyRepository.GetByIdAsync(idx);
            if (currencyToDelete == null)
            {
                return false;
            }

            await _currencyRepository.DeleteAsync(idx);
            await _currencyRepository.SaveChangesAsync();
            return true;
        }
    }
}