using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.QuestionController;
using Web.Models;
using Web.Repository;

namespace Web.Controllers.ExamController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamController : MyController
    {
        private IQuestionRepository questionRepository;
        private IAnswerRepository answerRepository;

        public ExamController(IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
        }

        [HttpGet("{Id}")]
        public List<QuestionDTO> Get(Guid Id)
        {
            List<QuestionDTO> responseList = new List<QuestionDTO>();
            

            return responseList;
        }
    }
}