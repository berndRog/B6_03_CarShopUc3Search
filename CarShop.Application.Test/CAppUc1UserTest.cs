using CarShop.Domain;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;
using CarShop.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;

namespace CarShop.Application.Test {
   [TestClass]
   public class CAppUc1UserTest {

      #region Init
      private CSeed       _seed;
      private IAppUc1User _appUc1User;
      private IUnitOfWork _unitOfWork;
      private CDbContext  _dbContext;
      
      [TestInitialize]
      public void Init() {        

         var appConfigBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
         var configuration = appConfigBuilder.Build();


         var serviceCollection = new ServiceCollection();
         serviceCollection.AddSingleton<IAppUc1User,  CAppUc1User>();
         serviceCollection.AddSingleton<IUnitOfWork,  CUnitOfWork>();
         var connectionString = configuration.GetConnectionString("LocalDb");
         serviceCollection.AddDbContext<CDbContext>(options =>
            options.UseSqlServer(connectionString));
         serviceCollection.AddLogging();
         var services = serviceCollection.BuildServiceProvider();

         _appUc1User = services.GetService<IAppUc1User>();
         _unitOfWork               = services.GetService<IUnitOfWork>();

         _dbContext = services.GetService<CDbContext>();
         _dbContext.Database.EnsureDeleted();
         _dbContext.Database.EnsureCreated();
         _dbContext.Dispose();
         
         _seed = new CSeed();
      }

      [TestCleanup]
      public void Teardown() {
         _dbContext.Dispose();
      }
      #endregion
      
      #region Test User Create, Register
      [TestMethod]
      public void CreateUserTest() {
         // Arrange
         var name     = "Martin Michel";
         var email    = "m.michel@gmx.de";
         var userName = "MartinM";
         var password = "geh1m_Geh1m";
         // Act
         var result = _appUc1User.CreateUser(name, email, userName, password, Role.User);
         // Assert
         Assert.IsTrue(result is Success<User>);
         var actual = result.Data;
         var hashed = AppSecurity.HashPbkdf2(password, actual.Salt);
         Assert.IsInstanceOfType(result,typeof(Success<User>));
         Assert.IsInstanceOfType(result.Data, typeof(User));
         Assert.AreEqual("Martin Michel",   actual.Name);
         Assert.AreEqual("m.michel@gmx.de", actual.Email);
         Assert.AreEqual("MartinM",         actual.UserName);
         Assert.AreEqual(hashed,            actual.Password);
         Assert.AreEqual(Role.User,         actual.Role);

      }
      [TestMethod]
      public void RegisterUserTest() {
         // Arrange
         User user = CreateUser();
         // Act
         var result = _appUc1User.RegisterUser(user);
         // Assert
         Assert.IsTrue(result is Success<User>);
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();
         Assert.IsNotNull(actual);
         Assert.IsTrue(user.IsEqual(actual));
      }     
      #endregion

      #region User Update, Remove
      [TestMethod]
      public void UpdateUserTest() {
         // Arrange
         var user = RegisterUser();
         // Act
         user.Email    = "m.michel@t-online.de";
         user.Role     = Role.Seller;        
         var result = _appUc1User.UpdateUser(user);
         // Assert
         Assert.IsTrue(result is Success<User>);
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();
         Assert.IsNotNull(actual);
         Assert.IsTrue(user.IsEqual(actual));
      }

      public void RemoveUserTest() {
         // Arrange
         var user = RegisterUser();
         // Act
         var result = _appUc1User.RemoveUser(user);    
         // Assert
         Assert.IsTrue(result is Success<User>);
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();
         Assert.IsNull(actual);
      }
      #endregion

      #region AddOrUpdateAddress
      [TestMethod]
      public void AddAddressTest() {
         // Arrange
         var user = RegisterUser();
         // Act
         _appUc1User.AddOrUpdateAddress("Herbert-Meyer-Str. 7","29556 Suderburg", user);
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         _unitOfWork.Dispose();

         
         
      }
      #endregion
      
      private User RegisterUser() {
         var createdUser = CreateUser();
         var result = _appUc1User.RegisterUser(createdUser);

         Assert.IsInstanceOfType(result, typeof(Success<User>));
         if(result is Success<User>) return result.Data;
         Assert.Fail();
         return null;
      }
      private User CreateUser() {
         var name     = "Martin Michel";
         var email    = "m.michel@gmx.de";
         var userName = "MartinM";
         var password = "geh1m_Geh1m";
         var result   = _appUc1User.CreateUser(name, email, userName, password, Role.Customer);
         
         Assert.IsInstanceOfType(result, typeof(Success<User>));
         if(result is Success<User>) return result.Data;
         Assert.Fail();
         return null;
      }
   }
}
