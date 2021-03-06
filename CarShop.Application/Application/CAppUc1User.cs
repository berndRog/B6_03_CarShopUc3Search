using CarShop.Domain;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CarShop.Application {
   public class CAppUc1User: IAppUc1User {

      #region fields
      private readonly IUnitOfWork         _unitOfWork;
      private readonly ILogger<CAppUc1User> _log;
      #endregion

      #region ctor
      public CAppUc1User(
         IUnitOfWork           unitOfWork,
         ILoggerFactory        loggerFactory
      ) {
         _unitOfWork = unitOfWork;
         _log        = loggerFactory.CreateLogger<CAppUc1User>();
         _log.LogInformation($"Ctor() {GetHashCode()}");
      }
      #endregion 
      
      #region methods - User Create, Register
      public Result<User> CreateUser(string name, string email, string userName, string password, Role role) {
         _log.LogDebug($"{nameof(CreateUser)} {userName}");

         // Check whether UserName already exists in repository
         var result = _unitOfWork.RepositoryUser
                                 .Find(user => user.UserName == userName);
         _unitOfWork.Dispose();
         if(result != null) 
            return new Error<User>($"UserName {userName} gibt es bereits.");
         var (salt, hashed) = AppSecurity.HashPassword(password);
         var user = new User().Init(0, name, email, userName, hashed, salt, Role.User);
         return new Success<User>(user);
      }

      public Result<User> RegisterUser(User user) {
         
         // Insert user into repository
         user.Id   = 0;  // Id is generated by db
         _unitOfWork.RepositoryUser
                    .Insert(user);
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         
         if(result) return new Success<User>(user); 
         else       return new Error<User>($"Error Insert {user.Name}"); 
      }
      #endregion

      #region methods - User Update, Remove
      public Result<User> UpdateUser(User user) {
         _log.LogDebug($"{nameof(UpdateUser)}");

         // Update user
         _unitOfWork.RepositoryUser
                    .Update(user);
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();

         if(result) return new Success<User>(user); 
         else       return new Error<User>($"Fehler in UpdateUser()");
      }

      public Result<User> RemoveUser(User user) {
         _log.LogDebug($"{nameof(RemoveUser)}");
        
         // Delete user 
         _unitOfWork.RepositoryUser
                    .Delete(user);
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         if(result) return new Success<User>(user);
         else       return new Error<User>($"Fehler in RemoveUser()");
      }
      #endregion

      #region AddorUpdateAddress
      public Result<User> AddOrUpdateAddress(string streetNr, string zipCity, User user) {
        
         _log.LogInformation($"{nameof(AddOrUpdateAddress)} {user.UserName}");

         // Add or Update Address
         if(user.Address == null) user.AddAddress(streetNr, zipCity);
         else                     user.UpdateAddress(streetNr, zipCity);  

         // update user in repository --> add new Address
         _unitOfWork.RepositoryUser
                    .Update(user); 
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         
         if(result) return new Success<User>(user); 
         else       return new Error<User>($"Error Update {user.Name}"); 
      }      
      #endregion
   }
}