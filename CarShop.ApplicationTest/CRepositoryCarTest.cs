using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarShop.Domain;
using CarShop.Domain.Entities;
using CarShop.Domain.Seed;
using CarShop.Persistence;
using Microsoft.EntityFrameworkCore;
using CarShop.Application.Seed;
using System.Diagnostics;
using System;
using CarShop.Persistence.Database;

namespace CarShop.Application.Test {

   [TestClass]
   public class CRepositoryCarTest {
      private User            _user;
      private Car             _car;
      private CSeed           _seed;
      private ISeedRepos      _seedRepos;
      private IUnitOfWork     _unitOfWork;
      private ServiceProvider _serviceProvider; 
      private CDbContext      _dbContext;
      /*
      [TestInitialize]
      public void Init() {
         var serviceCollection = new ServiceCollection();
         serviceCollection.AddLogging(builder => { 
         //   builder.AddDebug();
         });
         
         serviceCollection.AddDbContext<CDbContext>(options => {
            options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CarShopTest;Integrated Security=True");
            options.EnableSensitiveDataLogging();
//          options.LogTo(message => Debug.WriteLine(message)); 
         });
         serviceCollection.AddScoped<ISeedRepos,CSeedRepos>();
         serviceCollection.AddScoped<IUnitOfWork,CUnitOfWork>();
         _serviceProvider = serviceCollection.BuildServiceProvider();
         _seedRepos  = _serviceProvider.GetService<ISeedRepos>();
         _unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
         _dbContext       = _serviceProvider.GetService<CDbContext>();
         _dbContext.Users.RemoveRange(_dbContext.Users.ToList());
         _dbContext.Addresses.RemoveRange(_dbContext.Addresses.ToList());
         _dbContext.Cars.RemoveRange(_dbContext.Cars.ToList());
         _dbContext.SaveChanges();

         _seed = new CSeed();
         _seed.SetupDefaultUserWithOneOfferedCar(out _user, out _car);     
      }

      [TestMethod]
      public void FindByIdTest() {
         // Arrange
         _seedRepos.Seed(_unitOfWork, _seed);
         var car = _seed.Car04;
         // Act
         var actual = _unitOfWork.RepositoryCar.FindById(car.Id); // 07630585736076985395;
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(car.Id, actual.Id);
      }
      [TestMethod]
      public void SelectAllTest() {
         // Arrange
         _seedRepos.Seed(_unitOfWork, _seed);
         // Act
         var actual = _unitOfWork.RepositoryCar.SelectAll();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(8, actual.Count());
      }
      [TestMethod]
      public void InsertAndFindByIdTest() {
         // Arrange   
         // Act
         _unitOfWork.RepositoryCar.Insert(_car);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
         var actual = _unitOfWork.RepositoryCar.FindById(_car.Id);
         _unitOfWork.Dispose();
         Assert.AreEqual(_car.Id,  actual.Id);
      }
      [TestMethod]
      public void InsertCarsTest() {
         // Arrange
         var cars = new List<Car> {
            _seed.Car01,
            _seed.Car02,
            _seed.Car03
         };
         // Act
         _unitOfWork.RepositoryCar.Insert(cars);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
          var actual = _unitOfWork.RepositoryCar.SelectAll();
         _unitOfWork.Dispose();
         Assert.AreEqual(3, actual.Count());
      }
      [TestMethod]
      public void UpdateTest() {
         // Arrange
         _unitOfWork.RepositoryCar.Insert(_car);
         _unitOfWork.Save();
         _car.Km    = 99999;
         _car.Price = 9999;
         // Act

         _unitOfWork.RepositoryCar.Update(_car);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
         var actual = _unitOfWork.RepositoryCar.FindById(_car.Id);
         _unitOfWork.Dispose();
         Assert.AreEqual(_car.Id, actual.Id);
      }
      [TestMethod]
      public void DeleteTest() {
         // Arrange
         _seed.SetupDefaultUserWithOneOfferedCar(out _user, out _car);   
         _unitOfWork.RepositoryCar.Insert(_car);
         _unitOfWork.Save();
         // Act
         _unitOfWork.RepositoryCar.Delete(_car);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
         var actual = _unitOfWork.RepositoryCar.FindById(_car.Id);
         _unitOfWork.Dispose();
         Assert.IsNull(actual);
      }
      [TestMethod]
      public void DeleteCarsTest() {
         // Arrange
         var cars = new List<Car> {
            _seed.Car01,
            _seed.Car02,
            _seed.Car03
         };
         _unitOfWork.RepositoryCar.Insert(cars);
         _unitOfWork.Save();
         // Act
         _unitOfWork.RepositoryCar.Delete(cars);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
         var actual  = _unitOfWork.RepositoryCount.CountAllCars();
         _unitOfWork.Dispose();
         Assert.AreEqual(0, actual);
      }
    */
    }
}
