using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Yarnball.Data
{
    public class YarnballUser : IdentityUser<Guid>
    {
        public List<Post> Posts { get; set; }
    }
}