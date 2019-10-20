using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Authentication;
using Web.Models;

namespace Web.Repository
{
    public interface IUserRepository
    {
        User Authenticate(string username, string password);
        List<User> GetAllUser();
        Task<bool> AddUser(User user);
    }
    public class UserRepository : IUserRepository
    {
        private OnlineTestContext DbContext;
        private JwtSettings jwtSettings;

        public UserRepository(OnlineTestContext _context, JwtSettings jwtSettings)
        {
            DbContext = _context;
            this.jwtSettings = jwtSettings;
        }

        public async Task<bool> AddUser(User user)
        {
            if (DbContext != null)
            {
                await DbContext.Users.AddAsync(user);
                await DbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public User Authenticate(string username, string password)
        {
            var user = DbContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
            // return null if user not found
            if (user == null)
            {
                return null;
            }

            // authentication successfull so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public List<User> GetAllUser()
        {
            if (DbContext != null)
            {
                return DbContext.Users.ToList();
            }
            return null;
        }
    }
}