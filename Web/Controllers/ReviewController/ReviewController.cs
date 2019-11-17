using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Services.ReviewService;

namespace Web.Controllers.ReviewController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewController : MyController
    {
        private IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet]
        public List<ReviewExamDTO> List()
        {
            return reviewService.List(user.Id);
        }

        [HttpGet, Route("{Id}")]
        public ReviewExamDetailDTO Get(Guid Id)
        {
            return reviewService.GetAnswerContentObj(Id, user.Id);
        }
    }
}