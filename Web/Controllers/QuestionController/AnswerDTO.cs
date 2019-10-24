using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers.QuestionController
{
    public class AnswerDTO
    {
        public String Content { get; set; }
        public Boolean IsCorrect { get; set; }

    }
}
