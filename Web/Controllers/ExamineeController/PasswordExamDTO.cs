using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamineeController
{
    public class PasswordExamDTO
    {
        public List<ExamineeQuestionDTO> ExamineeQuestions = new List<ExamineeQuestionDTO>();
        public string TimeRemaining;
    }
}
