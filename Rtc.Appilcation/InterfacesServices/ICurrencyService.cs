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
        Task<IEnumerable<CurrencyDtos>> GetAllCurrenciesAsync();
        Task<CurrencyDtos> GetCurrencyByIdxAsync(int idx);
        Task<CurrencyDtos> GetCurrencyByCodeAsync(string currencyCode);
        Task<CurrencyDtos> CreateCurrencyAsync(CurrencyDtos currency);
        Task<bool> UpdateCurrencyAsync(CurrencyDtos currency); 
        Task<bool> DeleteCurrencyAsync(int idx);
    }
}
