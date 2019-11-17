using System;
using System.Collections.Generic;

namespace Web.Controllers.ExamineeController
{
    public class ExamAnswerDetailDTO
    {
        public Guid QuestionId;
        public List<UserAnswerDTO> UserAnswers;
        public string Content;
    }
}