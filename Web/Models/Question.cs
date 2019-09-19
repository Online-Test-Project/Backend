using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Question
    {
        public Question()
        {
            Answer = new HashSet<Answer>();
            ExamKit = new HashSet<ExamKit>();
        }

        public Guid Id { get; set; }
        public Guid BankId { get; set; }
        public string Difficulty { get; set; }
        public int Type { get; set; }

        public virtual QuestionBank Bank { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<ExamKit> ExamKit { get; set; }
    }
}
