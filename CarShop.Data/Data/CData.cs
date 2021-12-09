using System;
using System.Collections.Generic;
using CarShop.Domain.Entities;

namespace CarShop.Data {
   
   internal class CData : IData {

      public CData() { }

      public int CountCars()                                  => throw new NotImplementedException(); 
      public IList<string> SelectMakes()                      => throw new NotImplementedException(); 
      public IList<string> SelectModels(string make)          => throw new NotImplementedException(); 
      public IList<Car> SelectCars(string make, string model) => throw new NotImplementedException(); 
      public Car? SelectCar(int id)                           => throw new NotImplementedException();
   }
}
