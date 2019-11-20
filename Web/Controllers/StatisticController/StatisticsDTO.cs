using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.StatisticController
{
    public class StatisticsDTO
    {

        public int Participants { get; set; }
        public double AvgScore { get; set; }
        public string AvgTimeSpent { get; set; }
        
    }
}
