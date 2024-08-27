using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockService.Common.Interfaces;
using StockService.Models;

namespace StockService.Controllers
{
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet("GetStockList")]
        public IActionResult GetStockList()
        {
            return Ok(_stockRepository.GetStockList);
        }

        [HttpGet("GetProductList")]
        public IActionResult GetProductList()
        {
            return Ok(_stockRepository.GetProductList);
        }

        [HttpGet("GetCartList")]
        public IActionResult GetCartList()
        {
            return Ok(_stockRepository.GetCartList);
        }

        [HttpPost("CreateCart")]
        public async Task<IActionResult> CreateCart([FromBody] CartRequest req)
        {
            try
            {
                return Ok(await _stockRepository.CreateCart(req) ?
               new { message = "Successful" } : new { message = "Fail" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart([FromQuery] int cartId, [FromBody] CartRequest req)
        {
            try
            {
                return Ok(await _stockRepository.UpdateCart(cartId, req) ?
               new { message = "Successful" } : new { message = "Fail" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> DeleteCart([FromQuery] int cartId)
        {
            try
            {
                return Ok(await _stockRepository.DeleteCart(cartId) ?
                 new { message = "Successful" } : new { message = "Fail" });
            }
            catch (Exception ex)
            { 
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("CheckOutCart")]
        public async Task<IActionResult> CheckOutCart()
        {
            try
            {
                return Ok(await _stockRepository.CheckOutCart() ?
                 new { message = "Successful" } : new { message = "Fail" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
