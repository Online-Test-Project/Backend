using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.AppStart;
using Web.Common;
using Web.Models;

namespace Web.Services
{
    public interface IUserService : ITransientService
    {
        string Login(UserDTO user);
    }
    public class UserService : MyService, IUserService
    {
        private IJWTHandler JWTHandler;
        public UserService(IJWTHandler JWTHandler) : base()
        {
            this.JWTHandler = JWTHandler;
        }
        public string Login(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.Username))
                throw new BadRequestException("Bạn chưa điền Username");
            if (string.IsNullOrEmpty(user.Password))
                throw new BadRequestException("Bạn chưa điền Password");

            User User = DbContext.Users
               .Where(u => u.Username.ToLower().Equals(user.Username.ToLower())).FirstOrDefault();

            if (User == null)
                throw new BadRequestException("User không tồn tại.");
            if (!User.Password.Equals(user.Password))
                throw new BadRequestException("Bạn nhập sai Password.");
            user.Id = User.Id;
            return JWTHandler.CreateToken(user);
        }

    }
}
