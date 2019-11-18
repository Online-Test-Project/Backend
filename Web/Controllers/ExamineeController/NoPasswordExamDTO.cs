using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamineeController
{
    public class NoPasswordExamDTO
    {
        public string Name;
        public string Time; // this is time remaining
        public bool IsRandom;
        public string StartTime;
        public string EndTime;
        public string Status;
    }
}
