using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ExamineeController;
using Web.Models;
using Web.Repository;
using Web.Services.ExamService;

namespace Web.Services.ExamineeService
{
    public interface IExamineeService : ITransientService
    {
        NoPasswordExamDTO Get(Guid Id); // examId
        PasswordExamDTO Access(AccessExamDTO access, Guid userId);
        bool VerifyPassword(AccessExamDTO access);
        string TimeRemain(Guid examId, Guid userId);
        
    }
    public class ExamineeService : IExamineeService
    {
        private IAnswerRepository answerRepository;
        private IQuestionRepository questionRepository;
        private IExamRepository examRepository;
        private IExamService examService;
        private IScoreRepository scoreRepository;
        public ExamineeService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IExamRepository examRepository, IExamService examService, IScoreRepository socreRepository)
        {
            this.answerRepository = answerRepository;
            this.questionRepository = questionRepository;
            this.examRepository = examRepository;
            this.examService = examService;
            this.scoreRepository = socreRepository;
        }

        public PasswordExamDTO Access(AccessExamDTO access, Guid userId)
        {
            PasswordExamDTO returnDTO = new PasswordExamDTO();
            if (examService.IsRandom(access.Id))
            {
                // TODO: Gen Randrom exam
                throw new NotImplementedException();
                return returnDTO;
            }
            else
            {
                questionRepository.ListByExamId(access.Id).ForEach(x =>
                {
                    List<ExamineeAnswerDTO> examineeAnswers = new List<ExamineeAnswerDTO>();
                    answerRepository.ListByQuestionId(x.Id).ForEach(y => examineeAnswers.Add(new ExamineeAnswerDTO
                    {
                        Content = y.Content,
                        Id = y.Id
                    }));

                    returnDTO.ExamineeQuestions.Add(new ExamineeQuestionDTO
                    {
                        Id = x.Id,
                        Content = x.Content,
                        Answers = examineeAnswers,
                        Type = x.Type
                    });

                    returnDTO.TimeRemaining = TimeRemain(access.Id, userId);
                });

                return returnDTO;
            }

        }

        public bool VerifyPassword(AccessExamDTO access)
        {
            if (access.IsRandom)
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
            if (startTime.Equals(String.Empty))
            {
                if (examService.IsRandom(examId))
                {
                    return examRepository.GetRandomExam(examId).Time;
                }
            }
        }
    }
}
