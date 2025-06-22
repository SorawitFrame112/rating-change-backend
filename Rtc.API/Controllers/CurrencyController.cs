using Microsoft.AspNetCore.Mvc;
using Rtc.Appilcation.InterfacesServices;
using Rtc.Domain.Dtos;


namespace Rtc.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyDtos>>> GetAllCurrencies()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return Ok(currencies); 
        }

        [HttpGet("{idx}")]
        public async Task<ActionResult<CurrencyDtos>> GetCurrencyByIdx(int idx)
        {
            var currency = await _currencyService.GetCurrencyByIdxAsync(idx);
            if (currency == null)
            {
                return NotFound($"Currency with Idx {idx} not found."); 
            }
            return Ok(currency); 
        }

        [HttpGet("ByCode/{currencyCode}")]
        public async Task<ActionResult<CurrencyDtos>> GetCurrencyByCode(string currencyCode)
        {
            var currency = await _currencyService.GetCurrencyByCodeAsync(currencyCode);
            if (currency == null)
            {
                return NotFound($"Currency with Code '{currencyCode}' not found.");
            }
            return Ok(currency);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CurrencyDtos))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] 
        public async Task<ActionResult<CurrencyDtos>> CreateCurrency([FromBody] CurrencyDtos currencyDto)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState); 
            }

            try
            {
                var createdCurrency = await _currencyService.CreateCurrencyAsync(currencyDto);
                return CreatedAtAction(nameof(GetCurrencyByIdx), new { idx = createdCurrency.Idx }, createdCurrency);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating currency: {ex.Message}");
            }
        }

        [HttpPut("{idx}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] 
        public async Task<IActionResult> UpdateCurrency(int idx, [FromBody] CurrencyDtos currencyDto)
        {
            if (idx != currencyDto.Idx) // Ensure the ID in the route matches the ID in the body
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _currencyService.UpdateCurrencyAsync(currencyDto);
                if (!updated)
                {
                    return NotFound($"Currency with Idx {idx} not found or could not be updated.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating currency: {ex.Message}");
            }
        }

        [HttpDelete("{idx}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<IActionResult> DeleteCurrency(int idx)
        {
            try
            {
                var deleted = await _currencyService.DeleteCurrencyAsync(idx);
                if (!deleted)
                {
                    return NotFound($"Currency with Idx {idx} not found."); 
                }
                return NoContent(); 
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting currency: {ex.Message}");
            }
        }
    }
}