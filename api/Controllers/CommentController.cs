using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // [HttpPost]
        
    }
}