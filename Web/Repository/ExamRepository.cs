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
        bool Create(ExamDTO exam, Guid userId);
        bool CreateRandom(RandomExamDTO randomExamDTO, Guid userId);
        List<RandomExam> ListRandomByUserId(Guid userId);
        RandomExam GetRandomExam(Guid examId); //ExamId
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
            Exam exam = new Exam
            {
                Id = Guid.NewGuid(),
                Password = RandomString(8),
                OwnerId = userId
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
                Time = randomExamDTO.Time
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
    }
}