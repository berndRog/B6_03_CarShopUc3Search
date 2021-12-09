using System.Collections.Generic;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;

namespace CarShop.Domain {

   public class CSeed {

      public User User01 { get; }  
      public User User02 { get; }  
      public User User03 { get; }  
      public User User04 { get; }  
      public User User05 { get; }  
      public User User06 { get; }

      public Car Car01 { get; }
      public Car Car02 { get; }
      public Car Car03 { get; }
      public Car Car04 { get; }
      public Car Car05 { get; }
      public Car Car06 { get; }
      public Car Car07 { get; }
      public Car Car08 { get; }
      public Car Car09 { get; }
      public Car Car10 { get; }
      public Car Car11 { get; }

      public CSeed() {
         var password = "geh1m_Geh1m";
         var salt     = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
         var hashed   = AppSecurity.HashPbkdf2(password, salt);


         User01 = new User().Init(0, "Achim Arndt",      "a.arndt@t-online.de",  "AchimA",   hashed, salt, Role.User);
         User02 = new User().Init(0, "Berhard Bauer",    "b.bauer@outlook.com",  "BBauer",   hashed, salt, Role.User);
         User03 = new User().Init(0, "Christine Conrad", "c.conrad@google.com",  "ChriCon",  hashed, salt, Role.User);
         User04 = new User().Init(0, "Dagmar Deppe",     "d.deppe@freenet.de",   "DagDep",   hashed, salt, Role.User);
         User05 = new User().Init(0, "Emil Erdmann",     "e.erdmann@icloud.com", "EmilErd",  hashed, salt, Role.User);
         User06 = new User().Init(0, "Fritz Fischer",    "f.fischer@google.com", "FFischer", hashed, salt, Role.User);

         var DefaultUser = new User().Init(0, "Erika Mustermann","e.mustermann@t-online.de", "ErikaM", hashed, salt, Role.User);

         Car01 = new Car().Init(0, "Volkswagen",    "Passat", 19500, DefaultUser);
         Car02 = new Car().Init(0, "Opel",          "Astra",  21000, DefaultUser);
         Car03 = new Car().Init(0, "Audi",          "A4",     16800, DefaultUser);
         Car04 = new Car().Init(0, "Volkswagen",    "Golf",   10000, DefaultUser);
         Car05 = new Car().Init(0, "Mercedes-Benz", "B200",   28000, DefaultUser);
         Car06 = new Car().Init(0, "Volkswagen",    "Golf",   12500, DefaultUser);
         Car07 = new Car().Init(0, "Volkswagen",    "Polo",   9130, DefaultUser);
         Car08 = new Car().Init(0, "Volkswagen",    "Golf",   7000, DefaultUser);
         Car09 = new Car().Init(0, "BMW",           "330",    28900, DefaultUser);
         Car10 = new Car().Init(0, "Volkswagen",    "Golf",   8900, DefaultUser);
         Car11 = new Car().Init(0, "Volkswagen",    "Golf",   11000, DefaultUser);

         DefaultUser.AddOfferedCar(Car01);
         DefaultUser.AddOfferedCar(Car02);
         DefaultUser.AddOfferedCar(Car03);
         DefaultUser.AddOfferedCar(Car04);
         DefaultUser.AddOfferedCar(Car05);
         DefaultUser.AddOfferedCar(Car06);
         DefaultUser.AddOfferedCar(Car07);
         DefaultUser.AddOfferedCar(Car08);
         DefaultUser.AddOfferedCar(Car09);
         DefaultUser.AddOfferedCar(Car10);
         DefaultUser.AddOfferedCar(Car11);

      }
   }
}