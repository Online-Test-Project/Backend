using System;
using System.Collections.Generic;
using Web.Models;
using System.Linq;

namespace Web.Repository
{
    interface IQuestionRepository
    {
        List<Question> List(Guid bankId);

        Question Read(Guid questionId);

        bool Update(Question question);

        bool Create(Question question);

        bool Delete(Guid questionId);
    }

    public class QuestionRepository : IQuestionRepository
    { 
        
        private OnlineTestContext DbContext;

        public QuestionRepository(OnlineTestContext _context)
        {
            DbContext = _context;
        }

        public List<Question> List(Guid bankId)
        {
            var query =
                from q
                in DbContext.Questions
                where q.BankId == bankId
                select q;
            return query.ToList();
        }

        public Question Read(Guid questionId)
        {
            var query =
                from q
                in DbContext.Questions
                where q.Id == questionId
                select q;
            return query.First();
        }

        public bool Update(Question question)
        {
            try
            {
                DbContext.Questions.Update(question);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Create(Question question)
        {
            try
            {
                DbContext.Questions.Add(question);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Delete(Guid questionId)
        {
            try
            {
                var query =
                    from q
                    in DbContext.Questions
                    where q.Id == questionId
                    select q;
                DbContext.Questions.Remove(query.First());
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
