using CarShop.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CarShop.Domain.Entities {

   public class User:  IEntity {

      #region properties
      public int     Id       { get; set; } 
      public string  Name     { get; set; }  = string.Empty;
      public string  Email    { get; set; }  = string.Empty;
      public string  UserName { get; set; }  = string.Empty;
      public string  Password { get; set; }  = string.Empty;
      public string  Salt     { get; set; }  = string.Empty;
      public Role    Role     { get; set; }  = Role.User;
      // Navigation-Property User --> Address [0..1]                         
      public Address? Address { get; set; } = null;
      // Navigation-Property User --> Cars [0..*]     
      public IList<Car> OfferedCars { get; set; } = new List<Car>();
      #endregion

      #region ctor
      public User() { }
      #endregion

      #region methods
      public User Init(int id, string name, string email, string userName,
                       string password, string salt, Role role) {
         if(id > 0) Id = id;
         Name     = name;
         Email    = email;
         UserName = userName;
         Password = password;
         Salt     = salt;
         Role     = role;
         return this;
      }

      public string AsString() {
         var role = Role switch {
                       Role.User     => "User",
                       Role.Customer => "Customer",
                       Role.Seller   => "Seller",
                       _             => "Unknown"
                    };
         var text= $"{Id} {Name} {Email} {UserName} {role}";
         if (Address != null) text += $"\n{Address.StreetNr} {Address.ZipCity}";
         return text;
      }

      public bool IsEqual(User? user) {
         if( user == null) return false;
         
         // Check properties
         var result = Id       == user.Id       &&
                      Name     == user.Name     &&
                      Email    == user.Email    &&
                      UserName == user.UserName &&
                      Password == user.Password &&
                      Salt     == user.Salt &&
                      Role     == user.Role;
         // some properties are not equal
         if(result == false) return false;
   
         // Check Addresses
         // both Addresses are nul -> no check necessary
         if     (Address == null && user.Address == null) result &= true;
         else if(Address != null && user.Address != null) result &= Address.IsEqual(user.Address);
         // one Address null and the other is not
         else if(Address != null && user.Address == null ||
                 Address == null && user.Address != null) result &= false;
         if(result == false) return false;

         // Check Cars
         result = result && CompareOfferedCars(user);
         return result;
      }

      private bool CompareOfferedCars(User user) {
         // if the number of cars isn't equal -> false
         if(user.OfferedCars.Count != OfferedCars.Count) return false;
         var found = 0;
         foreach(Car car in user.OfferedCars) { // offerdCars in parameter
            foreach(var c in OfferedCars) {     // are compared with offeredCars in this object
               if(c.IsEqual(car)) {
                  found++;
                  break;
               }
            }
         }     
         // all cars in list found? 
         return found == user.OfferedCars.Count;
      }
      #endregion

      #region methods Address
      public void AddAddress(string streetNr, string zipCity) {
         if (Address == null)
            Address = new Address().Init(0, streetNr, zipCity);
         else   
            throw new ApplicationException("Address already exists");
      }
      public void UpdateAddress(string streetNr, string zipCity) {
         if (Address == null) 
            throw new ApplicationException("Address doesn't exist");
         Address.StreetNr = streetNr;
         Address.ZipCity  = zipCity;
      }
      #endregion

      #region methods OfferedCars

      public Car? GetOfferedCarById(long carId) 
         => OfferedCars.FirstOrDefault(car => car.Id == carId);
      
      public IList<Car> GetAllOfferedCars() 
         => OfferedCars.ToList<Car>();

      public void AddOfferedCar(Car car) {
         car.User = this;
         OfferedCars.Add(car);
      }

      public (bool, string) UpdateOfferedCar(Car car, double price) { 
         var foundCar = OfferedCars.FirstOrDefault(c => c.Id == car.Id);
         if (foundCar == null)
            return (false, $"UpdateOfferedCars(): Car with id {car.Id} not found");
         if (Id != foundCar?.User?.Id)
            return (false, $"UpdateOfferedCars(): Car with id {car.Id} belongs to another customer");
         
         foundCar.Price = price;
         return (true, string.Empty);
      }
      
      public void RemoveOfferedCar(Car car) { 
         OfferedCars.Remove(car);
      }
      #endregion
   }
}