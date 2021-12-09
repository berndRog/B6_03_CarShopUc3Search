using CarShop.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace CarShop.Application {
   public class CAppCore: IAppCore {

      #region fields
      private readonly IUnitOfWork         _unitOfWork;
      private readonly ILogger<CAppCore> _log;
      #endregion

      #region properties
      public User? LoggedInUser { get; set; } = null; // no user logged in
      #endregion

      #region ctor
      public CAppCore(
         IUnitOfWork           unitOfWork,
         ILoggerFactory        loggerFactory
      ) {
         _unitOfWork = unitOfWork;
         _log        = loggerFactory.CreateLogger<CAppCore>();
         _log.LogInformation($"Ctor() {GetHashCode()}");
      }
      #endregion 
      
      #region methods - User Login, Logout
      public Result<User> LoginUser(string userName, string password) {
         _log.LogDebug($"{nameof(LoginUser)} {userName}");
         
         // Check is username ok? 
         var result = SCheckInput.IsUserNameOk(userName);
         if(result is Success<string>) userName = result.Data!;
         else if(result is Error<string>) return new Error<User>(result.Message);
         
         // Is user registerd?
         var user = _unitOfWork.RepositoryUser
                               .Find(user => user.UserName == userName);
         _unitOfWork.Dispose();
         if(user == null) {
            return new Error<User>($"Nutzer {userName} gibt es nicht");
         }
         // Compare passwords
         else if(user != null) { 
            var salt   = user.Salt;
            var hashed = user.Password;
            if(AppSecurity.Compare(password, hashed, salt)) { 
               LoggedInUser = user; // notify observers
               return new Success<User>(LoggedInUser); 
            }
         } 
         return new Error<User>($"Login fehlgeschlagen");
      }

      public Result<User> LogoutUser() {
         _log.LogDebug($"{nameof(LogoutUser)}");
         LoggedInUser = new NullUser();
         return new Success<User>(LoggedInUser);
      }
      #endregion
  
   }
}