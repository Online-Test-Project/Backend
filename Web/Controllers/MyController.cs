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

        public OnlineTestContext DbContext;
        public Guid userId;

        public MyController(OnlineTestContext _DbContext)
        {
            DbContext = _DbContext;
        }
    }
}