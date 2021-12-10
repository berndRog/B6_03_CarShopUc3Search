using CarShop.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using System.Threading;

namespace CarShop.Domain.Entities {

   public class Car:  IEntity {

      #region properties
      public int    Id    { get; set; } = 0; 
      public string Make  { get; set; } = string.Empty;
      public string Model { get; set; } = string.Empty;
      public double Price { get; set; } = 0.0;
      
      // Inverse Navigation-Property one-to-many
      // Car --> User [0..1]
      public User  User   { get; set; } = null!;
      public int   UserId { get; set; } = 0;
      #endregion

      #region ctor
      public Car() {}
      #endregion 

      #region methods
      public Car Init(int id, string make, string model, double price, User user) {
         if(id > 0) Id = id;
         Make  = make;
         Model = model;
         Price = price;
         User = user;
         return this;
      } 
      
      public string AsString() => $"{Id,3} {Make,-15} {Model,-12} "     +
                                  $"{Price,8:f2} EUR ";

      
      public bool IsEqual(Car? car) {
        if( car == null) return false;
           var result =  Id    == car.Id    &&
                         Make  == car.Make  &&
                         Model == car.Model &&
                         Math.Abs(Price - car.Price) < 0.0001;

         if(User?.Id != car.UserId) result &= false;
         return result;
      }

      public override int GetHashCode() => this.GetHashCode();

      public bool FitsTo(CarTs carTs) 
         => Make  == carTs.Make      &&
            Model == carTs.Model     &&
            Price >= carTs.PriceFrom &&
            Price <= carTs.PriceTo;       
      #endregion
   }
}