using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ExamController;
using Web.Controllers.QuestionController;
using Web.Models;

namespace Web.Repository
{
    public interface IExamRepository : ITransientService
    {
        Exam Get(Guid examId);
        List<Exam> ListByUserId(Guid userId);
        int Count(Guid examId, bool IsRandom);
        bool Create(ExamDTO exam, Guid userId);
        bool CreateRandom(RandomExamDTO randomExamDTO, Guid userId);
        List<RandomExam> ListRandomByUserId(Guid userId);
        RandomExam GetRandomExam(Guid examId); 
        bool Delete(Guid Id, bool IsRandom);
        List<Guid> ListExamIdByBankId(Guid bankId);
    }
    public class ExamRepository : IExamRepository
    {

        private OnlineTestContext DbContext;

        public ExamRepository(OnlineTestContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool Create(ExamDTO examDTO, Guid userId)
        {
            try
            {
                Exam exam = new Exam
                {
                    Id = Guid.NewGuid(),
                    Password = RandomString(8),
                    OwnerId = userId,
                    Time = examDTO.Time,
                    Name = examDTO.Name,
                    BankId = examDTO.BankId,
                    StartTime = examDTO.StartTime,
                    EndTime = examDTO.EndTime,
                    Description = examDTO.Description
                };
                DbContext.Exams.Add(exam);

                examDTO.QuestionId.ForEach(x => DbContext.ExamQuestions.Add(new ExamQuestion
                {
                    QuestionId = x,
                    ExamId = exam.Id,
                }));

                DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Count(Guid examId, bool IsRandom)
        {
            if (IsRandom)
            {
                RandomExam randomExam = GetRandomExam(examId);
                return randomExam.NumberOfEasyQuestion + randomExam.NumberOfNormalQuestion + randomExam.NumberOfHardQuestion;
            }
            return DbContext.ExamQuestions.Where(x => x.ExamId == examId).Count();
        }

        public Exam Get(Guid examId)
        {
            return DbContext.Exams.Where(x => x.Id == examId).FirstOrDefault();
        }

        public List<Exam> ListByUserId(Guid userId)
        {
            return DbContext.Exams.Where(x => x.OwnerId == userId).ToList();
        }
        public bool CreateRandom(RandomExamDTO randomExamDTO, Guid userId)
        {
            RandomExam exam = new RandomExam
            {
                Id = Guid.NewGuid(),
                Password = RandomString(8),
                OwnerId = userId,
                BankId = randomExamDTO.BankId,
                NumberOfEasyQuestion = randomExamDTO.Difficulty[0],
                NumberOfNormalQuestion = randomExamDTO.Difficulty[1],
                NumberOfHardQuestion = randomExamDTO.Difficulty[2],
                StartTime = randomExamDTO.StartTime,
                EndTime = randomExamDTO.EndTime,
                Time = randomExamDTO.Time,
                Name = randomExamDTO.Name,
                Description = randomExamDTO.Description
            };
            DbContext.RandomExams.Add(exam);  

            DbContext.SaveChanges();
            return true;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public List<RandomExam> ListRandomByUserId(Guid userId)
        {
            return DbContext.RandomExams.Where(x => x.OwnerId == userId).ToList();
        }

        public RandomExam GetRandomExam(Guid examId)
        {
            return DbContext.RandomExams.Where(x => x.Id == examId).FirstOrDefault();
        }

        public bool Delete(Guid Id, bool IsRandom)
        {
            try
            {
                if (IsRandom)
                {
                    var deleteScore = DbContext.Scores.Where(x => x.RandomExamId == Id);
                    DbContext.Scores.RemoveRange(deleteScore);

                    var deleteRandomExam = DbContext.RandomExams.Where(x => x.Id == Id).FirstOrDefault();
                    DbContext.RandomExams.Remove(deleteRandomExam);

                }
                else
                {
                    var deleteScore = DbContext.Scores.Where(x => x.ExamId == Id);
                    DbContext.Scores.RemoveRange(deleteScore);

                    var delete = DbContext.Exams.Where(x => x.Id == Id).FirstOrDefault();
                    DbContext.Exams.Remove(delete);
                }
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
           
        }

        public List<Guid> ListExamIdByBankId(Guid bankId)
        {
            List<Guid> returnList = new List<Guid>();
            var nonRandList = DbContext.Exams.Where(x => x.BankId == bankId);
            var randList = DbContext.RandomExams.Where(x => x.BankId == bankId);
            returnList.AddRange(nonRandList.Select(x => x.Id));
            returnList.AddRange(randList.Select(x => x.Id));
            return returnList;
        }
    }
}