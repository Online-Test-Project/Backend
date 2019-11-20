using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.StatisticController;
using Web.Models;
using Web.Repository;
using Web.Services.ExamService;
using Web.Services.ReviewService;
using Web.Services.ScoreService;

namespace Web.Services.StatisticService
{
    public interface IStatisticService : ITransientService
    {
        StatisticsDTO GetStatistics(Guid examId);
        List<int> GetLineChart(LineChartDTO lineChartDTO);
        List<double> GetPieChart(Guid examId);
        List<ParticipantDTO> GetParticipants(SortParticipantDTO sortParticipantDTO);

    }

    public class StatisticService : IStatisticService
    {
        private IScoreService scoreService;
        private IReviewService reviewService;
        private IExamService examService;

        private IScoreRepository scoreRepository;
        private IExamRepository examRepository;
        private IUserRepository userRepository;

        public StatisticService(IScoreService scoreService, IReviewService reviewService, IExamService examService, IScoreRepository scoreRepository, IExamRepository examRepository, IUserRepository userRepository)
        {
            this.scoreService = scoreService;
            this.reviewService = reviewService;
            this.examService = examService;
            this.scoreRepository = scoreRepository;
            this.examRepository = examRepository;
            this.userRepository = userRepository;
        }

        public List<int> GetLineChart(LineChartDTO lineChartDTO)
        {
            List<Score> scores = scoreRepository.ListByExamId(lineChartDTO.ExamId);
            List<int> result = new List<int>();
            double start = 0;
            double end;
            while (start < 10)
            {
                end = start + lineChartDTO.Step;
                if (end == 10)
                {
                    end = 11;
                }
                result.Add(ScoreRangeCount(scores, start, true, end, false));
                start = end;
            }
            return result;
        }

        public List<double> GetPieChart(Guid examId)
        {
            List<Score> scores = scoreRepository.ListByExamId(examId);
            List<double> result = new List<double>();
            double total = scores.Count;
            result.Add(ScoreRangeCount(scores, 0, true, 4, true) / total);
            result.Add(ScoreRangeCount(scores, 4, false, 6.5, false) / total);
            result.Add(ScoreRangeCount(scores, 6.5, true, 8, false) / total);
            result.Add(ScoreRangeCount(scores, 8, true, 10, true) / total);
            return result;
        }

        public StatisticsDTO GetStatistics(Guid examId) 
        {
            StatisticsDTO response = new StatisticsDTO();
            List<Score> scores = scoreRepository.ListByExamId(examId);
            TimeSpan totalTime = TimeSpan.Zero;
            double totalScore = 0;
            TimeSpan examTime = TimeSpan.FromMinutes(int.Parse(examService.IsRandom(examId) ? examRepository.GetRandomExam(examId).Time : examRepository.Get(examId).Time));
            scores.ForEach(x =>
            {
                totalTime += x.Time.Equals("") ? examTime : TimeSpan.Parse(x.Time);
                totalScore += x.Score1;
            });
            double avgTimeInSecond = totalTime.TotalSeconds / scores.Count;

            response.Participants = scores.Count;
            response.AvgScore = Math.Round(totalScore / response.Participants, 2, MidpointRounding.AwayFromZero);
            response.AvgTimeSpent = ((int)(avgTimeInSecond / 60)) + " phút " + ((int)(avgTimeInSecond % 60)) + " giây";
            return response;
        }

        private int ScoreRangeCount(List<Score> scores, double start, bool inclusiveStart, double end, bool inclusiveEnd)
        {
            return scores.Where(x => inclusiveStart ? (x.Score1 >= start) : (x.Score1 > start)).Where(x => inclusiveEnd ? (x.Score1 <= end) : (x.Score1 < end)).Count();
        }
        //
        //public List<double> HardLevelCompletedPercent()
        //{
        //
        //}
        
        public List<ParticipantDTO> GetParticipants(SortParticipantDTO sortParticipantDTO)
        {
            List<ParticipantDTO> result = new List<ParticipantDTO>();
            List<Score> scores = scoreRepository.ListByExamId(sortParticipantDTO.ExamId);
            foreach (Score score in scores)
            {
                result.Add(new ParticipantDTO
                {
                    Username = userRepository.Get(score.UserId).Username,
                    Score = score.Score1,
                    Time = score.Time
                });
            }
            return result;
        }
    }
}
