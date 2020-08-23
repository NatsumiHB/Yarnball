using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarnball.Data;

namespace Yarnball.Pages
{
    public class SearchModel : PageModel
    {
        public string Query { get; set; }
        public List<Post> Posts { get; set; }

        private readonly ApplicationDbContext _dbContext;

        public SearchModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGet(string query)
        {
            Query = query;

            if (string.IsNullOrWhiteSpace(query))
                return RedirectToPage("Index");

            var searchTags = Query.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            searchTags.ForEach(t => t.Trim());

            Posts = _dbContext.Posts.Where(p => p.PostTags.All(t => searchTags.Contains(t.Tag.Name))).OrderByDescending(p => p.CreatedAt).ToList();

            foreach (var post in Posts)
            {
                await _dbContext.Entry(post).Reference(p => p.User).LoadAsync().ConfigureAwait(false);
                await _dbContext.Entry(post).Collection(p => p.PostTags).LoadAsync().ConfigureAwait(false);

                foreach (var pt in post.PostTags)
                    await _dbContext.Entry(pt).Reference(pt => pt.Tag).LoadAsync().ConfigureAwait(false);
            }

            return Page();
        }
    }
}