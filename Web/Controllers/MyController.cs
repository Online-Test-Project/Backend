using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        protected OnlineTestContext DbContext;
        public MyController(OnlineTestContext _context)
        {
            DbContext = _context;
        }
        public Guid userId;
    }
}