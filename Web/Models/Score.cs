using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Score
    {
        public Guid UserId { get; set; }
        public Guid? ExamId { get; set; }
        public int Score1 { get; set; }
        public string Time { get; set; }
        public string AnswerContent { get; set; }
        public string StartTime { get; set; }
        public Guid Id { get; set; }
        public Guid? RandomExamId { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual RandomExam RandomExam { get; set; }
        public virtual User User { get; set; }
    }
}
