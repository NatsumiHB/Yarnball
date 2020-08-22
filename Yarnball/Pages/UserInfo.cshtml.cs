using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarnball.Data;

namespace Yarnball.Pages
{
    public class UserInfoModel : PageModel
    {
        public readonly UserManager<YarnballUser> UserManager;
        public List<YarnballUser> SortedUsers { get; set; }
        private YarnballUser CurrentUser;

        public UserInfoModel(UserManager<YarnballUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task OnGet()
        {
            CurrentUser = await UserManager.GetUserAsync(User);

            SortedUsers = UserManager
                .Users
                .Where(u => u.Id != CurrentUser.Id)
                .OrderBy(u => u.UserName)
                .ToList();
        }

        public async Task<string> GetFirstRole(YarnballUser user)
        {
            var roles = await UserManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? "User";
        }
    }
}