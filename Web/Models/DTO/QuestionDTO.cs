using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.DTO
{
    public class QuestionDTO
    {
        public Guid Id;
        public int Difficulty;
        public int Type;
        public string Content;
        public List<AnswerDTO> answers;
    }
}
