using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.StatisticController
{
    public class SortParticipantDTO
    {
        public Guid ExamId { get; set; }
        public int SortField { get; set; }
        public bool ASC { get; set; }

    }
}
