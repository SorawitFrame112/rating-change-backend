using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; 


using Rtc.Domain.Entities;
using Rtc.Domain.Dtos;
using Rtc.Domain.InterfacesRepo;
using Rtc.Infrastructure.Repositories;
using Rtc.Infrastructure; 
using Rtc.Application.Services; 
namespace TestProject1
{
    public class CurrencyServiceTests : IDisposable
    {
        private RtcDbContext _context;
        private ICurrencyRepository _currencyRepository;
        private CurrencyService _currencyService;
        private Mock<IMapper> _mockMapper;

        public CurrencyServiceTests()
        {
            var options = new DbContextOptionsBuilder<RtcDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new RtcDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _mockMapper = new Mock<IMapper>();

            _currencyRepository = new CurrencyRepository(_context);
            _currencyService = new CurrencyService(_currencyRepository, _mockMapper.Object);

           
            _context.Currencies.AddRange(
                new Currency { Idx = 1, CurrencyCode = "USD", CurrencyName = "US Dollar", CreatedDate = DateTime.UtcNow, CreatedBy = "Test" },
                new Currency { Idx = 2, CurrencyCode = "EUR", CurrencyName = "Euro",CreatedDate = DateTime.UtcNow, CreatedBy = "Test" }
            );
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllCurrenciesAsync_ReturnsAllCurrencies()
        {
            
            var currencyDtosexpect = new List<CurrencyDto>
            {
                new CurrencyDto { Idx = 1, CurrencyCode = "USD", CurrencyName = "US Dollar"  },
                new CurrencyDto { Idx = 2, CurrencyCode = "EUR", CurrencyName = "Euro"  }
            };

            _mockMapper.Setup(m => m.Map<IEnumerable<CurrencyDto>>(It.IsAny<IEnumerable<Currency>>()))
                       .Returns(currencyDtosexpect);

            var result = await _currencyService.GetAllCurrenciesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockMapper.Verify(m => m.Map<IEnumerable<CurrencyDto>>(It.IsAny<IEnumerable<Currency>>()), Times.Once());
        }

        [Fact]
        public async Task GetCurrencyByIdxAsync_ReturnsCurrencyDto_WhenExists()
        {
            var currencyDto = new CurrencyDto { Idx = 1, CurrencyCode = "USD", CurrencyName = "US Dollar"  };

            _mockMapper.Setup(m => m.Map<CurrencyDto>(It.IsAny<Currency>()))
                       .Returns(currencyDto);

            var result = await _currencyService.GetCurrencyByIdxAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Idx);
            Assert.Equal("USD", result.CurrencyCode);
            _mockMapper.Verify(m => m.Map<CurrencyDto>(It.IsAny<Currency>()), Times.Once());
        }

        [Fact]
        public async Task GetCurrencyByIdxAsync_ReturnsNull_WhenNotExists()
        {
            _mockMapper.Setup(m => m.Map<CurrencyDto>(It.IsAny<Currency>()))
                       .Returns((CurrencyDto)null);

            var result = await _currencyService.GetCurrencyByIdxAsync(99);

            Assert.Null(result);
            _mockMapper.Verify(m => m.Map<CurrencyDto>(It.IsAny<Currency>()), Times.Once());
        }

        [Fact]
        public async Task GetCurrencyByCodeAsync_ReturnsCurrencyDto_WhenExists()
        {
            var currencyDto = new CurrencyDto { Idx = 1, CurrencyCode = "USD", CurrencyName = "US Dollar"  };

            _mockMapper.Setup(m => m.Map<CurrencyDto>(It.IsAny<Currency>()))
                       .Returns(currencyDto);

            var result = await _currencyService.GetCurrencyByCodeAsync("USD");

            Assert.NotNull(result);
            Assert.Equal("USD", result.CurrencyCode);
            _mockMapper.Verify(m => m.Map<CurrencyDto>(It.IsAny<Currency>()), Times.Once());
        }

        [Fact]
        public async Task GetCurrencyByCodeAsync_ReturnsNull_WhenNotExists()
        {
            _mockMapper.Setup(m => m.Map<CurrencyDto>(It.IsAny<Currency>()))
                       .Returns((CurrencyDto)null);

            var result = await _currencyService.GetCurrencyByCodeAsync("XYZ");

            Assert.Null(result);
            _mockMapper.Verify(m => m.Map<CurrencyDto>(It.IsAny<Currency>()), Times.Once());
        }

