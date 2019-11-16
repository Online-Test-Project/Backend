using System;
using System.Collections.Generic;

namespace Web.Controllers.ExamineeController
{
    public class ExamAnswerDetailDTO
    {
        Guid QuestionId;
        List<UserAnswerDTO> UserAnswers;
        string Content;
    }
}