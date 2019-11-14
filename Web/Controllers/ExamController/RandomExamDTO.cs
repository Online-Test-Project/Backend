using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamController
{
    public class RandomExamDTO
    {
        public Guid BankId;
        public List<int> Difficulty;
        public string Time;
        public string Name;
        public string Description;
    }
}