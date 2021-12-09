using CarShop.Domain.Entities;
namespace CarShop {
   public interface IAppUc2OfferCar {
      Result<Car>  CreateOfferedCar(User user, string make, string model, double price);
      Result<Car>  AddOfferedCar   (User user, Car car);
      Result<Car>  UpdateOfferedCar(User user, Car car);
      Result<Car>  RemoveOfferedCar(User user, Car car);
   }
}
