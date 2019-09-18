using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Exam
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public bool Random { get; set; }

        public virtual Account Owner { get; set; }
    }
}
