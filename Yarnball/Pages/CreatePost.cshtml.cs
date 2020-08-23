using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [BindProperty]
        public string Tags { get; set; }

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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);

            var tags = Tags.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var oldTags = new List<Tag>();
            var newTags = new List<Tag>();
            foreach (var tag in tags)
            {
                // Ensure that tag is in DB
                if (!await _dbContext.Tags.AnyAsync(t => t.Name == tag))
                {
                    newTags.Add(new Tag
                    {
                        Name = tag.Trim(),
                        PostTags = new List<PostTag>()
                    });
                }
                else
                    oldTags.Add(_dbContext.Tags.First(t => t.Name == tag));
            }
            // Save tags
            await _dbContext.Tags.AddRangeAsync(newTags).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            // Create post and add tags
            var post = new Post
            {
                CreatedAt = DateTime.Now,
                User = user,
                Title = Title,
                Content = PostContent,
                PostTags = new List<PostTag>()
            };

            foreach (var tag in oldTags.Concat(newTags).ToList())
            {
                post.PostTags.Add(new PostTag
                {
                    Post = post,
                    Tag = tag
                });
            }

            // Add tag to user and all posts, save
            await _dbContext.Entry(user).Collection(u => u.Posts).LoadAsync().ConfigureAwait(false);
            user.Posts.Add(post);
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("Blog");
        }
    }
}