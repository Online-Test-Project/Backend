using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Services.BankService;

namespace Web.Services.ScoreService
{
    public interface IScoreService : ITransientService
    {
        //bool Update(Guid examId, Guid userId);
    }
    public class ScoreService : IScoreService
    {
        private IBankService bankService;

        public ScoreService(IBankService bankService)
        {
            this.bankService = bankService;
        }

    }
}
