using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamineeController
{
    public class ExamineeQuestionDTO
    {
        public Guid Id;  
        public int Type; 
        public string Content;
        public List<ExamineeAnswerDTO> Answers;
    }
}
