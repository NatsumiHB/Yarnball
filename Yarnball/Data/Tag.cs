using System;
using System.Collections.Generic;

namespace Yarnball.Data
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}