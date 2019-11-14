using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Web.Models;
using Web.Repository;
using Web.Services.BankService;

namespace Web.Controllers.BankController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // Every Controller inherit MyController to get user.
    public class BankController : MyController
    {
        private IBankRepository repository;
        private IBankService bankService;
        public BankController(IBankRepository repository, IBankService bankService)
        {
            this.repository = repository;
            this.bankService = bankService;
        }

        [HttpGet]
        public List<BankDTO> List()
        {
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
        public bool Create([FromBody] BankDTO bank)
        {
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
        public bool Update([FromBody] BankDTO bank)
        {
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
        public bool Delete([FromBody] Guid bankId)
        {
            return bankService.Delete(bankId);
        }
    }
}
