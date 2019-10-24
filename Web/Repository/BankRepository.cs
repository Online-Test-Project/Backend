using System;
using System.Collections.Generic;
using System.Linq;
using Web.AppStart;
using Web.Common;
using Web.Controllers.BankController;
using Web.Models;

namespace Web.Repository
{
    interface IBankRepository : IScopedService
    {
        int Count(UserDTO user);
    }

    public class BankRepository : IBankRepository
    {

        private OnlineTestContext DbContext;

        public BankRepository(OnlineTestContext _context)
        {
            DbContext = _context;
        }

        public int Count(UserDTO user)
        {
            var temp = DbContext.QuestionBanks.Where(b => b.OwnerId == user.UserId).Select(x => x.Id);
            int returnn = temp.Count();
            return returnn;
        }
    }
}