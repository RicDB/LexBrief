﻿using BLL.Contracts;
using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LexBrief.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentsService _commentService;
        public CommentsController(ICommentsService commentsService)
        {
            _commentService = commentsService;
        }
        // GET: api/<CommentsController>
        [HttpGet]
        public IEnumerable<CommentDto> Get()
        {
            return _commentService.GetComments();
        }

        // POST api/<CommentsController>
        [HttpPost]
        public CommentDto Post([FromBody] CommentDto comment)
        {
            return _commentService.AddComment(comment);
        }

        [HttpPost("Summary")]
        public async Task<CommentsSummaryDto> Summary([FromBody] CommentDto[] comments)
        {
            return await _commentService.GetCommentsSummary(comments);
        }
    }
}
