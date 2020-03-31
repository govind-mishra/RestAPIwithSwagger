using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApiUsingCore.Domain;

namespace RestApiUsingCore.Services
{
    //make this class to make object single time only 
    public class PostService : IPostService
    {
        private readonly List<Post> _post;
        public PostService()
        {
            this._post = new List<Post>();
            for (int i = 0; i < 5; i++)
            {
                _post.Add(new Post
                {
                    id = Guid.NewGuid().ToString(),
                    Name = $"Post name is {i}"
                });
            }
        }

        public bool DeletePost(string postid)
        {
            var post = GetPostById(postid);
            if (post == null)
                return false;
            _post.Remove(post);
            return true;

        }

        public Post GetPostById(string id)
        {
            return _post.SingleOrDefault(x => x.id == id);
        }

        public List<Post> GetPosts()
        {
            return _post;
        }

        public bool UpdatePost(Post posttoUpdate)
        {
            var ifExists = GetPostById(posttoUpdate.id) != null;
            if (!ifExists)
                return false;
            else
            {
                //always find the index and then update so that it will not change the sequence
                var index = _post.FindIndex(x=>x.id==posttoUpdate.id);
                _post[index] = posttoUpdate;
                return true;
            }
        }
    }
}
