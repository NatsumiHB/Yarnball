using Microsoft.AspNetCore.Mvc;
using Yarnball.Data;
using Yarnball.Pages.Components.PostComponent;

namespace Yarnball.ViewComponents
{
    public class PostComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Post post, int width)
            => View(new PostComponentModel { Post = post, Width = width });
    }
}