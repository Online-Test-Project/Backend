using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Repository;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private OnlineTestContext DbContext;
        public BankController(OnlineTestContext _context)
        {
            DbContext = _context;
        }
        [HttpGet]
        public bool create()
        {
            IBankRepository bank = new BankRepository(DbContext);
            bank.Create(new Models.DTO.BankDTO
            {
                Description = "asdf",
                id = new Guid("00000000-0000-0000-0000-000000000000"),
                ModifiedDate = DateTime.Now,
                Name = "zcvzcx"
            });
            return true;
        }
                
    }
}