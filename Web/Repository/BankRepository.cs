using System;
using System.Collections.Generic;
using System.Linq;
using Web.Controllers.BankController;
using Web.Models;

namespace Web.Repository
{
    interface IBankRepository
    {
        bool Create(QuestionBank bank);

        List<QuestionBank> Read(Guid userId);

        bool Update(QuestionBank bank);

        bool Delete(Guid bankId);
    }

    public class BankRepository : IBankRepository
    {

        private OnlineTestContext DbContext;

        public BankRepository(OnlineTestContext _context)
        {
            DbContext = _context;
        }

        public bool Create(QuestionBank bank)
        {
            try
            {
                DbContext.QuestionBanks.Add(bank);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<QuestionBank> Read(Guid userId)
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
            try
            {
                var query =
                    from bank
                    in DbContext.QuestionBanks
                    where bank.Id == bankId
                    select bank;
                DbContext.QuestionBanks.Remove(query.First());
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }            
        }

        public bool Update(QuestionBank bank)
        {
            try
            {
                DbContext.QuestionBanks.Update(bank);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}