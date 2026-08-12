
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;

using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/stock")]
   
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly  IStockRepository _stockrepo;
        private readonly ApplicationDBContext _context;
        
        public StockController(ApplicationDBContext context , IStockRepository stockRepository)
        {
           _stockrepo=stockRepository;
            _context = context;
        }

        [HttpGet]
       
        public async Task<IActionResult>  GetAll()
        {
            

          
           var stocks = await _stockrepo.GetAllAsync();

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
            
            await _stockrepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
          

            var stockModel = await _stockrepo.UpdateAsync(id,updateDto);
          
            if (stockModel == null)
            {
                return NotFound();
            }

          
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
         
            var stockModel  = await _stockrepo.DeleteAsync(id);
         
            if (stockModel == null)
            {
                return NotFound();
            }
             
            return NoContent();
        }
    }

}