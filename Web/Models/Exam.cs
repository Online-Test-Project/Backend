using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
            Scores = new HashSet<Score>();
        }

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public TimeSpan Time { get; set; }

        public virtual Account Owner { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
