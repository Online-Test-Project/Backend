using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.AppStart;
using Web.Models;
using Web.Repository;
using Web.Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("Login"), HttpPost]
        public string Login([FromBody] UserDTO user)
        {
            string token = _userService.Login(user);
            var CookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30), Path = "/" };
            Response.Cookies.Append("JWT", token, CookieOptions);
            return token;
        }
    }
}