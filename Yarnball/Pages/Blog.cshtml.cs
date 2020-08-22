using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarnball.Data;

namespace Yarnball.Pages
{
    public class BlogModel : PageModel
    {
        public YarnballUser YarnballUser { get; set; }
        public List<Post> Posts { get; set; }

        private readonly UserManager<YarnballUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public BlogModel(UserManager<YarnballUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task OnGet(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                if (User.Identity.IsAuthenticated)
                    YarnballUser = await _userManager.GetUserAsync(User);
                else
                    YarnballUser = null;
            }
            else
                YarnballUser = await _userManager.FindByNameAsync(username).ConfigureAwait(false);

            Posts = _applicationDbContext.Posts.Where(p => p.UserId == YarnballUser.Id).ToList();
        }
    }
}