using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamController
{
    public class RandomExamDetailDTO
    {
        public Guid BankId;
        public string BankName;
        public string Name;
        public string Time;
        public int NumberOfEasyQuestion;
        public int NumberOfNormalQuestion;
        public int NumberOfHardQuestion;
    }
}
