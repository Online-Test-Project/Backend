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
        private IQuestionRepository questionRepository;
        private IAnswerRepository answerRepository;
        public QuestionController(IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
        }

        [Route("{Id}")]
        [HttpGet]
        public List<QuestionDTO> List(Guid Id)
        {
            

            List<Question> questions = questionRepository.ListByBankId(Id);
            List<QuestionDTO> result = new List<QuestionDTO>();

            foreach (Question question in questions)
            {
                List<Answer> answers = answerRepository.ListByQuestionId(question.Id);
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

            List<Answer> updatedAnswers = new List<Answer>();
            foreach (AnswerDTO answerDTO in questionDTO.Answers)
            {
                updatedAnswers.Add(new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = questionDTO.Id,
                    Content = answerDTO.Content,
                    IsCorrect = answerDTO.IsCorrect                    
                });
            }

            Question updatedQuestion = questionRepository.GetById(questionDTO.Id);
            updatedQuestion.Content = questionDTO.Content;
            updatedQuestion.Difficulty = questionDTO.Difficulty;

            return answerRepository.Update(updatedAnswers) && questionRepository.Update(updatedQuestion);
        }

        public bool Create(QuestionDTO questionDTO)
        {

            Question newQuestion = new Question {
                Id = Guid.NewGuid(),
                BankId = questionDTO.Id,
                Difficulty = questionDTO.Difficulty,
                Type = questionDTO.Type,
                Content = questionDTO.Content
            };

            List<Answer> newAnswers = new List<Answer>();
            foreach (AnswerDTO answerDTO in questionDTO.Answers)
            {
                newAnswers.Add(new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = newQuestion.Id,
                    Content = answerDTO.Content,
                    IsCorrect = answerDTO.IsCorrect                    
                });
            }
            return questionRepository.Create(newQuestion) && answerRepository.Create(newAnswers);
        }

        
        public bool Delete([FromBody]Guid Id)
        {
            return questionRepository.Delete(Id) && answerRepository.DeleteByQuestionId(Id);
        }
    }
}
