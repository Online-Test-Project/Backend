﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ExamineeController;
using Web.Models;
using Web.Repository;
using Web.Services.BankService;
using Web.Services.ExamService;

namespace Web.Services.ExamineeService
{
    public interface IExamineeService : ITransientService
    {
        NoPasswordExamDTO Get(Guid Id); // examId
        PasswordExamDTO Access(AccessExamDTO access, Guid userId);
        bool VerifyPassword(AccessExamDTO access);
        string TimeRemain(Guid examId, Guid userId);
        PasswordExamDTO GetFixedExam(Guid examId);
        PasswordExamDTO GetRandomExam(Guid examId);
        MarkDTO CalculateMark(ExamAnswerDTO examAnswer, Guid userId);
        
    }
    public class ExamineeService : IExamineeService
    {
        private IAnswerRepository answerRepository;
        private IQuestionRepository questionRepository;
        private IExamRepository examRepository;
        private IExamService examService;
        private IScoreRepository scoreRepository;
        private IBankService bankService;
        public ExamineeService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IExamRepository examRepository, IExamService examService, IScoreRepository socreRepository, IBankService bankService)
        {
            this.answerRepository = answerRepository;
            this.questionRepository = questionRepository;
            this.examRepository = examRepository;
            this.examService = examService;
            this.scoreRepository = socreRepository;
            this.bankService = bankService;
        }

        public PasswordExamDTO Access(AccessExamDTO access, Guid userId)
        {
            PasswordExamDTO returnDTO = new PasswordExamDTO();
            returnDTO = examService.IsRandom(access.Id) ? GetRandomExam(access.Id) : GetFixedExam(access.Id);
            return returnDTO;
        }

        public bool VerifyPassword(AccessExamDTO access)
        {
            if (examService.IsRandom(access.Id))
            {
                return examRepository.GetRandomExam(access.Id).Password == access.Password;
            }
            return examRepository.Get(access.Id).Password == access.Password;
        }

        public NoPasswordExamDTO Get(Guid Id)
        {
            if (examService.IsRandom(Id))
            {
                RandomExam randomExam = examRepository.GetRandomExam(Id);
                return new NoPasswordExamDTO
                {
                    IsRandom = true,
                    Name = randomExam.Name,
                    Time = randomExam.Time
                };
            }
            var examDetail = examRepository.Get(Id);
            return new NoPasswordExamDTO
            {
                IsRandom = false,
                Name = examDetail.Name,
                Time = examDetail.Time
            };
        }

        public string TimeRemain(Guid examId, Guid userId)
        {
            string startTime = scoreRepository.GetTimeStamp(examId, userId);
            bool isRandom = examService.IsRandom(examId);
            if (startTime.Equals(String.Empty))
            {
                if (isRandom)
                {
                    scoreRepository.Create(new Score
                    {
                        Id = Guid.NewGuid(),
                        RandomExamId = examId,
                        AnswerContent = String.Empty,
                        Score1 = 0,
                        StartTime = DateTime.Now.ToString(),
                        UserId = userId,
                        Time = String.Empty
                    }) ;
                    
                    return examRepository.GetRandomExam(examId).Time;
                }
                else
                {
                    scoreRepository.Create(new Score
                    {
                        Id = Guid.NewGuid(),
                        ExamId = examId,
                        AnswerContent = String.Empty,
                        Score1 = 0,
                        StartTime = DateTime.Now.ToString(),
                        UserId = userId,
                        Time = String.Empty
                    });
                    return examRepository.Get(examId).Time;
                }
            }
            else
            {
                Score recordedScore = scoreRepository.Get(examId, userId);
                var timeSpent = DateTime.Now - DateTime.Parse(startTime);
                string examTimeInString = isRandom ? examRepository.GetRandomExam(examId).Time : examRepository.Get(examId).Time;
                TimeSpan examTime = TimeSpan.Parse(examTimeInString);
                return (timeSpent < examTime && recordedScore.Time.Equals(String.Empty)) ? (examTime - timeSpent).ToString() : String.Empty;
            }
        }

        public PasswordExamDTO GetFixedExam(Guid examId)
        {
            PasswordExamDTO fixedExam = new PasswordExamDTO();
            questionRepository.ListByExamId(examId).ForEach(x =>
            {
                List<ExamineeAnswerDTO> examineeAnswers = new List<ExamineeAnswerDTO>();
                answerRepository.ListByQuestionId(x.Id).ForEach(y => examineeAnswers.Add(new ExamineeAnswerDTO
                {
                    Content = y.Content,
                    Id = y.Id
                }));

                fixedExam.ExamineeQuestions.Add(new ExamineeQuestionDTO
                {
                    Id = x.Id,
                    Content = x.Content,
                    Answers = examineeAnswers,
                    Type = x.Type
                });
            });

            fixedExam.TimeRemaining = String.Empty;
            return fixedExam;
        }

        public PasswordExamDTO GetRandomExam(Guid examId)
        {
            var exam = examRepository.GetRandomExam(examId);
            Guid bankId = exam.BankId;
            PasswordExamDTO examDTO = new PasswordExamDTO();
            examDTO.ExamineeQuestions.AddRange(bankService.ListRandomQuestion(bankId, exam.NumberOfEasyQuestion, 1));
            examDTO.ExamineeQuestions.AddRange(bankService.ListRandomQuestion(bankId, exam.NumberOfNormalQuestion, 2));
            examDTO.ExamineeQuestions.AddRange(bankService.ListRandomQuestion(bankId, exam.NumberOfHardQuestion, 3));

            examDTO.TimeRemaining = String.Empty;
            return examDTO;
        }

        public MarkDTO CalculateMark(ExamAnswerDTO examAnswer, Guid userId)
        {
            MarkDTO mark = new MarkDTO();
            mark.TimeSpent = CalculateTimeSpent(examAnswer.ExamId, userId);
        }

        private string CalculateTimeSpent(Guid examId, Guid userId)
        {
            string startTime = scoreRepository.GetTimeStamp(examId, userId);
            var examTime = examService.IsRandom(examId) ? examRepository.GetRandomExam(examId).Time : examRepository.Get(examId).Time;
            var timeSpent = TimeSpan.Parse(examTime) < (DateTime.Now - DateTime.Parse(startTime)
        }
    }

}
