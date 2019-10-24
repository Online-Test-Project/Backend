using System;
using System.Collections.Generic;
using System.Linq;
using Web.Common;
using Web.Models;


namespace Web.Repository
{
    interface IAnswerRepository
    {
        bool Create(List<Answer> answers);

        List<Answer> List(Guid questionId);

        bool Delete(Guid questionId);

        bool Update(List<Answer> answers);
    }

    public class AnswerRepository : IAnswerRepository
    {
        private OnlineTestContext DbContext;

        public AnswerRepository(OnlineTestContext _DbContext)
        {
            DbContext = _DbContext;
        }

        public List<Answer> List(Guid questionId)
        {
            var query =
                from a
                in DbContext.Answers
                where a.Id == questionId
                select a;
            return query.ToList();
        }

        public bool Create(List<Answer> answers)
        {
            try
            {
                foreach (Answer answer in answers)
                {
                    DbContext.Answers.Add(answer);
                }
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
                    in DbContext.Answers
                    where q.Id == questionId
                    select q;
                foreach (Answer answer in query)
                {
                    DbContext.Answers.Remove(answer);
                }
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Update(List<Answer> answers)
        {
            Guid questionId = answers.First().QuestionId;
            try
            {
                var query =
                    from q
                    in DbContext.Answers
                    where q.Id == questionId
                    select q;
                foreach (Answer answer in query)
                {
                    DbContext.Answers.Remove(answer);
                }

                foreach (Answer answer in answers)
                {
                    DbContext.Answers.Add(answer);
                }

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
