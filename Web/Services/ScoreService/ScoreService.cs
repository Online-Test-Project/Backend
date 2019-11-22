using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ExamineeController;
using Web.Controllers.ReviewController;
using Web.Models;
using Web.Repository;
using Web.Services.BankService;
using Web.Services.ExamineeService;
using Web.Services.ExamService;

namespace Web.Services.ScoreService
{
    public interface IScoreService : ITransientService
    {
        Score Get(Guid examId, Guid userId);
        ReviewExamDTO Update(ExamAnswerDTO examAnswer, Guid userId);
        string GetAnswerContent(Guid examId, Guid userId);
        void Delete(Guid userId, Guid examId);
    }
    public class ScoreService : IScoreService
    {
        private IBankService bankService;
        private IExamineeService examineeService;
        private IScoreRepository scoreRepository;
        private IExamService examService;
        private IAnswerRepository answerRepository;
        private IQuestionRepository questionRepository;
        private IExamRepository examRepository;
        public ScoreService(IBankService bankService, IExamineeService examineeService, IScoreRepository scoreRepository, IExamService examService, IAnswerRepository answerRepository, IQuestionRepository questionRepository, IExamRepository examRepository)
        {
            this.bankService = bankService;
            this.examineeService = examineeService;
            this.scoreRepository = scoreRepository;
            this.examService = examService;
            this.answerRepository = answerRepository;
            this.questionRepository = questionRepository;
            this.examRepository = examRepository;
        }

        public void Delete(Guid userId, Guid examId)
        {
            var ownerId = examService.IsRandom(examId) ? examRepository.GetRandomExam(examId).OwnerId : examRepository.Get(examId).OwnerId;
            if (ownerId == userId)
            {
                scoreRepository.Delete(userId, examId);
            }
        }

        public Score Get(Guid examId, Guid userId)
        {
            return scoreRepository.Get(examId, userId, examService.IsRandom(examId));
        }

        public string GetAnswerContent(Guid examId, Guid userId)
        {
            return scoreRepository.Get(examId, userId, examService.IsRandom(examId)).AnswerContent;
        }

        public ReviewExamDTO Update(ExamAnswerDTO examAnswer, Guid userId)
        {
            ReviewExamDTO mark = examineeService.CalculateMark(examAnswer, userId);
            mark.Date = DateTime.Now.ToString("dd/MM/yyyy");
            mark.ExamId = examAnswer.ExamId;

            Score score = Get(examAnswer.ExamId, userId);
            score.Score1 = Math.Round(mark.Score, 2, MidpointRounding.AwayFromZero);
            score.Time = mark.TimeSpent;
            score.AnswerContent = AnswerContentJSONToString(examAnswer, score.Score1, score.Time);
            scoreRepository.Update(score);
            ExamineeService.ExamineeService.QuestionState.Clear();
            return mark;
        }

        private string AnswerContentJSONToString(ExamAnswerDTO examAnswer, double score,  string time)
        {
            ReviewExamDetailDTO reviewExamDetail = new ReviewExamDetailDTO();
            reviewExamDetail.Name = examService.IsRandom(examAnswer.ExamId) ? examRepository.GetRandomExam(examAnswer.ExamId).Name : examRepository.Get(examAnswer.ExamId).Name;
            reviewExamDetail.Score = score;
            reviewExamDetail.Time = time;
            for (int questionIndex = 0; questionIndex < examAnswer.AnswerDetails.Count; questionIndex++)
            {
                /**
                 * Create new Question to add to review questions
                 * Create List answer to add to answers of questions above
                 * Get Userquestion with full information
                 * in question loop though set of answers to get information and add to list answer above
                 * assign list answer to question
                 * fill the rest of information for question
                 * add question to list
                 */
                ReviewQuestionDTO reviewQuestion = new ReviewQuestionDTO();
                List<ReviewAnswerDTO> reviewAnswers = new List<ReviewAnswerDTO>();

                var userQuestion = examAnswer.AnswerDetails[questionIndex];
                var questionInDb = questionRepository.GetById(userQuestion.QuestionId);

                for (int answerIndex = 0; answerIndex < userQuestion.UserAnswers.Count; answerIndex++)
                {
                    reviewAnswers.Add(new ReviewAnswerDTO
                    {
                        Content = (questionInDb.Type == 3)? userQuestion.Content : answerRepository.Get(userQuestion.UserAnswers[answerIndex].AnswerId).Content,
                        IsSelected = userQuestion.UserAnswers[answerIndex].IsSelected
                    });
                }
                //reviewAnswers.Add(new ReviewAnswerDTO
                //{
                //    Content = userQuestion.Content                    
                //});


                reviewQuestion.Content = userQuestion.Content;
                reviewQuestion.IsCorrect = examineeService.GetQuestionState(userQuestion.QuestionId);
                reviewQuestion.ReviewAnswers = reviewAnswers;
                reviewQuestion.Type = questionInDb.Type;
                reviewQuestion.Difficulty = questionInDb.Difficulty;

                reviewExamDetail.ReviewQuestions.Add(reviewQuestion);
            }

            return JsonConvert.SerializeObject(reviewExamDetail);
        }

        
    }
}
