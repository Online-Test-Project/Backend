using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AppStart;
using Web.Models;
using Web.Repository;

namespace Web.Controllers.BankController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // Every Controller inherit MyController to get user.
    public class BankController : MyController
    {

        public BankController(OnlineTestContext _context) : base(_context)
        {
        }

        [HttpGet]
        public List<BankDTO> Count()
        {
            IBankRepository repository = new BankRepository(DbContext);
            List<QuestionBank> list = repository.ListByOwnerId(user.Id);
            List<BankDTO> result = new List<BankDTO>();
            foreach (QuestionBank bank in list) {
                result.Add(new BankDTO
                {
                    Id = bank.Id,
                    Name = bank.Name,
                    Description = bank.Description,
                    ModifiedDate = bank.ModifiedDate
                });
            }
            return result;
        }

        [HttpPost]
        public bool Create(BankDTO bank)
        {
            IBankRepository repository = new BankRepository(DbContext);
            QuestionBank newBank = new QuestionBank {
                Id = Guid.NewGuid(),
                OwnerId = user.Id,
                Name = bank.Name,
                Description = bank.Description,
                ModifiedDate = bank.ModifiedDate
            };
            return repository.Create(newBank);
        }

        [HttpPost]
        public bool Update(BankDTO bank)
        {
            IBankRepository repository = new BankRepository(DbContext);
            QuestionBank updatedBank = new QuestionBank
            {
                Id = bank.Id,
                OwnerId = user.Id,
                Name = bank.Name,
                Description = bank.Description,
                ModifiedDate = bank.ModifiedDate
            };
            return repository.Update(updatedBank);
        }

        [HttpPost]
        public bool Delete(Guid bankId)
        {
            IBankRepository repository = new BankRepository(DbContext);
            return repository.Delete(bankId);
        }
    }
}
