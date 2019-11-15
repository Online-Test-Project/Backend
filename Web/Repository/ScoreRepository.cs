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
    }
    public class ScoreRepository : IScoreRepository
    {
        private OnlineTestContext DbContext = new OnlineTestContext();

        public ScoreRepository(OnlineTestContext dbContext)
        {
            DbContext = dbContext;
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
    }
}
