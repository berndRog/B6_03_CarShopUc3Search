using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CarShop.Domain.Ut {
   [TestClass]
   public class SAppSecurityUt {
      [TestMethod]
      public void GenereateSaltUt() {
         // Arrange
         // Act
         var salt = AppSecurity.GenerateSalt(24);
         // Assert
         var byteArray = Convert.FromBase64String(salt);
         Assert.AreEqual(24, byteArray.Length);
      }
      
      [TestMethod]
      public void HashPbkdf2Ut() {
         // Arrange
         var salt     = "1yVma82d4d8DBpuiXQ7+WCs6A/1lqv7h";
         var password = "Alle meine Entchen schwimmen auf dem See";
         // Act
         var hashed = AppSecurity.HashPbkdf2(password, salt);
         // Assert
         var expected = @"LlYaT19NiKyKKx7Q007fhjQ60a3/AGw03cvRcV5wJIN1pNA/suFZm356EFalgdCOR/ZDsFBzjzHxoJhM4HUHNw==";
         Assert.AreEqual(expected, hashed);
      }
      [TestMethod]
      public void HashPasswordAndCompareUt() {
         // Arrange
         var password = "Alle meine Entchen schwimmen auf dem See";
         // Act
         var (salt, hashed) = AppSecurity.HashPassword(password);
         // Assert
         var result = AppSecurity.Compare(password, hashed, salt);
         Assert.IsTrue(result);
      }
   }
}