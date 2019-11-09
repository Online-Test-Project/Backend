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
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }

        public virtual User Owner { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
