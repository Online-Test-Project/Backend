﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ExamController
{
    public class ExamDetailDTO
    {
        public Guid Id;
        public string Time;
        public string Name;
        public string Password;
        public string Description;
        public bool IsRandom;
        public string StartTime;
        public string EndTime;
        public int Count;
    }
}
