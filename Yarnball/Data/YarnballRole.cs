﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Yarnball.Data
{
    public class YarnballRole : IdentityRole<Guid>
    {
        public YarnballRole(string name) : base(name)
        {
        }
    }
}