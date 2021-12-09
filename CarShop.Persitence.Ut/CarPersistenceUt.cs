using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using CarShop.Domain;
using CarShop.Domain.ValueObjects;

namespace CarShop.Persistence.Ut {
   [TestClass]
   public class CarPersistenceUt {

      #region Init
      private IUnitOfWork _unitOfWork;
      private CSeed       _seed = new();
      
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
      }
      #endregion

      #region Car
      [TestMethod]
      public void FindbyIdUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         var car = _seed.Car03;
         // Act
         var actual = _unitOfWork.RepositoryCar.FindById(car.Id);
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(car.IsEqual(actual));
      }
      [TestMethod]
      public void FindUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         var car = _seed.Car03;
         // Act
         var actual = _unitOfWork.RepositoryCar.Find(c => c.Model =="A4");
         _unitOfWork.Dispose();
         // Assert
         Assert.IsTrue(car.IsEqual(actual));
      }
      [TestMethod]
      public void SelectUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         // Act
         var actualCars = _unitOfWork.RepositoryCar.Select(c => c.Make.Contains("Volks"));
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(5, actualCars.Count());
      }
      [TestMethod]
      public void SelectAllUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         // Act
         var actualCars = _unitOfWork.RepositoryCar.SelectAll();
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         // Assert
         Assert.AreEqual(9, actualCars.Count());
      }
      #endregion 
      
      #region Count 
      [TestMethod]
      public void CountCarsUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         // Act 
         var count = _unitOfWork.RepositoryCar.Count();
         // Assert
         Assert.AreEqual(9, count);
      }
      [TestMethod]
      public void CountbyMakeUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         var make = "Volkswagen";
         // Act 
         var count = _unitOfWork.RepositoryCar.CountByMake(make);
         // Assert
         Assert.AreEqual(5, count);
      }
      [TestMethod]
      public void CountbyMakeAndModelUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         var make  = "Volkswagen";
         var model = "Golf";
         // Act 
         var count = _unitOfWork.RepositoryCar.CountByMakeAndModel(make, model);
         // Assert
         Assert.AreEqual(3, count);
      }
      [TestMethod]
      public void CountbyCarsTsUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         var carTs = new CarTs().Init("Volkswagen","Golf",5000,15000);
         // Act 
         var count = _unitOfWork.RepositoryCar.CountByCarTs(carTs);
         // Assert
         Assert.AreEqual(3, count);
      }
      #endregion
      
      #region Find, Select 
      [TestMethod]
      public void SelectMakesUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         // Act 
         var makes = _unitOfWork.RepositoryCar.SelectMakes();
         // Assert
         var actualList = makes.ToList();
         Assert.AreEqual(5, makes.Count());
         Assert.AreEqual("Audi",         actualList[0]);
         Assert.AreEqual("BMW",          actualList[1]);
         Assert.AreEqual("Mercedes-Benz",actualList[2]);       
         Assert.AreEqual("Opel",         actualList[3]);
         Assert.AreEqual("Volkswagen",   actualList[4]);
      }
      [TestMethod]
      public void SelectModelsUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         var make = "Volkswagen";
         // Act 
         var models = _unitOfWork.RepositoryCar.SelectModels(make);
         // Assert
         var actualList = models.ToList();
         Assert.AreEqual(3, models.Count());
         Assert.AreEqual("Golf",   actualList[0]);
         Assert.AreEqual("Passat", actualList[1]);
         Assert.AreEqual("Polo",   actualList[2]);       
      }
      [TestMethod]
      public void SelectCarsUt() {
         // Arrange
         SeedUsersWithAddresesAndCarsAndInsert();
         var make = "Volkswagen";
         // Act 
         var models = _unitOfWork.RepositoryCar.SelectModels(make);
         // Assert
         var actualList = models.ToList();
         Assert.AreEqual(3,        models.Count());
         Assert.AreEqual("Golf",   actualList[0]);
         Assert.AreEqual("Passat", actualList[1]);
         Assert.AreEqual("Polo",   actualList[2]);       
      }
      #endregion
          
      private void SeedUsersWithAddresesAndCarsAndInsert() {
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
         
         _unitOfWork.RepositoryUser.Attach(_seed.User02);
         _seed.User02.AddOfferedCar(_seed.Car01);
         _seed.User02.AddOfferedCar(_seed.Car02);
         _unitOfWork.RepositoryUser.Update(_seed.User02);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         
         
         _unitOfWork.RepositoryUser.Attach(_seed.User03);
         _seed.User03.AddOfferedCar(_seed.Car03);
         _seed.User03.AddOfferedCar(_seed.Car04);
         _seed.User03.AddOfferedCar(_seed.Car05);
         _unitOfWork.RepositoryUser.Update(_seed.User03);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();

         _unitOfWork.RepositoryUser.Attach(_seed.User04);
         _seed.User04.AddOfferedCar(_seed.Car06);
         _seed.User04.AddOfferedCar(_seed.Car07);
         _seed.User04.AddOfferedCar(_seed.Car08);
         _seed.User04.AddOfferedCar(_seed.Car09);
         _unitOfWork.RepositoryUser.Update(_seed.User04);
         _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         
      }      
   }
}