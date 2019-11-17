using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ReviewController;
using Web.Repository;
using Web.Services.ExamService;

namespace Web.Services.ReviewService
{
    public interface IReviewService : ITransientService
    {
        //List<ReviewExamDTO> List(Guid userId);
    }
    public class ReviewService : IReviewService
    {
        private IScoreRepository scoreRepository;
        private IExamService examService;

        public ReviewService(IScoreRepository scoreRepository, IExamService examService)
        {
            this.scoreRepository = scoreRepository;
            this.examService = examService;
        }
    }
}
