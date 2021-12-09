using CarShop.Domain.Entities;
using System;
namespace CarShop {

   public interface IUnitOfWork: IDisposable {
      IRepositoryUser RepositoryUser { get; }      
      IRepositoryCar  RepositoryCar  { get; }      
      bool SaveChanges();
   }
}
