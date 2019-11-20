using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.StatisticController
{
    public class LineChartDTO
    {
        public Guid ExamId { get; set; }
        public double Step { get; set; }
    }
}
