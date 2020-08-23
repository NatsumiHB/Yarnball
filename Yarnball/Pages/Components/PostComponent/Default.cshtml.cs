using Microsoft.AspNetCore.Mvc.RazorPages;
using Yarnball.Data;

namespace Yarnball.Pages.Components.PostComponent
{
    public class PostComponentModel : PageModel
    {
        public Post Post { get; set; }
        public int Width { get; set; }
    }
}