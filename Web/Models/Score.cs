using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Score
    {
        public Guid UserId { get; set; }
        public Guid ExamId { get; set; }
        public int Score1 { get; set; }
        public string Time { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual RandomExam ExamNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
