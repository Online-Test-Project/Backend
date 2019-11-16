namespace Web.Controllers.ReviewController
{
    public class ReviewQuestionDTO
    {
        public string Content;
        public bool IsCorrect;
        public List<ReviewAnswerDTO> ReviewAnswers;
        public int Type;
    }
}