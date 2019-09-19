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
            RandomExam = new HashSet<RandomExam>();
            Score = new HashSet<Score>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual UserDetail UserDetail { get; set; }
        public virtual ICollection<Exam> Exam { get; set; }
        public virtual ICollection<QuestionBank> QuestionBank { get; set; }
        public virtual ICollection<RandomExam> RandomExam { get; set; }
        public virtual ICollection<Score> Score { get; set; }
    }
}
