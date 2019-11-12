using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Repository;
using Web.Services.ExamService;

namespace Web.Services.BankService
{
    public interface IBankService : ITransientService
    {
        bool Delete(Guid bankId);
    }
    public class BankService : IBankService 
    {
        private IQuestionRepository questionRepository;
        private IExamRepository examRepository;
        private IExamService examService;

        public BankService(IQuestionRepository questionRepository, IExamRepository examRepository, IExamService examService)
        {
            this.questionRepository = questionRepository;
            this.examRepository = examRepository;
            this.examService = examService;
        }

        public bool Delete(Guid bankId)
        {
            var listQuestionId = questionRepository.ListByBankId(bankId).Select(x => x.Id).ToList();
            bool deleteQuest = questionRepository.Delete(listQuestionId);
            bool flag = true;
            var listExamId = examRepository.ListExamIdByBankId(bankId);
            listExamId.ForEach(x =>
            {
                if (!examService.Delete(x))
                {
                    flag = false;
                }
            });

            return flag && deleteQuest;

        }
    }
}
