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
        private IExamRepository examRepository;

        public ExamController(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IExamRepository examRepository)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
            this.examRepository = examRepository;
        }

        [HttpGet("{Id}")]
        public List<QuestionDTO> Get(Guid Id) // Id of Exam
        {
            List<QuestionDTO> responseList = new List<QuestionDTO>();
            List<Question> questions = questionRepository.ListByExamId(Id);
            foreach (var question in questions)
            {
                // add answer to Questions
                List<AnswerDTO> answerDTOs = new List<AnswerDTO>();
                List<Answer> answers = answerRepository.ListByQuestionId(question.Id);
                foreach (var answer in answers)
                {
                    answerDTOs.Add(new AnswerDTO
                    {
                        Content = answer.Content,
                        IsCorrect = answer.IsCorrect
                    });
                }

                // add question to List
                responseList.Add(new QuestionDTO
                {
                    Id = question.Id,
                    Difficulty = question.Difficulty,
                    Type = question.Type,
                    Content = question.Content,
                    Answers = answerDTOs
                });
            }
           
            return responseList;
        }

        public List<ExamDetailDTO> List()
        {
            List<ExamDetailDTO> responseList = new List<ExamDetailDTO>();
            examRepository.ListByUserId(user.Id).ForEach(x => responseList.Add(new ExamDetailDTO
            {
                Id = x.Id,
                Time = x.Time,
                Name = x.Name,
                Password = x.Password
            }));

            return responseList;
        }

        public bool Create([FromBody] ExamDTO exam)
        {
            return examRepository.Create(exam, user.Id);
            
        }
    }

}