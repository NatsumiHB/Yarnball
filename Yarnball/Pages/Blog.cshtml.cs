using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Yarnball.Data;

namespace Yarnball.Pages
{
    public class BlogModel : PageModel
    {
        public YarnballUser YarnballUser { get; set; }

        private readonly UserManager<YarnballUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public BlogModel(UserManager<YarnballUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGet(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                if (User.Identity.IsAuthenticated)
                    YarnballUser = await _userManager.GetUserAsync(User);
                else
                    return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = "/Blog" });
            }
            else
                YarnballUser = await _userManager.FindByNameAsync(username).ConfigureAwait(false);

            if (YarnballUser != null)
            {
                // Load posts
                await _dbContext.Entry(YarnballUser).Collection(u => u.Posts).LoadAsync().ConfigureAwait(false);

                foreach (var post in YarnballUser.Posts)
                {
                    await _dbContext.Entry(post).Collection(p => p.PostTags).LoadAsync().ConfigureAwait(false);

                    foreach (var pt in post.PostTags)
                        await _dbContext.Entry(pt).Reference(pt => pt.Tag).LoadAsync().ConfigureAwait(false);
                }
            }

            return Page();
        }
    }
}