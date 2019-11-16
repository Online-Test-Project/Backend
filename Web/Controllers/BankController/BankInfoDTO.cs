using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.BankController
{
    public class BankInfoDTO
    {
        public string Name;
        public string Description;
        public string ModifiedDate;
        public List<int> Type;
        public List<int> Difficulty;
    }
}
