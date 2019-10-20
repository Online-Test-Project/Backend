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

        public BankController(OnlineTestContext _context) : base(_context)
        {
        }

        [HttpGet]
        public List<BankDTO> List()
        {
            IBankRepository repository = new BankRepository(DbContext);
            List<BankDTO> result = new List<BankDTO>();
            List<QuestionBank> banks = repository.Read(userId);
            foreach (QuestionBank bank in banks)
            {
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
        public bool Create(BankDTO bankDTO)
        {
            QuestionBank newBank = new QuestionBank();
            newBank.OwnerId = userId;
            newBank.Id = Guid.NewGuid();
            newBank.Name = bankDTO.Name;
            newBank.Description = bankDTO.Description;
            newBank.ModifiedDate = bankDTO.ModifiedDate;
            return new BankRepository(DbContext).Create(newBank);
        }

        [HttpPost]
        public bool Update(BankDTO bankDTO)
        {
            QuestionBank newBank = new QuestionBank();
            newBank.OwnerId = userId;
            newBank.Id = bankDTO.Id;
            newBank.Name = bankDTO.Name;
            newBank.Description = bankDTO.Description;
            newBank.ModifiedDate = bankDTO.ModifiedDate;
            return new BankRepository(DbContext).Update(newBank);
        }

        [HttpPost]
        public bool Delete(Guid bankId)
        {
            return new BankRepository(DbContext).Delete(bankId);
        }
    }
}
