using CarShop.Domain.Entities;
using CarShop.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace CarShop {
   public interface IRepositoryCar {

      Car? FindById(int id);
      Car? Find(Func<Car, bool> predicate);
      IEnumerable<Car> Select(Func<Car, bool> predicate);
      IEnumerable<Car> SelectAll(); 

      void Attach(Car car);

      int Count();
      int CountByMake        (string make);
      int CountByMakeAndModel(string make, string model);
      int CountByCarTs       (CarTs carTs);

      IEnumerable<string> SelectMakes ();
      IEnumerable<string> SelectModels(string make);
      IEnumerable<Car>    SelectCars  (CarTs carTs); 

   }
}