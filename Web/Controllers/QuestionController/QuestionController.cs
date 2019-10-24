using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Repository;

namespace Web.Controllers.QuestionController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : MyController
    {
        public QuestionController(OnlineTestContext _context) : base(_context)
        {
            
        }

        [Route("{id}")]
        [HttpGet]
        public List<QuestionDTO> List(Guid bankId)
        {
            IQuestionRepository questionRepository = new QuestionRepository(DbContext);
            IAnswerRepository answerRepository = new AnswerRepository(DbContext);

            List<Question> questions = questionRepository.List(bankId);
            List<QuestionDTO> result = new List<QuestionDTO>();

            foreach (Question question in questions)
            {
                List<Answer> answers = answerRepository.List(question.Id);
                List<AnswerDTO> answerDTOs = new List<AnswerDTO>();
                foreach (Answer a in answers)
                {
                    answerDTOs.Add(new AnswerDTO
                    {
                        Content = a.Content,
                        IsCorrect = a.IsCorrect
                    });
                }
                result.Add(new QuestionDTO
                {
                    Id = question.Id,
                    Difficulty = question.Difficulty,
                    Type = question.Type,
                    Content = question.Content,
                    Answers = answerDTOs
                });
            }

            return result;
        }

        public bool Update(QuestionDTO questionDTO)
        {
            IQuestionRepository questionRepository = new QuestionRepository(DbContext);
            IAnswerRepository answerRepository = new AnswerRepository(DbContext);

            List<Answer> updateAnswers = new List<Answer>();
            foreach (AnswerDTO a in questionDTO.Answers)
            {
                updateAnswers.Add(new Answer
                {
                    Id = Guid.NewGuid(),
                    Content = a.Content,
                    IsCorrect = a.IsCorrect,
                    QuestionId = questionDTO.Id
                });
            }

            Question updateQuestion = questionRepository.Read(questionDTO.Id);
            updateQuestion.Content = questionDTO.Content;
            updateQuestion.Difficulty = questionDTO.Difficulty;

            return answerRepository.Update(updateAnswers) && questionRepository.Update(updateQuestion);
        }

        public bool Create(QuestionDTO questionDTO)
        {
            IQuestionRepository questionRepository = new QuestionRepository(DbContext);
            IAnswerRepository answerRepository = new AnswerRepository(DbContext);

            Question newQuestion = new Question
            {
                Id = Guid.NewGuid(),
                BankId = questionDTO.Id,
                Difficulty = questionDTO.Difficulty,
                Type = questionDTO.Type,
                Content = questionDTO.Content
            };

            List<Answer> newAnswers = new List<Answer>();
            foreach (AnswerDTO a in questionDTO.Answers)
            {
                newAnswers.Add(new Answer
                {
                    Id = Guid.NewGuid(),
                    Content = a.Content,
                    IsCorrect = a.IsCorrect,
                    QuestionId = newQuestion.Id
                });
            }
            return questionRepository.Create(newQuestion) && answerRepository.Create(newAnswers);
        }

        public bool Delete(Guid questionId)
        {
            IQuestionRepository questionRepository = new QuestionRepository(DbContext);
            IAnswerRepository answerRepository = new AnswerRepository(DbContext);

            return answerRepository.Delete(questionId) && questionRepository.Delete(questionId);
        }
    }
}
