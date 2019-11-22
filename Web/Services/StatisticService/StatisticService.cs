using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;
using Web.Controllers.ReviewController;
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
        List<int> GetPieChart(Guid examId);
        CorrectPercentDTO GetCorrectPercent(Guid examId);
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
            if (lineChartDTO.Step == 0)
            {
                return new List<int>();
            }
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

        public List<int> GetPieChart(Guid examId)
        {
            List<Score> scores = scoreRepository.ListByExamId(examId);
            List<int> result = new List<int>();
            result.Add(ScoreRangeCount(scores, 0, true, 4, true));
            result.Add(ScoreRangeCount(scores, 4, false, 6.5, false));
            result.Add(ScoreRangeCount(scores, 6.5, true, 8, false));
            result.Add(ScoreRangeCount(scores, 8, true, 10, true));
            return result;
        }

        public StatisticsDTO GetStatistics(Guid examId) 
        {
            List<Score> scores = scoreRepository.ListByExamId(examId);
            if (scores.Count == 0)
            {
                return new StatisticsDTO
                {
                    Participants = 0,
                    AvgScore = 0,
                    AvgTimeSpent = "0 phút 0 giây"
                };
            }
            StatisticsDTO response = new StatisticsDTO();
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
            response.AvgScore = response.Participants == 0 ? 0 : Math.Round(totalScore / response.Participants, 2, MidpointRounding.AwayFromZero);
            response.AvgTimeSpent = ((int)(avgTimeInSecond / 60)) + " phút " + ((int)(avgTimeInSecond % 60)) + " giây";
            return response;
        }

        private int ScoreRangeCount(List<Score> scores, double start, bool inclusiveStart, double end, bool inclusiveEnd)
        {
            return scores.Where(x => inclusiveStart ? (x.Score1 >= start) : (x.Score1 > start)).Where(x => inclusiveEnd ? (x.Score1 <= end) : (x.Score1 < end)).Count();
        }
        
        public CorrectPercentDTO GetCorrectPercent(Guid examId)
        {
            CorrectPercentDTO result = new CorrectPercentDTO();
            int[] correctType = new int[4];
            int[] totalType = new int[4];
            int[] correctDifficulty = new int[3];
            int[] totalDifficulty = new int[3];
            scoreRepository.ListByExamId(examId).ForEach(score =>
            {
                reviewService.GetAnswerContentObj(examId, score.UserId).ReviewQuestions.ForEach(reviewQuestion =>
                {
                    totalType[reviewQuestion.Type - 1]++;
                    totalDifficulty[reviewQuestion.Difficulty - 1]++;
                    if (reviewQuestion.IsCorrect)
                    {
                        correctType[reviewQuestion.Type - 1]++;
                        correctDifficulty[reviewQuestion.Difficulty - 1]++;
                    }
                });
            });
            for (int i = 0; i < 4; i++)
            {
                if (i != 3)
                {
                    result.Difficulty.Add(totalDifficulty[i] == 0 ? 0 : Math.Round(correctDifficulty[i] * 100.0 / totalDifficulty[i], 2, MidpointRounding.AwayFromZero));
                }
                result.Type.Add(totalType[i] == 0 ? 0 : Math.Round(correctType[i] * 100.0 / totalType[i], 2, MidpointRounding.AwayFromZero));
            }
            return result;
        }
        
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
            if (sortParticipantDTO.ASC)
            {
                switch (sortParticipantDTO.SortField)
                {
                    case 1: result.OrderBy(x => x.Username); break;
                    case 2: result.OrderBy(x => x.Score); break;
                    case 3: result.OrderBy(x => x.Time); break;
                }
            }
            else
            {
                switch (sortParticipantDTO.SortField)
                {
                    case 1: result.OrderByDescending(x => x.Username); break;
                    case 2: result.OrderByDescending(x => x.Score); break;
                    case 3: result.OrderByDescending(x => x.Time); break;
                }
            }
            
            return result;
        }
    }
}
