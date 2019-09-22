using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class ExamQuestion
    {
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public string Code { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual Question Question { get; set; }
    }
}
