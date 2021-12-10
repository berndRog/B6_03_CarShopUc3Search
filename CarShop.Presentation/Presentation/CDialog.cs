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

      private readonly ILogger<CDialog> _log;

      // Dependency Injection via Ctor
      public CDialog(
         ILoggerFactory  loggerFactory
      ) {
         _log            = loggerFactory.CreateLogger<CDialog>();
         _log.LogInformation($"===========================================");

         _log.LogInformation($"Ctor() {GetHashCode()}");
       }

      public void ShowDialog() {
        
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