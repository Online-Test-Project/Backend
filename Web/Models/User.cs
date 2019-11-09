using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class User
    {
        public User()
        {
            Exams = new HashSet<Exam>();
            QuestionBanks = new HashSet<QuestionBank>();
            RandomExams = new HashSet<RandomExam>();
            Scores = new HashSet<Score>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual UserDetail UserDetail { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<QuestionBank> QuestionBanks { get; set; }
        public virtual ICollection<RandomExam> RandomExams { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
