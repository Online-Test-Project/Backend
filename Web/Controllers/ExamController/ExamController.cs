using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.QuestionController;
using Web.Models;
using Web.Repository;
using Web.Services.ExamService;

namespace Web.Controllers.ExamController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamController : MyController
    {
        private IQuestionRepository questionRepository;
        private IAnswerRepository answerRepository;
        private IExamRepository examRepository;
        private IBankRepository bankRepository;
        private IExamService examService;

        public ExamController(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IExamRepository examRepository, IBankRepository bankRepository, IExamService examService)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
            this.examRepository = examRepository;
            this.bankRepository = bankRepository;
            this.examService = examService;
        }

        [HttpGet, Route("{Id}")]
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

        [HttpGet, Route("{Id}")]
        public RandomExamDetailDTO GetRandom(Guid Id) // ExamId
        {
            string bankName = bankRepository.ListByOwnerId(user.Id).Where(x => x.Id == Id).Select(x => x.Name).FirstOrDefault();
            var exam = examRepository.GetRandomExam(Id);
            return new RandomExamDetailDTO
            {
                BankId = exam.BankId,
                BankName = bankName,
                Name = exam.Name,
                NumberOfEasyQuestion = exam.NumberOfEasyQuestion,
                NumberOfHardQuestion = exam.NumberOfHardQuestion,
                NumberOfNormalQuestion = exam.NumberOfNormalQuestion,
                Time = exam.Time
            };
        }
        public List<ExamDetailDTO> List()
        {
            List<ExamDetailDTO> responseList = new List<ExamDetailDTO>();
            examRepository.ListByUserId(user.Id).ForEach(x => responseList.Add(new ExamDetailDTO
            {
                Id = x.Id,
                Time = x.Time,
                Name = x.Name,
                Description = x.Description,
                Password = x.Password,
                IsRandom = false
            }));

            examRepository.ListRandomByUserId(user.Id).ForEach(x => responseList.Add(new ExamDetailDTO
            {
                Id = x.Id,
                Time = x.Time,
                Name = x.Name,
                Description = x.Description,
                Password = x.Password,
                IsRandom = true
            }));

            return responseList;
        }

        public bool Create([FromBody] ExamDTO exam)
        {
            return examRepository.Create(exam, user.Id);
        }

        public bool CreateRandom([FromBody] RandomExamDTO randomExam)
        {
            return examRepository.CreateRandom(randomExam, user.Id);
        }

        public bool Delete([FromBody] Guid examId)
        {
            return examRepository.Delete(examId, examService.IsRandom(examId));
        }
    }
}