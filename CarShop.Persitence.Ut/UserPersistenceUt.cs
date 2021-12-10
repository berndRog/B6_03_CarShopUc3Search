using System;
using System.Linq;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using CarShop.Domain;

namespace CarShop.Persistence.Ut {
   [TestClass]
   public class UserPersistenceUt {

      #region Init
      private IUnitOfWork _unitOfWork;
      private CSeed       _seed  = new();

      [TestInitialize]
      public void Init() {
         var appConfigBuilder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
         var configuration = appConfigBuilder.Build(); 
         
         var serviceCollection = new ServiceCollection();
         serviceCollection.AddSingleton<IUnitOfWork,  CUnitOfWork>();
         var connectionString = configuration.GetConnectionString("LocalDb");
         serviceCollection.AddDbContext<CDbContext>(options =>
            options.UseSqlServer(connectionString));
         serviceCollection.AddLogging();
         var services = serviceCollection.BuildServiceProvider();

         _unitOfWork  = services.GetService<IUnitOfWork>();
         
         var dbContext = services.GetService<CDbContext>();
         dbContext.Database.EnsureDeleted();
         dbContext.Database.EnsureCreated();
         dbContext.Dispose();

         _seed = new CSeed();

      }
       #endregion

      #region One User without Address
      [TestMethod]
      public void UserWithoutAddress_FindbyIdUt() {
         // Arrange
         var (id, user) = RegisterUser(CreateUser());
         // Act
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithoutAddress_FindUt() {
         // Arrange
         var (_, user) = RegisterUser(CreateUser());
         // Act
         var actual = _unitOfWork.RepositoryUser.Find(u => u.UserName =="MartinM");
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithoutAddress_SelectUt() {
         // Arrange
         var (_, user) = RegisterUser(CreateUser());
         // Act
         var actualUsers = _unitOfWork.RepositoryUser.Select(u => u.Email.Contains("gmx.de"));
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(1, actualUsers.Count());
         var listOfUsers = actualUsers.ToList();
         Assert.IsTrue(listOfUsers[0]?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithoutAddress_SelectAllUt() {
         // Arrange
         var (_, user) = RegisterUser(CreateUser());
         // Act
         var actualUsers = _unitOfWork.RepositoryUser.SelectAll();
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(1, actualUsers.Count());
         var listOfUsers = actualUsers.ToList();
         Assert.IsTrue(listOfUsers[0]?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithoutAddress_InsertUt() {
         // Arrange
         var user = CreateUser();
         // Act
         _unitOfWork.RepositoryUser.Insert(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();         
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithoutAddress_UpdateUt() {
         // Arrange
         var (id, _) = RegisterUser(CreateUser());
         // Act
         var user = _unitOfWork.RepositoryUser.FindById(id);
         user.Email = "m.michel@t-online.de";
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithoutAddressDeleteUt() {
         // Arrange
         var (id, _) = RegisterUser(CreateUser());
         // Act
         var user = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.RepositoryUser.Delete(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         Assert.IsNull(actual);
      }
      #endregion 

      #region One User with Address
      [TestMethod]
      public void UserWithAddress_FindbyIdUt() {
         // Arrange
         var (id, user) = RegisterUserWithAddress(CreateUser());
         // Act
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithtAddress_FindUt() {
         // Arrange
         var (_, user) = RegisterUserWithAddress(CreateUser());
         // Act
         var actual = _unitOfWork.RepositoryUser.Find(u => u.UserName =="MartinM");
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithAddress_SelectUt() {
         // Arrange
         var (_, user) = RegisterUserWithAddress(CreateUser());
         // Act
         var actualUsers = _unitOfWork.RepositoryUser.Select(u => u.Email.Contains("gmx.de"));
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(1, actualUsers.Count());
         var listOfUsers = actualUsers.ToList();
         Assert.IsTrue(listOfUsers[0]?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithAddress_SelectAllUt() {
         // Arrange
         var (_, user) = RegisterUserWithAddress(CreateUser());
         // Act
         var actualUsers = _unitOfWork.RepositoryUser.SelectAll();
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(1, actualUsers.Count());
         var listOfUsers = actualUsers.ToList();
         Assert.IsTrue(listOfUsers[0]?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithAddress_InsertUt() {
         // Arrange
         var user = CreateUser();
         // Act
         _unitOfWork.RepositoryUser.Insert(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         user.AddAddress("Herbert-Meyer-Str. 7","29556 Suderburg");
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithAddress_UpdateUt() {
         // Arrange
         var (id, _) = RegisterUserWithAddress(CreateUser());
         // Act
         var user = _unitOfWork.RepositoryUser.FindById(id);
         user.Email = "m.michel@t-online.de";
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWithAddress_DeleteUt() {
         // Arrange
         var (id, _) = RegisterUserWithAddress(CreateUser());
         // Act
         var user = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.RepositoryUser.Delete(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         Assert.IsNull(actual);
      }
      #endregion

      #region Six Users with and without Addresses
      [TestMethod]
      public void SixUsers_FindByIdUt() {
         // Arrange
         SeedUsersAndInsert();
         // Act
         var actual = _unitOfWork.RepositoryUser.FindById(_seed.User02.Id);
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(actual?.IsEqual(_seed.User02));
      }
      [TestMethod]
      public void SixUsers_FindUt() {
         // Arrange
         SeedUsersAndInsert();
         // Act
         var actual = _unitOfWork.RepositoryUser.Find(user => user.UserName =="BBauer");
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(actual?.IsEqual(_seed.User02));
      }
      [TestMethod]
      public void SixUsers_SelectUt() {
         // Arrange
         SeedUsersAndInsert();
         // Act
         var actualUsers = _unitOfWork.RepositoryUser.Select(u => u.Email.Contains("google.com"));
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(2, actualUsers.Count());
         var user3 = actualUsers.FirstOrDefault(u => u.Id == _seed.User03.Id);
         var user6 = actualUsers.FirstOrDefault(u => u.Id == _seed.User06.Id);
         Assert.IsTrue(user3?.IsEqual(_seed.User03));
         Assert.IsTrue(user6?.IsEqual(_seed.User06));
      }
      [TestMethod]
      public void SixUsers_SelectAllUt() {
         // Arrange
         SeedUsersAndInsert();
         // Act
         var actualUsers = _unitOfWork.RepositoryUser.SelectAll();
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(6, actualUsers.Count());
      }
      #endregion
            

      User CreateUser() {
         var name     = "Martin Michel";
         var email    = "m.michel@gmx.de";
         var userName = "MartinM";
         var password = "geh1m_Geh1m";
         var salt     = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
         var hashed   = AppSecurity.HashPbkdf2(password, salt);
         var user     = new User().Init(0, name, email, userName, hashed, salt, Role.Customer);
         return user;
      }

      (int,User) RegisterUser(User user) {
         _unitOfWork.RepositoryUser.Insert(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         return (user.Id, user);
      }

      (int,User) RegisterUserWithAddress(User user) {
         _unitOfWork.RepositoryUser.Insert(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         user.AddAddress("Herbert-Meyer-Str. 7","29556 Suderburg");
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         return (user.Id, user);
      }

      
      void SeedUsersAndInsert() {
         _unitOfWork.RepositoryUser.Insert(_seed.User01);
         _unitOfWork.RepositoryUser.Insert(_seed.User02);
         _unitOfWork.RepositoryUser.Insert(_seed.User03);
         _unitOfWork.RepositoryUser.Insert(_seed.User04);
         _unitOfWork.RepositoryUser.Insert(_seed.User05);
         _unitOfWork.RepositoryUser.Insert(_seed.User06);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();

         _unitOfWork.RepositoryUser.Attach(_seed.User02);
         _unitOfWork.RepositoryUser.Attach(_seed.User04);
         _unitOfWork.RepositoryUser.Attach(_seed.User06);
         _seed.User02.AddAddress("Bahnhofstr. 1", "29525 Uelzen");
         _seed.User04.AddAddress("Schloßplatz 23", "29227 Celle");
         _seed.User06.AddAddress("Wallstr. 17", "21335 Lüneburg");
         _unitOfWork.RepositoryUser.Update(_seed.User02);
         _unitOfWork.RepositoryUser.Update(_seed.User04);
         _unitOfWork.RepositoryUser.Update(_seed.User06);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
      }

      
      #region One User with one Car
      [TestMethod]
      public void UserWithCar_InsertCarUt() {
         // Arrange
         var (id, _) = RegisterUser(CreateUser());       
         // Act
         var user = _unitOfWork.RepositoryUser.FindById(id);
         user.AddOfferedCar(_seed.Car01);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         var carId = _seed.Car01.Id;
         // Assert
         var actual = _unitOfWork.RepositoryCar.FindById(id);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(_seed.Car01));
      }
      [TestMethod]
      public void UserWithCar_UpdateCarUt() {
         // Arrange
         var (userId, _) = RegisterUser(CreateUser());       
         var user = _unitOfWork.RepositoryUser.FindById(userId);
         user.AddOfferedCar(_seed.Car01);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         var carId = _seed.Car01.Id;
         // Act
         user    = _unitOfWork.RepositoryUser.FindById(userId);
         var car = user.OfferedCars.FirstOrDefault(c => c.Id == carId);
         car.Price = 9999;
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryCar.FindById(carId);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(car));
      }
      [TestMethod]
      public void UserWithCar_DeleteCarUt() {
         // Arrange
         var (userId, _) = RegisterUser(CreateUser());       
         var user = _unitOfWork.RepositoryUser.FindById(userId);
         user.AddOfferedCar(_seed.Car01);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         var carId = _seed.Car01.Id;
         // Act
         var car = _unitOfWork.RepositoryCar.FindById(carId);
         user = _unitOfWork.RepositoryUser.FindById(userId);
         user.OfferedCars.Remove(car);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryCar.FindById(carId);
         _unitOfWork.Dispose();
         Assert.IsNull(actual);
      }

      
      [TestMethod]
      public void UserWith3Cars_InsertUt() {
         // Arrange
         var (id, _) = RegisterUser(CreateUser());       
         // Act
         var user = _unitOfWork.RepositoryUser.FindById(id);
         user.AddOfferedCar(_seed.Car01);
         user.AddOfferedCar(_seed.Car05);
         user.AddOfferedCar(_seed.Car09);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWith3Cars_UpdateUt() {
         // Arrange
         var (id, _) = RegisterUser(CreateUser());       
         var user = _unitOfWork.RepositoryUser.FindById(id);
         user.AddOfferedCar(_seed.Car01);
         user.AddOfferedCar(_seed.Car05);
         user.AddOfferedCar(_seed.Car09);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Act
         user       = _unitOfWork.RepositoryUser.FindById(id);
         user.Email = "m.michel@t-online.de";
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void UserWith3Cars_DeleteUt() {
         // Arrange
         var (id, _) = RegisterUser(CreateUser());       
         var user = _unitOfWork.RepositoryUser.FindById(id);
         user.AddOfferedCar(_seed.Car01);
         user.AddOfferedCar(_seed.Car05);
         user.AddOfferedCar(_seed.Car09);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Act
         user       = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.RepositoryUser.Delete(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.Dispose();
         Assert.IsNull(actual);
      }
      #endregion

      #region Six Users
      [TestMethod]
      public void SixUsersWith3Car_FindByIdUt() {
         // Arrange register user and add address
         SeedUsersAndInsert();
         // Act add 3 cars and save to database
         var user = _unitOfWork.RepositoryUser.FindById(_seed.User02.Id);
         user.AddOfferedCar(_seed.Car01);
         user.AddOfferedCar(_seed.Car05);
         user.AddOfferedCar(_seed.Car09);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();
         Assert.IsTrue(actual?.IsEqual(user));
      }
      [TestMethod]
      public void DeleteUserWithAddressWith3CarUt() {
         // Arrange register user, add address and 3 cars and save to database
         var( id, _) = RegisterUser(CreateUser());
         var user = _unitOfWork.RepositoryUser.FindById(id);
         user.AddAddress("Herbert-Meyer-Str. 7", "29556 Suderburg");
         user.AddOfferedCar(_seed.Car01);
         user.AddOfferedCar(_seed.Car05);
         user.AddOfferedCar(_seed.Car09);
         _unitOfWork.RepositoryUser.Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Act 
         user = _unitOfWork.RepositoryUser.FindById(id);
         _unitOfWork.RepositoryUser.Delete(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();
         Assert.IsNull(actual);
      }
      #endregion

   }
}
