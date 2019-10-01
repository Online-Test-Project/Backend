using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Models.DTO;

namespace Web.Repository
{
    
    interface IBankRepository
    {
        Boolean Create(BankDTO bank);

        Task<bool> Update(BankDTO bank);

        Task<bool> Delete(Guid bankId);

    }
    public class BankRepository : IBankRepository
    {
        Guid emptyGuid = new Guid("00000000-0000-0000-0000-000000000000");
        private OnlineTestContext DbContext;
        public BankRepository(OnlineTestContext _context)
        {
            DbContext = _context;
        }
        public Boolean Create(BankDTO bank)
        {
            DbContext.QuestionBanks.Add(new QuestionBank
            {
                Id = Guid.NewGuid(),
                Description = bank.Description,
                ModifiedDate = bank.ModifiedDate,
                Name = bank.Name,
                OwnerId = emptyGuid,
            });
            DbContext.SaveChanges();
            return true;

        }

        public Task<bool> Delete(Guid bankId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BankDTO bank)
        {
            throw new NotImplementedException();
        }
    }
}