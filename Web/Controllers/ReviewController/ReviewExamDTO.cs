using System;

namespace Web.Controllers.ReviewController
{
    public class ReviewExamDTO
    {
        public Guid ExamId;
        public String TimeSpent;
        public Double Score;
        public String Date;
        public int NumsOfTrue;
        public int TotalQuest;
    }    
}