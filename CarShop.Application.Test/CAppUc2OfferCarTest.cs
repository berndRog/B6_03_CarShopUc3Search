using System.IO;
using System.Linq;
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
      private CSeed           _seed  = new();
      private IAppCore        _appCore;
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
         serviceCollection.AddSingleton<IAppCore,        CAppCore>();
         serviceCollection.AddSingleton<IAppUc1User,     CAppUc1User>();
         serviceCollection.AddSingleton<IAppUc2OfferCar, CAppUc2OfferCar>();
         serviceCollection.AddSingleton<IUnitOfWork,     CUnitOfWork>();
         var connectionString = configuration.GetConnectionString("LocalDb");
         serviceCollection.AddDbContext<CDbContext>(options =>
            options.UseSqlServer(connectionString));
         serviceCollection.AddLogging();
         var services = serviceCollection.BuildServiceProvider();

         _appCore        = services.GetService<IAppCore>();
         _appUc1User     = services.GetService<IAppUc1User>();
         _appUc2OfferCar = services.GetService<IAppUc2OfferCar>();
         _unitOfWork     = services.GetService<IUnitOfWork>();

         _dbContext = services.GetService<CDbContext>();
         _dbContext.Database.EnsureDeleted();
         _dbContext.Database.EnsureCreated();
         _dbContext.Dispose();
      }

      [TestCleanup]
      public void Teardown() {
         _dbContext.Dispose();
      }
      #endregion


      private (Car, User) RegiserUserWith3OfferedCars() {
         User user   = CreateUser();
         _unitOfWork.RepositoryUser
                    .Insert(user);
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         _seed.Car01.Id = 0;
         _seed.Car02.Id = 0;
         _seed.Car03.Id = 0;
         user.AddOfferedCar(_seed.Car01);
         user.AddOfferedCar(_seed.Car02);
         user.AddOfferedCar(_seed.Car03);
         _unitOfWork.RepositoryUser
                    .Update(user);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         return (_seed.Car02, user);
      }
   }
}