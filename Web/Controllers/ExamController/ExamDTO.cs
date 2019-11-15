using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamController
{
    public class ExamDTO
    {
        public List<Guid> QuestionId;
        public string Time;
        public string Name;
        public string Description;
    }
}
