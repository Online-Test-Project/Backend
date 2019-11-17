using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Models;

namespace Web.Repository
{
    public interface IScoreRepository : ITransientService
    {
        string GetTimeStamp(Guid examId, Guid userId);
        bool Create(Score score);
        Score Get(Guid examId, Guid userId, bool IsRandom);
        bool Update(Score score);
        List<Score> List(Guid userId);
    }
    public class ScoreRepository : IScoreRepository
    {
        private OnlineTestContext DbContext = new OnlineTestContext();

        public ScoreRepository(OnlineTestContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool Create(Score score)
        {
            try
            {
                DbContext.Scores.Add(score);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public Score Get(Guid examId, Guid userId, bool IsRandom)
        {
            if (IsRandom)
            {
                return DbContext.Scores.Where(x => x.RandomExamId == examId && x.UserId == userId).FirstOrDefault();
            }
            else
            {
                return DbContext.Scores.Where(x => x.ExamId == examId && x.UserId == userId).FirstOrDefault();
            }
        }

        public string GetTimeStamp(Guid examId, Guid userId)
        {
            var recoredTime = DbContext.Scores.Where(x => x.ExamId == examId && x.UserId == userId);
            if (recoredTime.Count() == 0)
            {
                return String.Empty;
            }
            else
            {
                return recoredTime.Select(x => x.StartTime).FirstOrDefault();
            }
        }

        public List<Score> List(Guid userId)
        {
            return DbContext.Scores.Where(x => x.UserId == userId).ToList();
        }

        public bool Update(Score score)
        {
            try
            {
                DbContext.Scores.Update(score);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
