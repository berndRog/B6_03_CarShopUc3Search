using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using CarShop.Di;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarShop.DataUt {

   [TestClass]
   public class IDataUt {

      private static ServiceProvider? serviceProvider; 
      
      public static void SetupDIContainer() {
         var serviceCollection = new ServiceCollection();
         serviceCollection.AddApplication();
         serviceCollection.AddData();
         serviceProvider = serviceCollection.BuildServiceProvider();
      }

      [TestInitialize]
      public void Init() {
         SetupDIContainer(); 
      }

      [TestMethod]
      public void CountCarsUt() {
         // Arrange
         var data = serviceProvider?.GetService<IData>();
         // Act
         var actual = data?.CountCars();
         // Assert
         Assert.IsNotNull(actual);
         Assert.AreEqual(10, actual);
      }

      [TestMethod]
      public void SelectMakesUt() {
         // Arrange
         var data = serviceProvider?.GetService<IData>();
         // Act
         var actual = data?.SelectMakes();
         // Assert
         Assert.IsNotNull(actual);
         Assert.AreEqual(5,               actual.Count);
         Assert.AreEqual("Audi",          actual[0]);
         Assert.AreEqual("BMW",           actual[1]);
         Assert.AreEqual("Mercedes-Benz", actual[2]);
         Assert.AreEqual("Opel",          actual[3]);
         Assert.AreEqual("Volkswagen",    actual[4]);

      }

      [TestMethod]
      public void SelectModelsUt() {
         // Arrange
         var data = serviceProvider?.GetService<IData>();
         var make = "Volkswagen";
         // Act
         var actual = data?.SelectModels(make);
         // Assert
         Assert.IsNotNull(actual);
         Assert.AreEqual(3,        actual.Count);
         Assert.AreEqual("Golf",   actual[0]);
         Assert.AreEqual("Passat", actual[1]);
         Assert.AreEqual("Polo",   actual[2]);
      }

      [TestMethod]
      public void SelectCarsUt() {
         // Arrange
         var data  = serviceProvider?.GetService<IData>();
         var make  = "Volkswagen";
         var model = "Golf";
         // Act
         var actual = data?.SelectCars(make, model);
         // Assert
         Assert.IsNotNull(actual);
         Assert.AreEqual(4, actual.Count);
      }

      [TestMethod]
      public void SelectCar() {
         // Arrange
         var data = serviceProvider?.GetService<IData>();
         var id   = 2;
         // Act
         var car = data?.SelectCar(id);
         // Assert
         Assert.IsNotNull(car);
         Assert.AreEqual("Volkswagen", car.Make);
         Assert.AreEqual("Golf",       car.Model);
         Assert.AreEqual(11000.0,      car.Price, 0.001);
         
         
      }
   }
}