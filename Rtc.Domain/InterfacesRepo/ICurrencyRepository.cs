

using Rtc.Domain.Entities;

namespace Rtc.Domain.InterfacesRepo
{
    public interface ICurrencyRepository : IRepository<Currency> 
    {
        Task<Currency> GetByCurrencyCodeAsync(string currencyCode);
        Task<List<Currency>> GetAllAsync();
        Task<bool> CreateCurrencyAsnyc(Currency currency);
        Task<bool> UpdateCurrencyAsnyc(Currency currency);
        Task<bool> DeleteCurrencyAsnyc(int currencyId);
    }
}
