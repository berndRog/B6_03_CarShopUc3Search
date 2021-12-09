using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Net.WebSockets;

using CarShop.Domain;
using CarShop.Domain.Entities;
using CarShop.Domain.Enums;

namespace CarShop.Presentation {
   
   internal class CDialog: IDialog {

      private readonly IAppUc1User     _appUCUser;


      // Dependency Injection via Ctor
      public CDialog(
         IAppUc1User     appUCUser
         
      ) {
         _appUCUser     = appUCUser;
      }

      public void ShowDialog() {
         RunUser();
      }

      private User? RunUser() {
         
       
         
         // Create User
         var name     = "Erika Mustermann";
         var email    = "e.mustermann@ostfalia.de";
         var userName = "ErikaM2";
         var password = "Geh1m_geh1m_";
         
         Result<User> resultCreate = _appUCUser.CreateUser(name, email, userName, password, Role.User);

         User? user = null;
         switch (resultCreate) {
            case Success<User>:
               user = resultCreate.Data;
               break;
            case Error<User>:
               Console.WriteLine("Error CreateUser failed");
               user = null;
               break;
         }
         if(user == null) {
            Console.WriteLine("*** Error user == null, S T O P");
            return null;
         }
           
         // Register User
         Result<User> resultRegister = _appUCUser.RegisterUser(user);
         if (resultRegister is Success<User>) {
            user = resultRegister.Data;
         } 
         else if (resultRegister is Error<User>) {
            Console.WriteLine(resultRegister.Message);
            user = null;
         }  
         return user;
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