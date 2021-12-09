using CarShop.Application;
using CarShop.Domain;
using CarShop.Domain.Entities;
using CarShop.Domain.Seed;
using CarShop.Persistence;
using CarShop.Persistence.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using System.Linq;

namespace CarShop.Application.Test {
/*
   [TestClass]
   public class CApplicationCarTest {
      
      private CSeed         _seed;
      private User          _user;
      private Car           _car;
      private IApplication  _application;
      private IUnitOfWork   _unitOfWork;
      private CDbContext    _dbContext;


      [TestInitialize]
      public void Init() {
         _seed = new CSeed();
         _user  = new User(_seed.DefaultUser);
         _car   = new Car (_seed.DefaultCar);

         var serviceCollection = new ServiceCollection();
         serviceCollection.AddLogging(builder => { 
//          builder.AddDebug
         });
         serviceCollection.AddDbContext<CDbContext>(options => {
            options.UseInMemoryDatabase("InMemoryCarShop");
//          options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CarShopTest;Integrated Security=True;MultipleActiveResultSets=true");
            options.EnableSensitiveDataLogging();
            options.LogTo(message => Debug.WriteLine(message));
         });
         serviceCollection.AddSingleton<IApplication, CApplication>();
         serviceCollection.AddSingleton<IRepository,  CRepository>();

         var serviceProvider = serviceCollection.BuildServiceProvider();

         _dbContext   = serviceProvider.GetService<CDbContext>();

//       _dbContext.Database.EnsureDeleted();
         _dbContext.Database.EnsureCreated();

         _dbContext.Users.     RemoveRange(_dbContext.Users.     ToList());
         _dbContext.Addresses. RemoveRange(_dbContext.Addresses. ToList());
         _dbContext.Cars.      RemoveRange(_dbContext.Cars.      ToList());
         _dbContext.MarkedCars.RemoveRange(_dbContext.MarkedCars.ToList());
         _dbContext.SaveChanges();
       
         _application = serviceProvider.GetService<IApplication>();
         _unitOfWork  = serviceProvider.GetService<IRepository>();
      }

 
      #region Tests Car AddOfferCar, UpdateOfferedCar, DeleteOfferedCar
      [TestMethod]
      public void AddOfferedCarTest() {
         // Arrange
         _application.RegisterUser(_user);
         var car = new Car().Init("Ford", "Fiesta", 1500, 2005, 187_000, null);
         // Act
         var result = _application.AddOfferedCar(_user, car);
         // Assert 
         Assert.IsTrue(result is Success<Car>);
         var offeredCar       = result.Data; // 
         var actualCar        = _unitOfWork.SelectCarById(car.Id, UnitOfWork.CrCl);
         var actualUser       = _unitOfWork.SelectUserById(_user.Id, UnitOfWork.CrCl);
         var carInOfferedCars = actualUser.GetOfferedCarById(car.Id);
         Assert.AreEqual(offeredCar.Id,   actualCar.Id);
         Assert.AreEqual(_user.Id,        actualUser.Id);
         Assert.AreEqual(offeredCar.Id,   carInOfferedCars.Id);
         Assert.AreEqual(offeredCar.User, actualCar.User);
      }
      [TestMethod]
      public void UpdateOfferedCarTest() {
         // Arrange
         _application.RegisterUser(_user);
         var car = new Car().Init("Ford", "Fiesta", 1500, 2005, 187_000, null);  
         _application.AddOfferedCar(_user, car);
         // Act
         car.Km    = 123456;
         car.Price = 9999.99;
         var result = _application.UpdateOfferedCar(_user, car);
         // Assert 
         Assert.IsTrue(result is Success<Car>);
         var updatedCar       = result.Data; // 
         var actualCar        = _unitOfWork.SelectCarById(car.Id, UnitOfWork.CrCl);
         var actualUser       = _unitOfWork.SelectUserById(_user.Id, UnitOfWork.CrCl);
         var carInOfferedCars = actualUser.GetOfferedCarById(car.Id);
         Assert.AreEqual(updatedCar.Id, actualCar.Id);
         Assert.AreEqual(_user.Id,         actualUser.Id);
         Assert.AreEqual(updatedCar.Id, carInOfferedCars.Id);
         Assert.AreEqual(updatedCar.Price, actualCar.Price, 1.0e-6);
         Assert.AreEqual(updatedCar.Km,    actualCar.Km);
      }
      [TestMethod]
      public void RemoveOfferedCarUt() {
         // Arrange
         _application.RegisterUser(_user);
         var car        = new Car().Init("Ford", "Fiesta", 1500, 2005, 187_000, null);
         var result     = _application.AddOfferedCar(_user, car);
         var offeredCar = result.Data;
         _application.AddMarkedCar(_seed.User1, offeredCar);
         // Act
         result = _application.RemoveOfferedCar(_user, offeredCar);
         // Assert 
         Assert.IsTrue(result is Success<Car>);
         var actualCar  = _unitOfWork.SelectCarById(car.Id, UnitOfWork.CrCl);
         var actualUser = _unitOfWork.SelectUserById(_user.Id, UnitOfWork.CrCl);
         var carInOfferedCars = actualUser.GetOfferedCarById(car.Id);
         Assert.IsNull(actualCar);
         Assert.IsNull(carInOfferedCars);
      }
      #endregion
   }
*/
}