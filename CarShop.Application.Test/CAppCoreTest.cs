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
   public class CAppCoreTest {

      private CSeed         _seed;

      private IAppCore    _appCore;
      private IAppUc1User _appUc1User;
      private IUnitOfWork _unitOfWork;
      
      [TestInitialize]
      public void Init() {        

         var appConfigBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
         var configuration = appConfigBuilder.Build();

         var serviceCollection = new ServiceCollection();
         serviceCollection.AddSingleton<IAppCore,    CAppCore>();
         serviceCollection.AddSingleton<IAppUc1User, CAppUc1User>();
         serviceCollection.AddSingleton<IUnitOfWork, CUnitOfWork>();
         var connectionString = configuration.GetConnectionString("LocalDb");
         serviceCollection.AddDbContext<CDbContext>(options =>
            options.UseSqlServer(connectionString));
         serviceCollection.AddLogging();
         var services = serviceCollection.BuildServiceProvider();

         _appCore    = services.GetService<IAppCore>();
         _appUc1User = services.GetService<IAppUc1User>();
         _unitOfWork = services.GetService<IUnitOfWork>();

         var dbContext = services.GetService<CDbContext>();
         dbContext.Database.EnsureDeleted();
         dbContext.Database.EnsureCreated();
         dbContext.Dispose();
      }

      #region Test User Login, Logout
      [TestMethod]
      public void LoginUserTest() {
         // Arrange
         var user = RegisterUser();
         // Act
         var userName = "MartinM";
         var password = "geh1m_Geh1m";
         var result   = _appCore.LoginUser(userName, password);
         // Assert
         Assert.IsTrue(result is Success<User>);
         var actual = result.Data;
         Assert.AreEqual(user.Id, actual.Id);
      }
      [TestMethod]
      public void LoginUserFailsWithWrongUsernameTest() {
         // Arrange
         var user = RegisterUser();
         // Act
         var userName = "123";
         var password = "geh1m_Geh1m";
         var result   = _appCore.LoginUser(userName, password);
         // Assert
         Assert.IsTrue(result is Error<User>);
         Assert.AreNotEqual(user.UserName, userName );
      }
      [TestMethod]
      public void LoginUserFailsWithWrongPasswordTest() {
         // Arrange
         var user = RegisterUser();         
         // Act
         var userName = "MartinM";
         var password = "123";
         var result   = _appCore.LoginUser(userName, password);
         // Assert
         Assert.IsTrue(result is Error<User>);
      }
      [TestMethod]
      public void LogoutUserTest() {
         // Arrange
         var user = RegisterUser();
         var userName  = "MartinM";
         var password  = "geh1m_Geh1m";
         _appCore.LoginUser(userName, password);
         // Act
         var result = _appCore.LogoutUser();
         // Assert
         Assert.IsTrue(result is Success<User>);
         var actual = result.Data;
         Assert.IsInstanceOfType(actual,typeof(NullUser));
         Assert.AreEqual(-1, actual.Id);
      }
      [TestMethod]
      public void LoggedInUserTest() {
         // Arrange
         var user     = RegisterUser();
         var userName = "MartinM";
         var password = "geh1m_Geh1m";
         // Act
         var result = _appCore.LoginUser(userName, password);
         var actual = _appCore.LoggedInUser;
         // Assert
         Assert.IsTrue(result is Success<User>);
         Assert.IsNotNull(actual);
         Assert.AreEqual(user.Id,        actual.Id);
         Assert.AreEqual(user.UserName,  actual.UserName );
         Assert.AreEqual(user.Password,  actual.Password);
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
