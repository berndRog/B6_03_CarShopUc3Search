using CarShop.Domain.Entities;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace CarShop.Persistence {

   internal class CRepositoryUser: IRepositoryUser {

      #region fields
      private readonly CDbContext _dbContext;
      private readonly ILogger<CRepositoryUser> _log;
      #endregion

      #region ctor
      public CRepositoryUser(
         CDbContext dbContext, 
         ILoggerFactory loggerFactory
      ) {   
         _dbContext = dbContext;
         _log = loggerFactory.CreateLogger<CRepositoryUser>();
         _log.LogInformation($"Ctor() {GetHashCode()}");
      }
      #endregion

      #region methods IRepositoryUser
      public User? FindById(int id) 
         => _dbContext.Users  
                      .Include(u => u.Address)
                      .Include(u => u.OfferedCars)  
                      .FirstOrDefault( u => u.Id == id);
               
      public User? Find(Func<User,bool> predicate) 
         => _dbContext.Users
                      .Include(u => u.Address)
                      .Include(u => u.OfferedCars)  
                      .SingleOrDefault(predicate);

      public IEnumerable<User> Select(Func<User, bool> predicate)
         => _dbContext.Users
                     .Include(u => u.Address)
                     .Include(u => u.OfferedCars)  
                     .Where(predicate).ToList();

      public IEnumerable<User> SelectAll() 
         => _dbContext.Users
                      .Include(u => u.Address)
                      .Include(u => u.OfferedCars)  
                      .ToList();

      public void Insert(User user) 
         => _dbContext.Users.Add(user);
      
      public void Update(User user) 
         => _dbContext.Users.Update(user);     

      public void Delete(User user) {
         // Set UserId in cars to 
         foreach(var c in user.OfferedCars) {
            c.User   = null;
         // c.UserId = 0;
            _dbContext.Cars.Remove(c);  
         }
         if(user.Address == null )  
            _dbContext.Users.Remove(user);
         else {
            _dbContext.Addresses.Remove(user.Address);
            _dbContext.Users.Remove(user);
         }
      }

      public void Attach(User user) {
         _dbContext.Users.Attach(user);
          Debug.WriteLine(_dbContext?.ChangeTracker.DebugView.LongView);
      }
      #endregion
   }
}