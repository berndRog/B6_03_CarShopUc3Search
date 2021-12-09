using CarShop.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace CarShop.Application {

   public class CAppUc2OfferCar: IAppUc2OfferCar {
      
      #region fields
      private readonly IUnitOfWork       _unitOfWork;
      private readonly ILogger<CAppUc2OfferCar> _log;
      #endregion

      #region ctor
      public CAppUc2OfferCar(
         IServiceScopeFactory  serviceScopeFactory,
         ILoggerFactory        loggerFactory
      ) {
         _unitOfWork = serviceScopeFactory.CreateScope()
                                          .ServiceProvider.GetRequiredService<IUnitOfWork>();
         _log = loggerFactory.CreateLogger<CAppUc2OfferCar>();
         _log.LogDebug("Ctor()");
      }
      #endregion 
      
      #region methods
      public Result<Car> CreateOfferedCar(User user, string make, string model, double price) {
         _log.LogDebug($"{nameof(CreateOfferedCar)} {make} {model} {price}");

         // Checks ...   
         var car = new Car().Init(0, make, model, price, user);
         return new Success<Car>(car);
      }
      public Result<Car> AddOfferedCar(User user, Car car) {
         _log.LogInformation($"{nameof(AddOfferedCar)} {user.UserName} {car.Make}");
     
         // Update user (with car)
         car.Id = 0;
         user.AddOfferedCar(car);
         _unitOfWork.RepositoryUser
                    .Update(user);
         // Transfer to database
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         if(result) return new Success<Car>(car);
         else       return new Error<Car>($"Fehler in AddOfferedCar()");
      }

      public Result<Car> UpdateOfferedCar(User user, Car car) {
         _log.LogDebug($"{nameof(UpdateOfferedCar)} {user.UserName} {car.Make}");
         
         // Update car in users offeredCars
         user.UpdateOfferedCar(car, 1234);   
         
         // Update user and car 
         _unitOfWork.RepositoryUser
                    .Update(user);  
         
         // Transfer to database
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();

         if(result) return new Success<Car>(car); 
         else       return new Error<Car>($"Fehler in UpdateOfferedCar()");
      }

      public Result<Car> RemoveOfferedCar(User user, Car car) {
         _log.LogDebug($"{nameof(RemoveOfferedCar)} {user.UserName} {car.Make}");

         // Remove the car as offered cars
         user.RemoveOfferedCar(car);

         // Transfer to database
         var result = _unitOfWork.SaveChanges();
         _unitOfWork.Dispose();
         
         if(result) return new Success<Car>(car); 
         else       return new Error<Car>($"Fehler in RemoveCar()");
      }
      #endregion
   }
}