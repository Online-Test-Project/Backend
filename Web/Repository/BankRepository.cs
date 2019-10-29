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
        int CountByOwnerId(Guid ownerId);

        bool Create(QuestionBank newBank);

        List<QuestionBank> ListByOwnerId(Guid ownerId);

        bool Update(QuestionBank updatedBank);

        bool Delete(Guid bankId);
    }

    public class BankRepository : IBankRepository
    {

        private OnlineTestContext DbContext;

        public BankRepository(OnlineTestContext _context)
        {
            DbContext = _context;
        }

        public int CountByOwnerId(Guid ownerId)
        {
            var query = from bank
                        in DbContext.QuestionBanks
                        where bank.OwnerId == ownerId
                        select bank.Id;
            return query.Count();
        }

        public bool Create(QuestionBank newBank)
        {
            try
            {
                DbContext.QuestionBanks.Add(newBank);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<QuestionBank> ListByOwnerId(Guid ownerId)
        {
            var query =
                from bank
                in DbContext.QuestionBanks
                where bank.OwnerId == ownerId
                select bank;
            return query.ToList();
        }

        public bool Update(QuestionBank updatedBank)
        {
            try
            {
                DbContext.QuestionBanks.Update(updatedBank);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
    }
}