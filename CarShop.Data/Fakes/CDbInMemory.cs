using System;
using System.Collections.Generic;
using CarShop.Domain.Entities;

namespace CarShop.Fakes {
   

   internal class CDbInMemory {

      private readonly Dictionary<long,Car> _cars = new ();
      
      internal CDbInMemory() {
         var cars = new List<Car> {
            new Car().Init(1,"Volkswagen",    "Golf",   10000.0),
            new Car().Init(2,"Volkswagen",    "Golf",   11000.0),
            new Car().Init(3,"Volkswagen",    "Golf",    7000.0),
            new Car().Init(4,"Volkswagen",    "Passat", 19500.0),
            new Car().Init(5,"Volkswagen",    "Golf",   12500.0),
            new Car().Init(6,"Volkswagen",    "Polo",    9130.0),
            new Car().Init(7,"Audi",          "A4",     16800.0),
            new Car().Init(8,"BMW",           "330",    28900.0),
            new Car().Init(9,"Mercedes-Benz", "B200",   28000.0),
            new Car().Init(10,"Opel",         "Astra",  21000.0)
         };
         // Key-Value Id fortlaufend
         foreach(var c in cars) {
            _cars[c.Id] = c;
            Console.WriteLine(c.AsString());
         }
      }

      internal int CountCars() => _cars.Count;

      internal IList<string> SelectMakes() {
         var makes = new List<string>();
         foreach(Car car in _cars.Values) 
            if(!makes.Contains(car.Make)) makes.Add(car.Make);
         makes.Sort();
         return makes;
      }

      internal IList<string> SelectModels(string make) {
         var models = new List<string>();
         foreach(Car car in _cars.Values) {
            if (car.Make != make) continue;
            if(!models.Contains(car.Model)) models.Add(car.Model);
         }
         models.Sort();
         return models;
      }
      
      internal IList<Car> SelectCars(string make, string model) {
         var selectCars = new List<Car>();
         foreach(var car in _cars.Values) 
            if(car.Make == make && car.Model == model) selectCars.Add(car);
         return selectCars;
      }

      internal Car? SelectCar(int id) 
         => _cars.ContainsKey(id) ? _cars[id] : null;
      
   }
}
