using System;
using System.ComponentModel.DataAnnotations;

namespace Yarnball.Data
{
    public class Post
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public YarnballUser User { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}