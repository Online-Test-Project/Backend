using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AppStart;
using Web.Models;
using Web.Repository;

namespace Web.Controllers.BankController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // Every Controller inherit MyController to get user.
    public class BankController : MyController
    {

        public BankController(OnlineTestContext _context) : base(_context)
        {
        }

        [HttpGet]
        public int Count()
        {
            IBankRepository repository = new BankRepository(DbContext);
            return repository.Count(user);
        }
    }
}
