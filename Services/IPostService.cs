using RestApiUsingCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiUsingCore.Services
{
    public interface IPostService
    {
        List<Post> GetPosts();

        Post GetPostById(string id);


        bool UpdatePost(Post posttoUpdate);

        bool DeletePost(string postid);
    }
}
