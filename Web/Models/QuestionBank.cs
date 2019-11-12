using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class QuestionBank
    {
        public QuestionBank()
        {
            Exams = new HashSet<Exam>();
            Questions = new HashSet<Question>();
            RandomExams = new HashSet<RandomExam>();
        }

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User Owner { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<RandomExam> RandomExams { get; set; }
    }
}
