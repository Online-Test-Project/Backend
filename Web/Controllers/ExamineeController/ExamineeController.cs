//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Web.Repository;
//using Web.Services.ExamineeService;
////using Web.Services.ExamineeService;
//using Web.Services.ExamService;

//namespace Web.Controllers.ExamineeController
//{
//    [Route("api/[controller]/[action]")]
//    [ApiController]
//    public class ExamineeController : MyController
//    {
//        private IExamRepository examRepository;
//        private IExamService examService;
//        private IExamineeService examineeService;
//        //private IExamineeService examineeService;

//        public ExamineeController(IExamRepository examRepository, IExamineeService examineeService)
//        {
//            this.examRepository = examRepository;
//            this.examineeService = examineeService;
//        }

//        [HttpGet, Route("{Id}")]
//        public NoPasswordExamDTO Get(Guid Id)
//        {
//            return examineeService.Get(Id);
//        }

//        public IActionResult Do([FromBody] AccessExamDTO accessExam)
//        {
//            if (!examineeService.VerifyPassword(accessExam))
//            {
//                return Ok("Bad password");
//            }

//            return Ok(examineeService.Access(accessExam));
//        }
//    }
//}