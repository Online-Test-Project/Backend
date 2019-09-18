using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Question Question { get; set; }
    }
}
