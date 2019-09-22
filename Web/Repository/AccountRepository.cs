using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Repository
{
    public interface IAccountRepository
    {
        Task<bool> Create(Account account);
        Task<bool> Update(Account account);
        Task<bool> Delete(Guid accountId); 
    }
    public class AccountRepository : IAccountRepository
    {
        private OnlineTestContext DbContext;
        public AccountRepository(OnlineTestContext _DbContext)
        {
            DbContext = _DbContext;
        }

        public async Task<bool> Create(Account account)
        {
            DbContext.Accounts.Add(account);
            DbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(Guid accountId)
        {
            Account deleteAcount = DbContext.Accounts.Where(c => c.Id == accountId).FirstOrDefault();
            DbContext.Accounts.Remove(deleteAcount);
            return true;
        }

        public Task<bool> Update(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
