using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public class MyService
    {
        protected OnlineTestContext DbContext;

        public MyService()
        {
            DbContext = new OnlineTestContext();
        }
    }
}
