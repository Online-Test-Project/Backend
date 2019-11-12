using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Repository;

namespace Web.Services.ExamService
{
    public interface IExamService :ITransientService
    {
        bool IsRandom(Guid examId);
        bool Delete(Guid examId);
    }
    public class ExamService : IExamService
    {
        private IExamRepository examRepository;

        public ExamService(IExamRepository examRepository)
        {
            this.examRepository = examRepository;
        }

        public bool Delete(Guid examId)
        {
            return examRepository.Delete(examId, IsRandom(examId));
        }

        public bool IsRandom(Guid examId)
        {
            if (examRepository.GetRandomExam(examId) != null)
            {
                return true;
            }

            return false;
        }


    }
}
