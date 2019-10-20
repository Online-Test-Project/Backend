using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class UserDetail
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public virtual User User { get; set; }
    }
}
