using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamKit = new HashSet<ExamKit>();
            Score = new HashSet<Score>();
        }

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public virtual Account Owner { get; set; }
        public virtual ICollection<ExamKit> ExamKit { get; set; }
        public virtual ICollection<Score> Score { get; set; }
    }
}
