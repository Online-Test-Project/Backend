using System;
namespace Web.Controllers.ReviewController
{
    public class ReviewExamDetailDTO
    {
       public string Time;
       public Double Score;
       public List<ReviewQuestionDTO> ReviewQuestions;
    }
}