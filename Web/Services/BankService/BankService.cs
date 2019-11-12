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
        private IBankRepository bankRepository;

        public BankService(IQuestionRepository questionRepository, IExamRepository examRepository, IExamService examService, IBankRepository bankRepository)
        {
            this.questionRepository = questionRepository;
            this.examRepository = examRepository;
            this.examService = examService;
            this.bankRepository = bankRepository;
        }

        public bool Delete(Guid bankId)
        {
            var listQuestion = questionRepository.ListByBankId(bankId);
            var listQuestionId = listQuestion.Select(x => x.Id).ToList();
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

            flag = bankRepository.Delete(bankId) && flag;

            return flag && deleteQuest;

        }
    }
}
