using System;
using System.Collections.Generic;

namespace Web.Controllers.ReviewController
{
    public class ReviewExamDetailDTO
    {
        public string Name;
        public string Time;
        public Double Score;
        public List<ReviewQuestionDTO> ReviewQuestions;
    }
}