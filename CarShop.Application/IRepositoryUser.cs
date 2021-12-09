using CarShop.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CarShop {
   public interface IRepositoryUser {

      User? FindById(int id);
      User? Find(Func<User, bool> predicate);
      IEnumerable<User> Select(Func<User, bool> predicate);
      IEnumerable<User> SelectAll(); 

      void Insert(User user);
      void Update(User user);
      void Delete(User user);

      void Attach(User user);

   }
}