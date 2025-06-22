using Microsoft.EntityFrameworkCore;
using Rtc.Domain.Entities;
using Rtc.Domain.InterfacesRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Infrastructure.Repositories
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
            private readonly RtcDbContext _dbContext;
        public CurrencyRepository(RtcDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Currency> GetByCurrencyCodeAsync(string currencyCode)
        {
            return await _dbContext.Currencies.FirstOrDefaultAsync(c => c.CurrencyCode == currencyCode);
        }
        public async Task<List<Currency>> GetAllAsync()
        {
            return await _dbContext.Currencies.ToListAsync();
        }

        public async Task<bool> CreateCurrencyAsnyc(Currency currency)
        {
            await _dbContext.Currencies.AddAsync(currency);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateCurrencyAsnyc(Currency currency)
        {
            _dbContext.Currencies.Update(currency);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteCurrencyAsnyc(int currencyId)
        {
           await  _dbContext .Currencies.Where(c => c.Idx == currencyId).ExecuteDeleteAsync();
           await  _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
