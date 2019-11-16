using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.AppStart;
using Web.Models;

namespace Web.Repository
{
    public interface IUserRepository
    {
        int CountByUsername(string username);

        bool Create(UserDTO userDTO);
    }
    public class UserRepository : IUserRepository
    {
        private OnlineTestContext DbContext;

        public UserRepository(OnlineTestContext dbContext)
        {
            DbContext = dbContext;
        }

        public int CountByUsername(string username)
        {
            return DbContext.Users.Where(x => x.Username == username).Count();
        }

        public bool Create(UserDTO userDTO)
        {
            try
            {
                 User user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = userDTO.Username,
                    Password = userDTO.Password,
                    DateCreated = DateTime.Now
                };
                DbContext.Users.Add(user);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}