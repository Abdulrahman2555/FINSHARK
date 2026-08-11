using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;

// using api.Dtos.Stock;
// using api.Helpers;
// using api.Interfaces;
// using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        
        public StockController(ApplicationDBContext context)
        {
           
            _context = context;
        }

        [HttpGet]
       
        public async Task<IActionResult>  GetAll()
        {
            

          
           var stocks=await  _context.Stocks.ToListAsync();

           var stockDtos = stocks.Select(s => s.ToStockDto());
            // var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stockDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel =  stockDto.ToStockFromCreateDTO();
             await _context.Stocks.AddAsync(stockModel);
             await  _context.SaveChangesAsync();
            // await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x =>x.Id==id);
            

            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol=updateDto.Symbol;
            stockModel.CompanyName=updateDto.CompanyName;
            stockModel.Purchase=updateDto.Purchase;
            stockModel.LastDiv=updateDto.LastDiv;
            stockModel.Industry=updateDto.Industry;
            stockModel.MarketCap=updateDto.MarketCap;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel  =await _context.Stocks.FirstOrDefaultAsync(x =>x.Id==id);  
            // await _stockRepo.DeleteAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }
              _context.Stocks.Remove(stockModel); 
             await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}