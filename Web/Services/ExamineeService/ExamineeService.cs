using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ExamineeController;
using Web.Controllers.ReviewController;
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
        ReviewExamDTO CalculateMark(ExamAnswerDTO examAnswer, Guid userId);
        bool GetQuestionState(Guid questionId);
        string ExamStatus(string startTime, string endTime);
    }
    public class ExamineeService : IExamineeService
    {
        private IAnswerRepository answerRepository;
        private IQuestionRepository questionRepository;
        private IExamRepository examRepository;
        private IExamService examService;
        private IScoreRepository scoreRepository;
        private IBankService bankService;
        public static IDictionary<Guid, bool> QuestionState = new Dictionary<Guid, bool>();
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
            PasswordExamDTO returnDTO;
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
            NoPasswordExamDTO result;
            if (examService.IsRandom(Id))
            {
                RandomExam randomExam = examRepository.GetRandomExam(Id);
                result = new NoPasswordExamDTO
                {
                    IsRandom = true,
                    Name = randomExam.Name,
                    Time = randomExam.Time,

                    StartTime = DateTime.ParseExact(randomExam.StartTime, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture).ToString("dd-MM-yyyy hh:mm:ss"),
                    EndTime = DateTime.ParseExact(randomExam.EndTime, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture).ToString("dd-MM-yyyy hh:mm:ss"),

                    Status = ExamStatus(randomExam.StartTime, randomExam.EndTime)
                };
            }
            else
            {
                var examDetail = examRepository.Get(Id);
                result = new NoPasswordExamDTO
                {
                    IsRandom = false,
                    Name = examDetail.Name,
                    Time = examDetail.Time,

                    StartTime = DateTime.ParseExact(examDetail.StartTime, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture).ToString("dd-MM-yyyy hh:mm:ss"),
                    EndTime = DateTime.ParseExact(examDetail.EndTime, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CurrentCulture).ToString("dd-MM-yyyy hh:mm:ss"),

                    Status = ExamStatus(examDetail.StartTime, examDetail.EndTime)
                };
            }
            return result;
        }

        public string ExamStatus(string startTime, string endTime)
        {
            if (DateTime.Now < DateTime.Parse(startTime))
            {
                return "early";
            }
            return DateTime.Now < DateTime.Parse(endTime) ? "intime" : "late";
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
                        ExamName = examRepository.GetRandomExam(examId).Name,
                        RandomExamId = examId,
                        AnswerContent = String.Empty,
                        Score1 = 0,
                        StartTime = DateTime.Now.ToString(),
                        UserId = userId,
                        Time = String.Empty
                    }) ;
                    
                    return TimeSpan.FromMinutes(Double.Parse(examRepository.GetRandomExam(examId).Time)).TotalMilliseconds.ToString();
                }
                else
                {
                    scoreRepository.Create(new Score
                    {
                        Id = Guid.NewGuid(),
                        ExamName = examRepository.Get(examId).Name,
                        ExamId = examId,
                        AnswerContent = String.Empty,
                        Score1 = 0,
                        StartTime = DateTime.Now.ToString(),
                        UserId = userId,
                        Time = String.Empty
                    });
                    return TimeSpan.FromMinutes(Double.Parse(examRepository.Get(examId).Time)).TotalMilliseconds.ToString();
                }
            }
            else
            {
                Score recordedScore = scoreRepository.Get(examId, userId, isRandom);
                var timeSpent = DateTime.Now - DateTime.Parse(startTime);
                TimeSpan examTime = TimeSpan.FromMinutes(Double.Parse(isRandom ? examRepository.GetRandomExam(examId).Time : examRepository.Get(examId).Time));
                TimeSpan remainTime = examTime - timeSpent;
                return (timeSpent < examTime && recordedScore.Time.Equals(String.Empty)) ? remainTime.TotalMilliseconds.ToString() : String.Empty;
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

        public ReviewExamDTO CalculateMark(ExamAnswerDTO examAnswer, Guid userId)
        {
            ReviewExamDTO mark = new ReviewExamDTO();
            mark.TimeSpent = CalculateTimeSpent(examAnswer.ExamId, userId);
            int numsOfTrue = 0;
            examAnswer.AnswerDetails.ForEach(answer =>
            {
                var key = questionRepository.GetById(answer.QuestionId);
                if (key.Type == 3)
                {
                    if (answer.Content.Equals(key.Content))
                    {
                        QuestionState.Add(key.Id, true);
                        numsOfTrue++;
                    } 
                }
                else
                {
                    var userAnswers = answer.UserAnswers.Where(x => x.IsSelected == true).Select(x => x.AnswerId).OrderBy(x => x).ToList();
                    var trueAnswers = answerRepository.ListByQuestionId(key.Id).Where(x => x.IsCorrect == true).Select(x => x.Id).OrderBy(x => x).ToList();
                    if (string.Join("", userAnswers).Equals(string.Join("", trueAnswers)))
                    {
                        QuestionState.Add(key.Id, true);
                        numsOfTrue++;
                    }
                }
                if (!QuestionState.ContainsKey(key.Id))
                {
                    QuestionState.Add(key.Id, false);
                }
            });
            mark.Score = ((Double) numsOfTrue) / examRepository.Count(examAnswer.ExamId, examService.IsRandom(examAnswer.ExamId)) * 10;

            return mark;
        }

        private string CalculateTimeSpent(Guid examId, Guid userId)
        {
            string startTime = scoreRepository.GetTimeStamp(examId, userId);
            var examTime = examService.IsRandom(examId) ? examRepository.GetRandomExam(examId).Time : examRepository.Get(examId).Time;
            var timeSpent = TimeSpan.Parse(examTime) < (DateTime.Now - DateTime.Parse(startTime)) ? TimeSpan.Parse(examTime) : (DateTime.Now - DateTime.Parse(startTime));
            return timeSpent.ToString();
        }

        public bool GetQuestionState(Guid questionId)
        {
            bool result;
            return QuestionState.TryGetValue(questionId, out result) ? result : false;
             
        }
    }

}
