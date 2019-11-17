using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ReviewController;
using Web.Repository;
using Web.Services.ExamService;
using Web.Services.ScoreService;

namespace Web.Services.ReviewService
{
    public interface IReviewService : ITransientService
    {
        List<ReviewExamDTO> List(Guid userId);
        ReviewExamDetailDTO GetAnswerContentObj(Guid examId, Guid userId);
    }
    public class ReviewService : IReviewService
    {
        private IScoreRepository scoreRepository;
        private IExamService examService;
        private IScoreService scoreService;

        public ReviewService(IScoreRepository scoreRepository, IExamService examService, IScoreService scoreService)
        {
            this.scoreRepository = scoreRepository;
            this.examService = examService;
            this.scoreService = scoreService;
        }

        public ReviewExamDetailDTO GetAnswerContentObj(Guid examId, Guid userId)
        {
            string answerContent = scoreService.GetAnswerContent(examId, userId);
            return JsonConvert.DeserializeObject<ReviewExamDetailDTO>(answerContent);
        }

        public List<ReviewExamDTO> List(Guid userId)
        {
            var scoreList = scoreRepository.List(userId);
            List<ReviewExamDTO> myList = new List<ReviewExamDTO>();
            scoreList.ForEach(x =>
            {
                var examId = (x.ExamId != null) ? x.ExamId : x.RandomExamId;
                myList.Add(new ReviewExamDTO
                {
                    Date = DateTime.Parse(x.StartTime).ToString("dd-MM-yyyy"),
                    ExamId = (Guid)examId,
                    Name = x.ExamName,
                    Score = x.Score1,
                    TimeSpent = x.Time,
                });
            });
            return myList;
        }


    }
}
