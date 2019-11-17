using System;
using System.Collections.Generic;
using System.Linq;
using Web.Common;
using Web.Models;


namespace Web.Repository
{
    public interface IAnswerRepository : ITransientService
    {
        bool Create(List<Answer> newAnswers);

        List<Answer> ListByQuestionId(Guid questionId);

        bool DeleteByQuestionId(Guid questionId);

        bool Update(List<Answer> updatedAnswers);

        Answer Get(Guid answerId);
    }

    public class AnswerRepository : IAnswerRepository
    {
        private OnlineTestContext DbContext;

        public AnswerRepository(OnlineTestContext _DbContext)
        {
            DbContext = _DbContext;
        }

        public bool Create(List<Answer> newAnswers)
        {
            try
            {
                foreach (Answer answer in newAnswers)
                {
                    DbContext.Answers.Add(answer);
                }
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Answer> ListByQuestionId(Guid questionId)
        {
            var query =
                from answer
                in DbContext.Answers
                where answer.QuestionId == questionId
                select answer;
            return query.ToList();
        }

        public bool DeleteByQuestionId(Guid questionId)
        {
            try
            {
                //var query =
                //    from answer
                //    in DbContext.Answers
                //    where answer.QuestionId == questionId
                //    select answer;
                var query = DbContext.Answers.Where(x => x.QuestionId == questionId);
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

        public bool Update(List<Answer> updatedAnswers)
        {
            Guid questionId = updatedAnswers.First().QuestionId;
            try
            {
                var query =
                    from answer
                    in DbContext.Answers
                    where answer.QuestionId == questionId
                    select answer;
                foreach (Answer answer in query)
                {
                    DbContext.Answers.Remove(answer);
                }

                foreach (Answer answer in updatedAnswers)
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

        public Answer Get(Guid answerId)
        {
            return DbContext.Answers.Where(x => x.Id == answerId).FirstOrDefault();
        }
    }
}
