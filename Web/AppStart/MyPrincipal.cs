using System.Security.Claims;

namespace Web.AppStart
{
    public class MyPrincipal : ClaimsPrincipal
    {
        public MyPrincipal(UserDTO user)
        {
            this.UserEntity = user;
        }

        public UserDTO UserEntity { get; set; }
      
    }
}
