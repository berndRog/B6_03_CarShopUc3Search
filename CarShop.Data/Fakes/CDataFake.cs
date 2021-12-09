using System.Collections.Generic;
using CarShop.Domain.Entities;

namespace CarShop.Fakes {

   internal class CDataFake : IData {

      private readonly CDbInMemory _inMemory = new ();

      public CDataFake() {}

      public int CountCars()                                        
         => _inMemory.CountCars();
      public IList<string> SelectMakes()                      
         => _inMemory.SelectMakes();
      public IList<string> SelectModels(string make)          
         => _inMemory.SelectModels(make);     
      public IList<Car>    SelectCars(string make, string model) 
         => _inMemory.SelectCars(make, model);
      public Car?                SelectCar(int id)                  
         => _inMemory.SelectCar(id);
   }
}
