﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.ReviewController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        [HttpGet]
        public List<ReviewExamDTO> List()
        {
            List<ReviewExamDTO> reviewExams = new List<ReviewExamDTO>();
            reviewExams.Add(new ReviewExamDTO
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Date = DateTime.Now.ToString(),
                Score = 8.5,
                Time = "15:27"
            });

            reviewExams.Add(new ReviewExamDTO
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab"),
                Date = DateTime.Now.ToString(),
                Score = 8.5,
                Time = "15:27"
            });

            return reviewExams;
        }
    }
}