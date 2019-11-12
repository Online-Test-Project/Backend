using System;
using System.Collections.Generic;
using Web.Models;
using Web.Common;
using System.Linq;

namespace Web.Repository
{
    public interface IQuestionRepository : ITransientService
    {
        List<Question> ListByBankId(Guid bankId);

        Question GetById(Guid questionId);

        bool Update(Question updatedQuestion);

        bool Create(Question newQuestion);

        bool Delete(List<Guid> Ids);

        List<Question> ListByExamId(Guid examId);
    }

    public class QuestionRepository : IQuestionRepository
    { 
        
        private OnlineTestContext DbContext;

        public QuestionRepository(OnlineTestContext _context)
        {
            DbContext = _context;
        }

        public List<Question> ListByBankId(Guid bankId)
        {
            var query =
                from question
                in DbContext.Questions
                where question.BankId == bankId
                select question;
            return query.ToList();
        }

        public Question GetById(Guid questionId)
        {
            var query =
                from question
                in DbContext.Questions
                where question.Id == questionId
                select question;
            return query.First();
        }

        public bool Update(Question updatedQuestion)
        {
            try
            {
                DbContext.Questions.Update(updatedQuestion);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Create(Question newQuestion)
        {
            try
            {
                DbContext.Questions.Add(newQuestion);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Delete(List<Guid> Ids)
        {
            try
            {
                Ids.ForEach(x =>
                {
                    var singleEQ = DbContext.ExamQuestions.Where(y => y.QuestionId == x);
                    DbContext.ExamQuestions.RemoveRange(singleEQ);
                    
                    var Q = DbContext.Questions.Where(y => y.Id == x).FirstOrDefault();
                    DbContext.Questions.Remove(Q);
                });
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Question> ListByExamId(Guid examId)
        {
            List<Question> returnList = new List<Question>();
            returnList = DbContext.Questions.Where(x => x.ExamQuestions.Any(y => y.ExamId == examId)).ToList();
            return returnList;
        }
    }
}
