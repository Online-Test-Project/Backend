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
    public class AccountRepository
    {
        public AccountRepository()
        {
            
        }
    }
}
