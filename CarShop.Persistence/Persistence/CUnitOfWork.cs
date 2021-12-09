using CarShop.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Infrastructure;
[assembly: InternalsVisibleToAttribute("CarShop.Application.Test")]
[assembly: InternalsVisibleToAttribute("CarShop.Persistence.Ut")]
namespace CarShop.Persistence {

   internal class CUnitOfWork: IUnitOfWork, IDisposable {

      #region fields
      private readonly IServiceScopeFactory _serviceScopeFactory;
      private readonly ILoggerFactory       _loggerFactory;
      private readonly ILogger<CUnitOfWork> _log;

      private          CDbContext?          _dbContext;
      private          IRepositoryUser?     _repositoryUser;
      private          IRepositoryCar?      _repositoryCar;
      private          bool                 _areReposInjected = false; 
      #endregion

      #region properties    (simulation property injection) 
      public IRepositoryUser RepositoryUser {
         get {
            PropertyInjection();
            return _repositoryUser!;
         }
      }
       public IRepositoryCar RepositoryCar {
         get {
            PropertyInjection();
            return _repositoryCar!;
         }
      }
      #endregion

      #region ctor
      public CUnitOfWork(
         IServiceScopeFactory serviceScopeFactory,
         ILoggerFactory       loggerFactory
      ) {
         _serviceScopeFactory = serviceScopeFactory;
         _loggerFactory       = loggerFactory;
         _log                 = loggerFactory.CreateLogger<CUnitOfWork>();   
         _log.LogInformation($"Ctor() {GetHashCode()}");        
      }
      #endregion

      #region methods
      // simulating property injection
      private void PropertyInjection() {
         if(_areReposInjected) return;

         _log.LogDebug($"PropertyInjection() {this.GetHashCode()}"); 
         _dbContext   = _serviceScopeFactory.CreateScope()
                                            .ServiceProvider
                                            .GetService<CDbContext>()
                        ?? throw new NullReferenceException("CDbContext is null");

         _repositoryUser = new CRepositoryUser(_dbContext, _loggerFactory);
         _repositoryCar  = new CRepositoryCar(_dbContext, _loggerFactory);
         var value = _dbContext?.Database;

         _areReposInjected = true; 
      }

      public void Dispose() {
         _log.LogDebug($"Dispose() {this.GetHashCode()}"); 
         _dbContext?.Dispose(); 
         _dbContext = null;
         _repositoryUser   = null;
         _repositoryCar    = null;
         _areReposInjected = false;
      }

      public bool SaveChanges() {
         _log.LogDebug($"SaveChanges() {this.GetHashCode()}"); 
         
         //if(_dbContext?.ChangeTracker.Entries<AEntity>() != null) {
         //   foreach (var entry in _dbContext?.ChangeTracker.Entries<AEntity>()!) {
         //      switch (entry.State) {
         //         case EntityState.Added:
         //            entry.Entity.CreatedDate = DateTime.Now;
         //            break;
         //         case EntityState.Modified:
         //            entry.Entity.LastModifiedDate = DateTime.Now;
         //            break;
         //      }
         //   }
         //}

         Debug.WriteLine(_dbContext?.ChangeTracker.DebugView.LongView);
         
         var records = _dbContext?.SaveChanges();
  
         Debug.WriteLine(_dbContext?.ChangeTracker.DebugView.LongView);
         if(records > 0) return true;
         return false;
      }
      #endregion
   }
}