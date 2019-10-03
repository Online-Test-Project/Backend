using System;
using System.Collections.Generic;
using System.Linq;
using Web.Controllers.BankController;
using Web.Models;

namespace Web.Repository
{
    
    interface IBankRepository
    {
        bool Create(BankDTO bank);

        List<QuestionBank> Read(Guid userId);

        bool Update(BankDTO bank);

        bool Delete(Guid bankId);
    }

    public class BankRepository : IBankRepository
    {

        private OnlineTestContext DbContext;

        public BankRepository(OnlineTestContext _context)
        {
            DbContext = _context;
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

        public bool Update(BankDTO bank)
        {
            try
            {
                var query =
                    from b
                    in DbContext.QuestionBanks
                    where b.Id == bank.Id
                    select b;
                QuestionBank questionBank = query.First();

                questionBank.Name = bank.Name;
                questionBank.Description = bank.Description;
                questionBank.ModifiedDate = bank.ModifiedDate;

                DbContext.QuestionBanks.Update(questionBank);
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