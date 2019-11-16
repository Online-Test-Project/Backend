using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.ReviewController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        [HttpGet]
        public List<ReviewExamDTO> List()
        {
            List<ReviewExamDTO> reviewExams = new List<ReviewExamDTO>();
            reviewExams.Add(new ReviewExamDTO
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Date = DateTime.Now.ToString(),
                Score = 8.5,
                Time = "15:27"
            });

            reviewExams.Add(new ReviewExamDTO
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab"),
                Date = DateTime.Now.ToString(),
                Score = 8.5,
                Time = "15:27"
            });

            return reviewExams;
        }

        [HttpGet, Route("{Id}")]
        public ReviewExamDetailDTO Get(Guid Id)
        {
            var quests = new List<ReviewQuestionDTO>();

            quests.Add(new ReviewQuestionDTO
            {
                Content = "Thúy có phải người xinh đẹp nhất thế giới?",
                IsCorrect = false,
                Type = 1,
                ReviewAnswers = new List<ReviewAnswerDTO>
                {
                    new ReviewAnswerDTO
                    {
                        Content = "Sai",
                        IsSelected = false
                    },
                    new ReviewAnswerDTO
                    {
                        Content = "Đúng",
                        IsSelected = true
                    }
                }
            });

            quests.Add(new ReviewQuestionDTO
            {
                Content = "Ai là người code ít bug nhất?",
                IsCorrect = true,
                Type = 1,
                ReviewAnswers = new List<ReviewAnswerDTO>
                {
                    new ReviewAnswerDTO
                    {
                        Content = "Thành",
                        IsSelected = true
                    },
                    new ReviewAnswerDTO
                    {
                        Content = "Minh",
                        IsSelected = false
                    },
                    new ReviewAnswerDTO
                    {
                        Content = "Hậu",
                        IsSelected = false
                    },
                    new ReviewAnswerDTO
                    {
                        Content = "Ngọc",
                        IsSelected = false
                    }
                }
            });

            quests.Add(new ReviewQuestionDTO
            {
                Content = "Ai sinh ra nhiều bug hơn 2 người còn lại",
                IsCorrect = false,
                Type = 2,
                ReviewAnswers = new List<ReviewAnswerDTO>
                {
                    new ReviewAnswerDTO
                    {
                        Content = "Thành",
                        IsSelected = true
                    },
                    new ReviewAnswerDTO
                    {
                        Content = "Thúy",
                        IsSelected = false
                    },
                    new ReviewAnswerDTO
                    {
                        Content = "Minh",
                        IsSelected = true
                    },
                    new ReviewAnswerDTO
                    {
                        Content = "Ngọc",
                        IsSelected = false
                    }
                }
            });

            quests.Add(new ReviewQuestionDTO
            {
                Content = "Ai là người sẽ ế đến già?",
                IsCorrect = false,
                Type = 3,
                ReviewAnswers = new List<ReviewAnswerDTO>
                {
                    new ReviewAnswerDTO
                    {
                        Content = "Thành",
                        IsSelected = true
                    }
                }
            });

            return new ReviewExamDetailDTO
            {
                Time = DateTime.Now.ToString(),
                Score = 8,
                ReviewQuestions = quests
            };
        }
    }
}