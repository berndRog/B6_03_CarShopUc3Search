using CarShop.Domain.Entities;

namespace CarShop {
   public interface IAppCore {

      public User? LoggedInUser {get; set; }
      
      Result<User> LoginUser   (string userName, string password);
      Result<User> LogoutUser  ();
   }
}
