using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Controllers.QuestionController;

namespace Web.Controllers.ExamController
{
    public class ExamWithQuestionsDTO
    {
        public string Name;
        public string Time;
        public List<QuestionDTO> Questions;
    }
}
