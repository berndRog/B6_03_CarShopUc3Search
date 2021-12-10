using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Net.WebSockets;

using CarShop.Domain;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;

using Microsoft.Extensions.Logging;

namespace CarShop.Presentation {
   
   internal class CDialog: IDialog {

      private readonly IAppCore         _appCore;
      private readonly IAppUc1User      _appUc1User;
      private readonly IAppUc2OfferCar  _appUc2OfferCar;
      private readonly IUnitOfWork      _unitOfWork;
      private readonly ILogger<CDialog> _log;

      // Dependency Injection via Ctor
      public CDialog(
         IAppCore        appCore,
         IAppUc1User     appUc1User,
         IAppUc2OfferCar appUc2OfferCar,
         IUnitOfWork     unitOfWork,
         ILoggerFactory  loggerFactory
      ) {
         _appCore        = appCore;
         _appUc1User     = appUc1User;
         _appUc2OfferCar = appUc2OfferCar;
         _unitOfWork     = unitOfWork;
         _log            = loggerFactory.CreateLogger<CDialog>();

         _log.LogInformation($"Ctor() {GetHashCode()}");
       }

      public void ShowDialog() {
         RunUserWithOfferdCars();
      }

      private void RunUserWithOfferdCars() {
         
         // Register User
         var user = RegisterUserDialog();
         
         // Login
         var userName = "ErikaM2";
         var password = "Geh1m_geh1m_";
         var resultUser = _appCore.LoginUser(userName, password);
         var loggedInUser = EvaluateResult(resultUser);
         
         // Add 3 Offered  Cars
         var car1 = new Car().Init(0, "Volkswagen",    "Passat", 19500, loggedInUser!);
         var resultCar = _appUc2OfferCar.AddOfferedCar(loggedInUser!, car1);
         var car1Db = EvaluateResult(resultCar);

         var car2 = new Car().Init(0, "Opel",          "Astra",  21000, loggedInUser!);
         resultCar = _appUc2OfferCar.AddOfferedCar(loggedInUser!, car2);       
         var car2Db = EvaluateResult(resultCar);

         var car3 = new Car().Init(0, "Audi",          "A4",     16800, loggedInUser!);
         resultCar = _appUc2OfferCar.AddOfferedCar(loggedInUser!, car3);
         var car3Db = EvaluateResult(resultCar);

         // Logout
         resultUser = _appCore.LogoutUser();
         var loggedOutUser = EvaluateResult(resultUser);
         

         // Login
         resultUser = _appCore.LoginUser(userName, password);
         loggedInUser = EvaluateResult(resultUser);
         // Read user from database
         var userInDatabase = _unitOfWork.RepositoryUser.FindById(loggedInUser!.Id);


      }

      private User RegisterUserDialog() {
         // Create User
         var name = "Erika Mustermann";
         var email = "e.mustermann@ostfalia.de";
         var userName = "ErikaM2";
         var password = "Geh1m_geh1m_";

         Result<User> result = _appUc1User.CreateUser(name, email, userName, password, Role.User);
         User? user = EvaluateResult(result);

         // Register User
         result = _appUc1User.RegisterUser(user!);
         user = EvaluateResult(result);
         return user!;
      }

      private User? EvaluateResult(Result<User> result) {
         if      (result is Success<User>) return result.Data; 
         else if (result is Error<User>  ) {
            _log.LogError(result.Message); 
            Environment.Exit(-1); // Abort app
            return null;
         }
         else
            return null;
      }
      private Car? EvaluateResult(Result<Car> result) {
         if      (result is Success<Car>) return result.Data; 
         else if (result is Error<Car>  ) {
            _log.LogError(result.Message); 
            Environment.Exit(-1); // Abort app
            return null;
         }
         else
            return null;
      }

      /*
            private void RunApplication() {
               var count = _application.CountCars();
               Console.WriteLine($"\nTreffer{count,3}");

               var makes = _application.ReadMakes();
               Console.WriteLine($"\nMarkenliste");
               foreach (var make in makes) Console.WriteLine($"{make}");

               var selectedMake = "Volkswagen";
               var    models       = _application.ReadModels(selectedMake);
               Console.WriteLine($"\nModelliste");
               foreach (var model in models) Console.WriteLine($"{model}");

               var selectedModel = "Golf";
               var    cars          = _application.ReadCars(selectedMake, selectedModel);
               Console.WriteLine($"\nAusgewählt: {selectedMake} {selectedModel}");
               foreach (var car in cars) Console.WriteLine($"{car.AsString()}");
            }
      */
   }
}