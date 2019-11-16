using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Web.AppStart;
using Web.Common;
using Web.Models;
using Web.Repository;

namespace Web.Services
{
    public interface IUserService : ITransientService
    {
        string Login(UserDTO user);

        string Register(UserDTO userDTO);
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

        public string Register(UserDTO user) 
        {
            if (string.IsNullOrEmpty(user.Username))
                throw new BadRequestException("Bạn chưa điền Username");
            if (string.IsNullOrEmpty(user.Password))
                throw new BadRequestException("Bạn chưa điền Password");
            IUserRepository repository = new UserRepository(DbContext);
            if (repository.CountByUsername(user.Username) != 0)
            {
                throw new BadRequestException("Username đã tồn tại");
            }
            if (repository.Create(user))
            {
                return JWTHandler.CreateToken(user);
            } 
            else
            {
                throw new BadRequestException("Lỗi tạo tài khoản");
            }
        }

    }
}
