using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Repository;

namespace Web.Controllers.BankController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankController : MyController
    {
        private OnlineTestContext DbContext;

        public BankController(OnlineTestContext context)
        {
            this.DbContext = context;
        }

        [HttpGet]
        public List<BankDTO> List()
        {
            IBankRepository repository = new BankRepository(DbContext);
            List<BankDTO> result = new List<BankDTO>();
            List<QuestionBank> banks = repository.Read(userId);
            foreach (QuestionBank bank in banks)
            {
                result.Add(new BankDTO(bank));
            }
            return result;
        }

        [HttpPost]
        public bool Create(BankDTO bank)
        {
            bank.Id = userId;
            return new BankRepository(DbContext).Create(bank);
        }

        [HttpPost]
        public bool Update(BankDTO bank)
        {
            return new BankRepository(DbContext).Update(bank);
        }

        [HttpPost]
        public bool Delete(Guid bankId)
        {
            return new BankRepository(DbContext).Delete(bankId);
        }
    }
}
