using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamineeController
{
    public class AccessExamDTO
    {
        public Guid Id;
        public string Password;
        public bool IsRandom;
    }
}
