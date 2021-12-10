using System;
using System.Collections.Generic;
using System.Linq;
using CarShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CarShop.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CarShop.Persistence {
   internal class CRepositoryCar: IRepositoryCar {

      #region fields
      private readonly CDbContext _dbContext;
      private readonly ILogger<CRepositoryCar> _log;
      #endregion

      #region ctor
      public CRepositoryCar(
         CDbContext dbContext, 
         ILoggerFactory loggerFactory
      ) {   
         _dbContext = dbContext;
         _log = loggerFactory.CreateLogger<CRepositoryCar>();
         _log.LogInformation($"Ctor() {GetHashCode()}");
      }
      #endregion

      #region methods IRepositoryCar
      // Select * From Cars Where Id = p1
      public Car? FindById(int id)
         => _dbContext.Cars
                      .Include(c => c.User)
                      .FirstOrDefault(c => c.Id == id);

      // Select * From Cars Where predicate
      public Car? Find(Func<Car,bool> predicate) 
         => _dbContext.Cars
                      .Include(c => c.User)
                      .SingleOrDefault(predicate);

      public IEnumerable<Car> Select(Func<Car, bool> predicate)
         => _dbContext.Cars
                      .Include(c => c.User)
                      .Where(predicate).ToList();

      public IEnumerable<Car> SelectAll() 
         => _dbContext.Cars
                      .Include(car => car.User)
                      .ToList();

      public void Attach(Car car) 
         => _dbContext.Cars
                      .Attach(car);

      public void Delete(Car car) 
         => _dbContext.Cars.Remove(car);
     
      #endregion

      #region methods Count 
      // Select Count(*) From Cars
      public int Count() 
         => _dbContext.Cars
                      .Count();

      // Select Count(*) From Cars As c Where (c.Make = make)
      public int CountByMake(string make) 
         => _dbContext.Cars
                      .Count(c => c.Make == make);

      // Select Count(*) From Cars As c Where (c.Make = make) And (c.Model = model)
      public int CountByMakeAndModel(string make, string model) 
         => _dbContext.Cars
                      .Count(c => c.Make == make && c.Model == model);

      // Select Count(*) From Cars As c Where (c.Make   = p1) And (c.Model  = p2) And
      //                                      (c.Price >= p3) And (c.Price <= p4) 
      public int CountByCarTs(CarTs carTs)
         => _dbContext.Cars.Count(c => c.Make  == carTs.Make && 
                                       c.Model == carTs.Model &&
                                       c.Price >= carTs.PriceFrom &&
                                       c.Price <= carTs.PriceTo);
      #endregion
      
      #region methods Select
      // Select Distinct c.Make from Cars as c Order By c.Make
      public IEnumerable<string> SelectMakes()     
         =>_dbContext.Cars
                     .Select(car => car.Make) // projection row make only
                     .Distinct()              // each make only once
                     .OrderBy(make => make)   // order asc
                     .ToList(); 
      // Select Distinct c.Model from Cars as c Where c.Make = make Order By c.Model
      public IEnumerable<string> SelectModels(string make) 
         => _dbContext.Cars
                      .Where (car => car.Make == make)
                      .Select(car => car.Model)
                      .Distinct()
                      .OrderBy(model => model)
                      .ToList();

      public IEnumerable<Car> SelectCars(CarTs carTs) 
          => _dbContext.Cars
                       .Where(c => c.Make  == carTs.Make      && 
                                   c.Model == carTs.Model     &&
                                   c.Price >= carTs.PriceFrom &&
                                   c.Price <= carTs.PriceTo)
                       .ToList();
      #endregion
   }
}
