using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _ICommentRepo;
        private readonly IStockRepository _stockRepo;
        
        public CommentController(ICommentRepository commentRepo,
        IStockRepository stockRepo)
        {
            _ICommentRepo = commentRepo;
            _stockRepo = stockRepo;
           
        }

        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _ICommentRepo.GetAllAsync();

            var commentDto = comments.Select(s => s.tocommentdto());

            return Ok(commentDto);
        }
        [HttpGet("{id}")]
        // [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)

        {
            var comment =await _ICommentRepo.GetByIdAsync(id);
            
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.tocommentdto());
            
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] int stockId,CreateCommentDto commentDto )
        {
            if(!await _stockRepo.StockExists(stockId))
            {
               return BadRequest ("Stock does not exist");     

            }
            var commentModel =commentDto.tocommentfromcreate(stockId);

            await _ICommentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById),new{id=commentModel},commentModel.tocommentdto());
        }
         [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _ICommentRepo.UpdateAsync(id, updateDto.tocommentfromupdate(id));

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment.tocommentdto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _ICommentRepo.DeleteAsync(id);

            if (commentModel == null)
            {
                return NotFound("Comment does not exist");
            }

            return Ok(commentModel);
        }
        
    }
}





