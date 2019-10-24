using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers.QuestionController
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public int Difficulty { get; set; }
        public int Type { get; set; }
        public String Content { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}
