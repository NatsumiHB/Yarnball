using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Yarnball.Data;

namespace Yarnball.Pages
{
    [Authorize]
    public class CreatePostModel : PageModel
    {
        [Required]
        [BindProperty]
        public string Title { get; set; }

        [Required]
        [BindProperty]
        [DisplayName("Content")]
        public string PostContent { get; set; }

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<YarnballUser> _userManager;

        public CreatePostModel(ApplicationDbContext dbContext, UserManager<YarnballUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task OnPost()
        {
            var user = await _userManager.GetUserAsync(User);

            _dbContext.Posts.Add(new Post
            {
                CreatedAt = DateTime.Now,
                UserId = user.Id,
                User = user,
                Title = Title,
                Content = PostContent
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}