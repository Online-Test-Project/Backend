using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers.BankController
{
    public class BankDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime ModifiedDate { get; set; }

        public BankDTO(QuestionBank bank)
        {
            Id = bank.Id;
            Name = bank.Name;
            Description = bank.Description;
            ModifiedDate = bank.ModifiedDate;
        }
    }
}
