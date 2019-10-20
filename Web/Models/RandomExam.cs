using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class RandomExam
    {
        public RandomExam()
        {
            Scores = new HashSet<Score>();
        }

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid BankId { get; set; }
        public int NumberOfEasyQuestion { get; set; }
        public int NumberOfNormalQuestion { get; set; }
        public int NumberOfHardQuestion { get; set; }
        public TimeSpan Time { get; set; }

        public virtual QuestionBank Bank { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
