using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Account
    {
        public Account()
        {
            Exam = new HashSet<Exam>();
            QuestionBank = new HashSet<QuestionBank>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Exam> Exam { get; set; }
        public virtual ICollection<QuestionBank> QuestionBank { get; set; }
    }
}
