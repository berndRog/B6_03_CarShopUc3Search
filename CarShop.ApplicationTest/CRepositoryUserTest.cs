using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarShop.Domain;
using CarShop.Domain.Ut;
using CarShop.Application.Fakes;
using CarShop.Domain.Entities;
using CarShop.Domain.Seed;
using CarShop.Application.Seed;
using CarShop.Persistence;
using CarShop.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarShop.Application.Test {
   
   [TestClass]
   public class CRepositoryUserTest {
      private User            _user;
      private Car             _car;
      private CSeed           _seed;
      private ISeedRepos      _seedRepos;
      private IUnitOfWork     _unitOfWork;
      private ServiceProvider _serviceProvider; 
      private CDbContext      _dbContext;
      
      [TestInitialize]
      public void Init() {
       var serviceCollection = new ServiceCollection();
         serviceCollection.AddLogging(builder => { 
         //   builder.AddDebug();
         });
         
         serviceCollection.AddDbContext<Persistence.Database.CDbContext>(options => {
//          options.UseInMemoryDatabase("InMemoryCarShop");
            options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CarShopTest;Integrated Security=True;");
            options.EnableSensitiveDataLogging();
//          options.LogTo(message => Debug.WriteLine(message)); 
         });
         serviceCollection.AddScoped<ISeedRepos,CSeedRepos>();
         serviceCollection.AddScoped<IUnitOfWork, Persistence.CUnitOfWork>();
         _serviceProvider = serviceCollection.BuildServiceProvider();
         _seedRepos       = _serviceProvider.GetService<ISeedRepos>();
         _unitOfWork      = _serviceProvider.GetService<IUnitOfWork>();
         _dbContext = _serviceProvider.GetService<Persistence.Database.CDbContext>();
         _dbContext.Users.RemoveRange(_dbContext.Users.ToList());
         _dbContext.Addresses.RemoveRange(_dbContext.Addresses.ToList());
         _dbContext.Cars.RemoveRange(_dbContext.Cars.ToList());
         _dbContext.SaveChanges();

         _seed = new CSeed();
         _seed.SetupDefaultUserWithOneOfferedCar(out _user, out _car);    
      }

      #region User
      [TestMethod]
      public void SeedTest() {
         // Act
         // Register User
         var user = _seed.DefaultUser;
         _unitOfWork.RepositoryUser
                    .Insert(user);
         _unitOfWork.Save();
         _unitOfWork.Dispose();

         // Add Car01
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car01); 
         _seed.Car01.User = user;
         user.AddOfferedCar(_seed.Car01);        
         _unitOfWork.RepositoryUser
                    .Update(user); 
         _unitOfWork.Save();
         _unitOfWork.Dispose();

         // Add Car02
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car02); 
         _seed.Car01.User = user;
         user.AddOfferedCar(_seed.Car02);        
         _unitOfWork.RepositoryUser
                    .Update(user); 
         _unitOfWork.Save();
         _unitOfWork.Dispose();
         
         // Add Car03
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car03); 
         _seed.Car01.User = user;
         user.AddOfferedCar(_seed.Car03);        
         _unitOfWork.RepositoryUser
                    .Update(user); 
         _unitOfWork.Save();
         _unitOfWork.Dispose();

         // Register User1
         var user1 = _seed.User1;
         _unitOfWork.RepositoryUser
                    .Insert(user1);
         _unitOfWork.Save();
         _unitOfWork.Dispose();

         // Add 4 Cars offered by User1
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car04);          
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car05); 
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car06); 
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car07); 
         _unitOfWork.Save();

         _seed.SetupOfferedCarsUser1();
         _unitOfWork.RepositoryUser
                    .Update(user1); 
         _unitOfWork.Save();
         _unitOfWork.Dispose();


         // Register User2
         var user2 = _seed.User2;
         _unitOfWork.RepositoryUser
                    .Insert(user2);
         _unitOfWork.Save();
         _unitOfWork.Dispose();

         // Add 4 Cars offered by User2
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car08);          
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car09); 
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car10); 
         _unitOfWork.RepositoryCar
                    .Insert(_seed.Car11); 
         _unitOfWork.Save();

         _seed.SetupOfferedCarsUser2();
         _unitOfWork.RepositoryUser
                    .Update(user2); 
         _unitOfWork.Save();
         _unitOfWork.Dispose();

        
         // Assert

         var countCars = _unitOfWork.RepositoryCount.CountAllCars();

         var actual = _unitOfWork.RepositoryUser.FindById(user.Id); 
         var actual1 = _unitOfWork.RepositoryUser.FindById(user1.Id); 
         var actual2 = _unitOfWork.RepositoryUser.FindById(user2.Id); 

         Assert.AreEqual(user.Id, actual.Id);
         Assert.AreEqual(user1.Id, actual1.Id);
         Assert.AreEqual(user2.Id, actual2.Id);

      }


      [TestMethod]
      public void FindByIdTest() {
         // Arrange
         _seedRepos.Seed(_unitOfWork, _seed);
         var user = _seed.User1;
         // Act
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id); // "04333507046076985395";
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(user.Id, actual.Id);
      }
      [TestMethod]
      public void FindByUserNameTest() {
         // Arrange
         _seedRepos.Seed(_unitOfWork, _seed); 
         var user = _seed.User1; 
         // Act
         var actual = _unitOfWork.RepositoryUser.Find(u => u.UserName == user.UserName); // "B.Bauer";
          _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(user.Id, actual.Id);
      }
      [TestMethod]
      public void SelectAllTest() {
         // Arrange
         _seedRepos.Seed(_unitOfWork, _seed);
         // Act
         var actual = _unitOfWork.RepositoryUser.SelectAll();
          _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(2, actual.Count());
      }
      [TestMethod]
      public void InsertTest() {
         // Arrange, Act 
         _unitOfWork.RepositoryUser.Insert(_user);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
         var actual = _unitOfWork.RepositoryUser.FindById(_user.Id);
          _unitOfWork.Dispose();
         Assert.AreEqual(_user.Id, actual.Id);
      }

      [TestMethod]
      public void UpdateTest() {
         // Arrange, Act 
         _unitOfWork.RepositoryUser.Insert(_user);
         _unitOfWork.Save();
         _user.Name  = "Erika Müller";
         _user.Email = "E.mueller@gmail.com";
         // Act
         _unitOfWork.RepositoryUser.Update(_user);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
         var actual = _unitOfWork.RepositoryUser.FindById(_user.Id);
         _unitOfWork.Dispose();
         Assert.AreEqual(_user.Id,    actual.Id);
         Assert.AreEqual(_user.Name,  actual.Name);
         Assert.AreEqual(_user.Email, actual.Email);
      }
      [TestMethod]
      public void DeleteTest() {
         // Arrange
         _unitOfWork.RepositoryUser.Insert(_user);
          _unitOfWork.Save();
         // Act 
         _unitOfWork.RepositoryUser.Delete(_user);
         var result = _unitOfWork.Save();
         // Assert
         Assert.IsTrue(result);
         var actual = _unitOfWork.RepositoryUser.FindById(_user.Id);
         Assert.IsNull(actual);
      }
      #endregion

   }
}
