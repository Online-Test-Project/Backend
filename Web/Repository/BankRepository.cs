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

        bool Create(BankDTO bank);

        List<QuestionBank> List(Guid userId);

        // bool Update(BankDTO bank);

        bool Delete(Guid bankId);
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
            var temp = DbContext.QuestionBanks.Where(b => b.OwnerId == user.Id).Select(x => x.Id);
            int returnn = temp.Count();
            return returnn;
        }

        public bool Create(BankDTO bank)
        {
            try
            {
                DbContext.QuestionBanks.Add(new QuestionBank
                {
                    Id = Guid.NewGuid(),
                    Description = bank.Description,
                    ModifiedDate = bank.ModifiedDate,
                    Name = bank.Name,
                    OwnerId = bank.Id
                });
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<QuestionBank> List(Guid userId)
        {
            var query =
                from bank
                in DbContext.QuestionBanks
                where bank.OwnerId == userId
                select bank;
            return query.ToList();
        }

        public bool Delete(Guid bankId)
        {
            return true;
        }
        
    }
}