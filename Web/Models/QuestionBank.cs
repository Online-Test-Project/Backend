using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class QuestionBank
    {
        public QuestionBank()
        {
            Question = new HashSet<Question>();
            RandomExam = new HashSet<RandomExam>();
        }

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Subject { get; set; }

        public virtual Account Owner { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ICollection<RandomExam> RandomExam { get; set; }
    }
}
