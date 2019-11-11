//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Web.Common;
//using Web.Controllers.ExamineeController;
//using Web.Models;
//using Web.Repository;
//using Web.Services.ExamService;

//namespace Web.Services.ExamineeService
//{
//    public interface IExamineeService : ITransientService
//    {
//        NoPasswordExamDTO Get(Guid Id); // examId
//        PasswordExamDTO Access(AccessExamDTO access);
//        bool VerifyPassword(AccessExamDTO access);
//    }
//    public class ExamineeService : IExamineeService
//    {
//        private IAnswerRepository answerRepository;
//        private IQuestionRepository questionRepository;
//        private IExamRepository examRepository;
//        private IExamService examService;
//        public ExamineeService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IExamRepository examRepository, IExamService examService)
//        {
//            this.answerRepository = answerRepository;
//            this.questionRepository = questionRepository;
//            this.examRepository = examRepository;
//            this.examService = examService;
//        }

//        public PasswordExamDTO Access(AccessExamDTO access)
//        {
//            PasswordExamDTO returnDTO = new PasswordExamDTO();
//            if (access.IsRandom)
//            {

//            }
//            else
//            {
//                questionRepository.ListByExamId(access.Id).ForEach(x =>
//                {
//                    List<ExamineeAnswerDTO> examineeAnswers = new List<ExamineeAnswerDTO>();
//                    answerRepository.ListByQuestionId(x.Id).ForEach(y => examineeAnswers.Add(new ExamineeAnswerDTO
//                    {
//                        Content = y.Content,
//                        Id = y.Id
//                    }));

//                    returnDTO.ExamineeQuestions.Add(new ExamineeQuestionDTO
//                    {
//                        Id = x.Id,
//                        Content = x.Content,
//                        Answers = examineeAnswers,
//                        Type = x.Type
//                    });

//                    // TODO: Time remaining
//                });

//            }

//        }

//        public bool VerifyPassword(AccessExamDTO access)
//        {
//            if (access.IsRandom)
//            {
//                return examRepository.GetRandomExam(access.Id).Password == access.Password;
//            }
//            return examRepository.Get(access.Id).Password == access.Password;
//        }

//        public NoPasswordExamDTO Get(Guid Id)
//        {
//            if (examService.IsRandom(Id))
//            {
//                RandomExam randomExam = examRepository.GetRandomExam(Id);
//                return new NoPasswordExamDTO
//                {
//                    IsRandom = true,
//                    Name = randomExam.Name,
//                    Time = randomExam.Time
//                };
//            }
//            var examDetail = examRepository.Get(Id);
//            return new NoPasswordExamDTO
//            {
//                IsRandom = false,
//                Name = examDetail.Name,
//                Time = examDetail.Time
//            };
//        }
//    }
//}
