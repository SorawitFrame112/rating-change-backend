using Rtc.Domain.Dtos;
using Rtc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Appilcation.InterfacesServices
{
    public  interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync();
        Task<CurrencyDto> GetCurrencyByIdxAsync(int idx);
        Task<CurrencyDto> GetCurrencyByCodeAsync(string currencyCode);
        Task<CurrencyDto> CreateCurrencyAsync(CurrencyDto currency);
        Task<bool> UpdateCurrencyAsync(CurrencyDto currency); 
        Task<bool> DeleteCurrencyAsync(int idx);
    }
}
