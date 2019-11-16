using System;
using System.Collections.Generic;
using System.Linq;
using Web.Common;
using Web.Models;

namespace Web.Repository
{
    public interface IBankRepository : ITransientService
    {
        QuestionBank Get(Guid bankId);

        bool Create(QuestionBank newBank);

        List<QuestionBank> ListByOwnerId(Guid ownerId);

        List<int> CountByType(Guid bankId);

        List<int> CountByDifficulty(Guid bankId);

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

        public bool Create(QuestionBank newBank)
        {
            try
            {
                DbContext.QuestionBanks.Add(newBank);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
            {
                return false;
            }
        }

        public List<int> CountByType(Guid bankId)
        {
            List<int> result = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                result.Add(DbContext.Questions.Where(x => x.BankId == bankId && x.Type == i).Count());
            }
            return result;
        }

        public List<int> CountByDifficulty(Guid bankId)
        {
            List<int> result = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                result.Add(DbContext.Questions.Where(x => x.BankId == bankId && x.Difficulty == i).Count());
            }
            return result;
        }

        public QuestionBank Get(Guid bankId)
        {
            return DbContext.QuestionBanks.Where(x => x.Id == bankId).FirstOrDefault();
        }
    }
}