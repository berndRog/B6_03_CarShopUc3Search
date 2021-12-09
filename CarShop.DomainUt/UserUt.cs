using System.Linq;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarShop.Domain.Ut {
   [TestClass]
   public class UserUt {

      private CSeed _seed;
      private User  _user;
 
      [TestInitialize]
      public void Init() {
         var salt     = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
         var input    = "Geh1m_geh1m";
         var password = AppSecurity.HashPbkdf2(input, salt);
         _seed = new CSeed();
         _user = new User().Init(99, "Erika Mustermann", "e.mustermann@t-online.de", 
                                "ErikaM", password, salt, Role.User);
      }
      [TestMethod]
      public void CtorUt() {
         // Arrange
         var user = new User();
         // Assert
         Assert.IsNotNull(user);
         Assert.IsInstanceOfType(user,typeof(User));
      }
      private static bool AreUsersEqual(User expected, User actual)
         => expected.Id                == actual.Id       &&
            expected.Name              == actual.Name     &&
            expected.Email             == actual.Email    &&
            expected.UserName          == actual.UserName &&
            expected.Password          == actual.Password &&
            expected.Salt              == actual.Salt     &&
            expected.Role              == actual.Role;
      
      [TestMethod]
      public void CtorInit1Ut() {
         // Arrange 
         var password = "geh1m_Geh1m";
         var salt     = AppSecurity.GenerateSalt(24);
         // Act
         var user = new User().Init(99, "Erika Mustermann", "e.mustermann@t-online.de", 
                                    "Erika", password, salt, Role.User);
         // Assert
         Assert.IsNotNull(user);
         Assert.IsInstanceOfType(user, typeof(User));
      }
      [TestMethod]
      public void CtorInit2Ut() {
         // Arrange 
         var password = "geh1m_Geh1m";
         var salt     = AppSecurity.GenerateSalt(24);
         // Act
         var user = new User().Init(99, "Erika Mustermann", "e.mustermann@t-online.de", 
                                    "Erika", password, salt, Role.User);
         // Assert
         Assert.IsNotNull(user);
         Assert.IsInstanceOfType(user, typeof(User));
      }
      [TestMethod]
      public void GetterUt() {
         // Arrange
         var hashed = AppSecurity.HashPbkdf2("Geh1m_geh1m", _user.Salt);
         // Act
         var id       = _user.Id;
         var name     = _user.Name;
         var email    = _user.Email;
         var userName = _user.UserName;
         var password = _user.Password;
         var salt     = _user.Salt;
         var role     = _user.Role;
         
         // Assert
         Assert.AreEqual(99,                                             id);
         Assert.AreEqual("Erika Mustermann",                             name);
         Assert.AreEqual("e.mustermann@t-online.de",                     email);
         Assert.AreEqual("ErikaM",                                       userName);
         Assert.AreEqual(hashed,                                         password);
         Assert.AreEqual("iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=", salt);
         Assert.AreEqual(Role.User,                                     _user.Role);
      }
      [TestMethod]
      public void SetterUt() {
         // Arrange
         var password = AppSecurity.HashPbkdf2("Noch_Geh1mer", "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=");
         // Act 
         _user.Name     = "Erika Müller";
         _user.Email    = "erika.mueller@gmail.com";
         _user.Password = password;
         _user.Role = Role.Seller;
         // Assert
         Assert.AreEqual(99,                                             _user.Id);
         Assert.AreEqual("Erika Müller",                                 _user.Name);
         Assert.AreEqual("erika.mueller@gmail.com",                      _user.Email);
         Assert.AreEqual("ErikaM",                                       _user.UserName);
         Assert.AreEqual(password,                                       _user.Password);
         Assert.AreEqual("iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=", _user.Salt);
         Assert.AreEqual(Role.Seller,                                    _user.Role);
      }
      [TestMethod]
      public void AddAdressUt() {
         // Arrange
         var streetNr = "Rote Reihe 23";
         var zipCity  = "30827 Garbsen";
         // Act
         _user.AddAddress(streetNr, zipCity);
         // Assert
         Assert.AreEqual(streetNr, _user.Address?.StreetNr);
         Assert.AreEqual(zipCity,  _user.Address?.ZipCity);
      }
      
   }
}