using System;
using System.Collections.Generic;
using System.Linq;
using Web.Common;
using Web.Controllers.ExamineeController;
using Web.Controllers.QuestionController;
using Web.Models;
using Web.Repository;
using Web.Services.ExamService;

namespace Web.Services.BankService
{
    public interface IBankService : ITransientService
    {
        bool Delete(Guid bankId);
        List<ExamineeQuestionDTO> ListRandomQuestion(Guid bankId, int numsOfQuest, int difficulty);
        List<E> ShuffleList<E>(List<E> inputList);
    }
    public class BankService : IBankService 
    {
        private IQuestionRepository questionRepository;
        private IExamRepository examRepository;
        private IExamService examService;
        private IBankRepository bankRepository;
        private IAnswerRepository answerRepository;

        public BankService(IQuestionRepository questionRepository, IExamRepository examRepository, IExamService examService, IBankRepository bankRepository, IAnswerRepository answerRepository)
        {
            this.questionRepository = questionRepository;
            this.examRepository = examRepository;
            this.examService = examService;
            this.bankRepository = bankRepository;
            this.answerRepository = answerRepository;
        }

        public bool Delete(Guid bankId)
        {
            var listQuestion = questionRepository.ListByBankId(bankId);
            var listQuestionId = listQuestion.Select(x => x.Id).ToList();
            bool deleteQuest = questionRepository.Delete(listQuestionId);
            bool flag = true;
            var listExamId = examRepository.ListExamIdByBankId(bankId);
            listExamId.ForEach(x =>
            {
                if (!examService.Delete(x))
                {
                    flag = false;
                }
            });

            flag = bankRepository.Delete(bankId) && flag;

            return flag && deleteQuest;

        }

        public List<ExamineeQuestionDTO> ListRandomQuestion(Guid bankId, int numsOfQuest, int difficulty)
        {
            var listQuestionByBank = questionRepository.ListByBankId(bankId).Where(x => x.Difficulty == difficulty).ToList();
            int totalQuest = listQuestionByBank.Count;
            if (totalQuest == 0)
            {
                return null;
            }

            listQuestionByBank = ShuffleList(listQuestionByBank);

            Random r = new Random();
            List<ExamineeQuestionDTO> randomQuests = new List<ExamineeQuestionDTO>();
            for (int i = 0; i < numsOfQuest; i++)
            {

                var questionModel = listQuestionByBank[i % totalQuest];
                // add answer to Questions
                List<ExamineeAnswerDTO> examineeAnswers = new List<ExamineeAnswerDTO>();
                List<Answer> answers = answerRepository.ListByQuestionId(questionModel.Id);
                foreach (var answer in answers)
                {
                    examineeAnswers.Add(new ExamineeAnswerDTO
                    {
                        Content = answer.Content,
                        Id = answer.Id
                    }) ;
                }

                // add question to List
                randomQuests.Add(new ExamineeQuestionDTO
                {
                    Id = questionModel.Id,
                    Type = questionModel.Type,
                    Content = questionModel.Content,
                    Answers = examineeAnswers
                });
            }

            return randomQuests;
        }
        public List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
    }
}