        [Fact]
        public async Task CreateCurrencyAsync_CreatesNewCurrency_ReturnsCurrencyDto()
        {
            var newCurrencyDto = new CurrencyDto { CurrencyCode = "JPY", CurrencyName = "Japanese Yen"  };
            var newCurrency = new Currency { Idx = 3, CurrencyCode = "JPY", CurrencyName = "Japanese Yen"  };

            _mockMapper.Setup(m => m.Map<Currency>(newCurrencyDto)).Returns(newCurrency);
            _mockMapper.Setup(m => m.Map<CurrencyDto>(newCurrency)).Returns(newCurrencyDto);

            var result = await _currencyService.CreateCurrencyAsync(newCurrencyDto);

            Assert.NotNull(result);
            Assert.Equal("JPY", result.CurrencyCode);
            Assert.Contains(_context.Currencies, c => c.CurrencyCode == "JPY");
            Assert.Equal(3, _context.Currencies.Count()); // 2 initial + 1 new
            _mockMapper.Verify(m => m.Map<Currency>(newCurrencyDto), Times.Once());
            _mockMapper.Verify(m => m.Map<CurrencyDto>(It.Is<Currency>(c => c.CurrencyCode == "JPY")), Times.Once());
        }

        [Fact]
        public async Task CreateCurrencyAsync_ThrowsException_WhenCurrencyCodeExists()
        {
            var existingCurrencyDto = new CurrencyDto { CurrencyCode = "USD", CurrencyName = "US Dollar"  };
            var existingCurrency = new Currency { Idx = 1, CurrencyCode = "USD", CurrencyName = "US Dollar"  };

            _mockMapper.Setup(m => m.Map<Currency>(existingCurrencyDto)).Returns(existingCurrency);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _currencyService.CreateCurrencyAsync(existingCurrencyDto)
            );

            Assert.Contains("Currency with code 'USD' already exists.", exception.Message);
            Assert.Equal(2, _context.Currencies.Count()); // No new currency added
            _mockMapper.Verify(m => m.Map<Currency>(existingCurrencyDto), Times.Once());
        }

        [Fact]
        public async Task UpdateCurrencyAsync_UpdatesExistingCurrency_ReturnsTrue()
        {
            var updatedCurrencyDto = new CurrencyDto { Idx = 1, CurrencyCode = "USD", CurrencyName = "Updated US Dollar" };
            var existingCurrency = _context.Currencies.Find(1); // Get the real existing entity

            _mockMapper.Setup(m => m.Map(updatedCurrencyDto, existingCurrency)).Callback(() =>
            {
                existingCurrency.CurrencyName = updatedCurrencyDto.CurrencyName;
            });

            var result = await _currencyService.UpdateCurrencyAsync(updatedCurrencyDto);

            Assert.True(result);
            var updatedInDb = await _context.Currencies.FindAsync(1);
            Assert.NotNull(updatedInDb);
            Assert.Equal("Updated US Dollar", updatedInDb.CurrencyName);
            _mockMapper.Verify(m => m.Map(updatedCurrencyDto, existingCurrency), Times.Once());
        }

        [Fact]
        public async Task UpdateCurrencyAsync_ReturnsFalse_WhenCurrencyDoesNotExist()
        {
            var nonExistentCurrencyDto = new CurrencyDto { Idx = 99, CurrencyCode = "XYZ", CurrencyName = "Non Existent"  };

            var result = await _currencyService.UpdateCurrencyAsync(nonExistentCurrencyDto);

            Assert.False(result);
            _mockMapper.Verify(m => m.Map(It.IsAny<CurrencyDto>(), It.IsAny<Currency>()), Times.Never()); // Mapper should not be called
        }

        [Fact]
        public async Task UpdateCurrencyAsync_ThrowsException_WhenCurrencyCodeDuplicate()
        {
            var updatedCurrencyDto = new CurrencyDto { Idx = 1, CurrencyCode = "EUR", CurrencyName = "Updated EUR"  };
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _currencyService.UpdateCurrencyAsync(updatedCurrencyDto)
            );

            Assert.Contains("Currency with code 'EUR' already exists for another entry.", exception.Message);
            var originalUsd = await _context.Currencies.FindAsync(1);
            Assert.Equal("USD", originalUsd.CurrencyCode);
        }

        [Fact]
        public async Task DeleteCurrencyAsync_RemovesCurrency_ReturnsTrue()
        {
            var result = await _currencyService.DeleteCurrencyAsync(1);

            Assert.True(result);
            Assert.Null(await _context.Currencies.FindAsync(1));
            Assert.Equal(1, _context.Currencies.Count()); 
        }

        [Fact]
        public async Task DeleteCurrencyAsync_ReturnsFalse_WhenCurrencyDoesNotExist()
        {
            var result = await _currencyService.DeleteCurrencyAsync(99);

            Assert.False(result);
            Assert.Equal(2, _context.Currencies.Count()); 
        }
    }
}