//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Web.Models;

//namespace Web.Repository
//{
//    interface IAnswerRepository
//    {
//        Task<List<AnswerDTO>> List(Guid questionId);
//        Task<bool> BulkCreate(Answer answer);
//        Task<bool> Delete(Guid questionId);
//    }
//    public class AnswerRepository : IAnswerRepository
//    {
//        private OnlineTestContext DbContext;
//        public AnswerRepository(OnlineTestContext _DbContext)
//        {
//            DbContext = _DbContext;
//        }

//        public Task<bool> BulkCreate(Answer answer)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> Delete(Guid questionId)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<AnswerDTO>> List(Guid questionId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
