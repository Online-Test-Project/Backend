using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
        }

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Password { get; set; }
        public Guid BankId { get; set; }

        public virtual QuestionBank Bank { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
