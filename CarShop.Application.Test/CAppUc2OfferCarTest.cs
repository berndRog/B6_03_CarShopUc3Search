using System.IO;
using CarShop.Domain;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;
using CarShop.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CarShop.Application.Test {
   [TestClass]
   public class CAppUc2OfferCarTest {

      #region Init
      private CSeed           _seed;
      private IAppUc1User     _appUc1User;
      private IAppUc2OfferCar _appUc2OfferCar;
      private IUnitOfWork     _unitOfWork;
      private CDbContext      _dbContext;
      
      [TestInitialize]
      public void Init() {        

         var appConfigBuilder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
         var configuration = appConfigBuilder.Build();


         var serviceCollection = new ServiceCollection();
         serviceCollection.AddSingleton<IAppUc1User,     CAppUc1User>();
         serviceCollection.AddSingleton<IAppUc2OfferCar, CAppUc2OfferCar>();
         serviceCollection.AddSingleton<IUnitOfWork,     CUnitOfWork>();
         var connectionString = configuration.GetConnectionString("LocalDb");
         serviceCollection.AddDbContext<CDbContext>(options =>
            options.UseSqlServer(connectionString));
         serviceCollection.AddLogging();
         var services = serviceCollection.BuildServiceProvider();

         _appUc1User     = services.GetService<IAppUc1User>();
         _appUc2OfferCar = services.GetService<IAppUc2OfferCar>();
         _unitOfWork     = services.GetService<IUnitOfWork>();

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

      [TestMethod]
      public void OfferCarTest() {
         // Arrange
         User user   = CreateUser();
         var  result = _appUc1User.RegisterUser(user);
         Car  car    = _seed.Car01;
         // Act 
         _appUc2OfferCar.AddOfferedCar(user, car);
         // Assert
         var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
         //Assert.

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