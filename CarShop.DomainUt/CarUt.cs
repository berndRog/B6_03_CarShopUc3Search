using System;
using System.ComponentModel.DataAnnotations;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CarShop.Domain.Ut {
   [TestClass]
   public class CarUt {

      private User  _user;
      private Car   _car;

      [TestInitialize]
      public void Init() {
         var salt     = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
         var input    = "Geh1m_geh1m";
         var password = AppSecurity.HashPbkdf2(input, salt);
         _user = new User().Init(99, "Erika Mustermann", "e.mustermann@t-online.de", 
                                "ErikaM", password, salt, Role.User);
         _car = new Car().Init(99, "Audi", "A6", 37900,_user);
         _user.AddOfferedCar(_car);
      }

      [TestMethod]
      public void CtorUt() {
         // Arrange
         var car = new Car();
         // Assert
         Assert.IsNotNull(car);
         Assert.IsInstanceOfType(car, typeof(IEntity));
         Assert.IsInstanceOfType(car, typeof(Car));
      }

      private static bool AreCarsEqual(Car expected, Car actual)
         => expected.Id                             == actual.Id    &&
            expected.Make                           == actual.Make  &&
            expected.Model                          == actual.Model &&
            Math.Abs(expected.Price - actual.Price) < 1.0e-6        &&
            expected.User                           == actual.User;
      [TestMethod]
      public void Init1Ut() {
         // Arrange
         var car = new Car().Init(1, "Ford","Fiesta", 1500, _user);
         _user.AddOfferedCar(car);
         // Assert
         Assert.IsNotNull(car);
         Assert.IsInstanceOfType(car, typeof(Car));
      }
      [TestMethod]
      public void GetterCarUt() {
         // Arrange
         // Act
         var make     = _car.Make;
         var model    = _car.Model;
         var price    = _car.Price;
         var user     = _car.User;
         // Assert
         Assert.AreEqual("Audi",   make);
         Assert.AreEqual("A6",     model);
         Assert.AreEqual(37_900.0, price, 1.0e-6);
         Assert.AreEqual(_user,  user);
      }
      [TestMethod]
      public void SetterCarUt() {
         // Arrange, Act 
         _car.Price     = 35_000;
         //var seller     = new User().Update(_seed.User1);
         //_car.User      = seller;
         // Assert
         Assert.AreEqual(35_000.0,  _car.Price, 1.0e-6);
         //Assert.AreEqual(seller,    _car.User);
      }
   }
}