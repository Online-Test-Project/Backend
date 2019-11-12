using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.AppStart
{
    public class AfterLoginDTO
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Jwt { get; set; }

    }
}
