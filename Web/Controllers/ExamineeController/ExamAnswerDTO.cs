using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamineeController
{
    public class ExamAnswerDTO
    {
        public Guid ExamId;
        public List<ExamAnswerDetailDTO> AnswerDetails;
    }
}
