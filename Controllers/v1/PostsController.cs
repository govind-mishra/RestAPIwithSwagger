using Microsoft.AspNetCore.Mvc;
using RestApiUsingCore.Contracts;
using RestApiUsingCore.Contracts.v1;
using RestApiUsingCore.Contracts.v1.Requests;
using RestApiUsingCore.Contracts.v1.Responses;
using RestApiUsingCore.Domain;
using RestApiUsingCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiUsingCore.Controllers.v1
{
    public class PostsController : Controller
    {
        //make the list of posts 
        private readonly IPostService _postService; 

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetPosts());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute] string postId)
        {
            var post = _postService.GetPostById(postId);
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)
        {
            //you should not mix up domain with your request because of versioning
            var post = new Post { id = postRequest.Id };
            if (String.IsNullOrEmpty(post.id))
                post.id = Guid.NewGuid().ToString();
            _postService.GetPosts().Add(post);

            var baseUrl = HttpContext.Request.Scheme + "/" + HttpContext.Request.Host.ToUriComponent();
            var locationurl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.id);
            //seperate response object from real domain
            var postResponse = new PostResponse { Id = post.id };
            return Created(locationurl, postResponse);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute] string postId,[FromBody] UpdatePostRequest updaterequest)
        {
            var post = new Post
            {
                id = postId,
                Name = updaterequest.Name
            };

            bool updated = _postService.UpdatePost(post);

            if (updated)
                return Ok(post);
            else
            {
                return NotFound();
            }

        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public IActionResult Delete([FromRoute] string postId)
        {
            bool deleted = _postService.DeletePost(postId);
            if (deleted)
                return NoContent();
            else
                return NotFound();
                
        }
    }
}
