using CarShop.Domain.Entities;
using CarShop.Domain.Enums;

namespace CarShop {
   public interface IAppUc1User {
      Result<User> CreateUser  (string name, string email, 
                                string userName,string password, 
                                Role role);
      Result<User> RegisterUser(User   user);

      Result<User> UpdateUser  (User user); 
      Result<User> RemoveUser  (User user);

      Result<User> AddOrUpdateAddress(string streetNr, string zipCity, 
                                      User user);
   }
}
