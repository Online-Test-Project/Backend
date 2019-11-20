using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Repository;
using Web.Services.ExamineeService;
using Web.Services.ExamService;
using Web.Services.StatisticService;

namespace Web.Controllers.StatisticController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : MyController
    {

        private IStatisticService statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;

        }

        public StatisticController()
        {

        }

        [HttpPost, Route("/statistics")]
        public StatisticsDTO Statistics([FromBody] Guid examId)
        {
            return statisticService.GetStatistics(examId);
        }

        [HttpPost, Route("/line-chart")]
        public List<int> LineChart([FromBody] LineChartDTO lineChartDTO)
        {
            return statisticService.GetLineChart(lineChartDTO);
        }

        [HttpPost, Route("/pie-chart")]
        public List<double> PieChart([FromBody] Guid examId)
        {
            return statisticService.GetPieChart(examId);
        }

        [HttpPost, Route("/progressbar")]
        public List<double> ProgressBar([FromBody] Guid examId)
        {
            List<double> result = new List<double>();
            result.Add(0.34);
            result.Add(0.43);
            result.Add(0.4);
            return result;
        }

        [HttpPost, Route("/participant")]
        public List<ParticipantDTO> Participant([FromBody] SortParticipantDTO sortParticipantDTO)
        {
            return statisticService.GetParticipants(sortParticipantDTO);
        }

    }
}