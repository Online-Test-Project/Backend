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

        [HttpPost, Route("statistics")]
        public StatisticsDTO Statistics([FromBody] Guid examId)
        {
            return statisticService.GetStatistics(examId);
        }

        [HttpPost, Route("line-chart")]
        public List<int> LineChart([FromBody] LineChartDTO lineChartDTO)
        {
            return statisticService.GetLineChart(lineChartDTO);
        }

        [HttpPost, Route("pie-chart")]
        public List<int> PieChart([FromBody] Guid examId)
        {
            return statisticService.GetPieChart(examId);
        }

        [HttpPost, Route("progressbar")]
        public CorrectPercentDTO ProgressBar([FromBody] Guid examId)
        {
            return statisticService.GetCorrectPercent(examId);
        }

        [HttpPost, Route("participant")]
        public List<ParticipantDTO> Participant([FromBody] SortParticipantDTO sortParticipantDTO)
        {
            return statisticService.GetParticipants(sortParticipantDTO);
        }

    }
}