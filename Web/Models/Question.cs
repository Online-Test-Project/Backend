using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            ExamQuestions = new HashSet<ExamQuestion>();
        }

        public Guid Id { get; set; }
        public Guid BankId { get; set; }
        public string Difficulty { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }

        public virtual QuestionBank Bank { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
