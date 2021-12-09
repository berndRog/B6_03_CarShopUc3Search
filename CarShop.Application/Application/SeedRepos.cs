using CarShop.Domain;
using System.Collections.Generic;


namespace CarShop.Application {
   class SeedRepos {

      public SeedRepos(IUnitOfWork unitOfWork, CSeed seed) {
         /*
         // register User1 + User2
         unitOfWork.RepositoryUser.Insert(seed.User1);
         unitOfWork.RepositoryUser.Insert(seed.User2);
         unitOfWork.RepositoryUser.Insert(seed.DefaultUser);
         unitOfWork.SaveChanges();

         // offer cars
         seed.User1.AddOfferedCar(seed.Car04);
         seed.User1.AddOfferedCar(seed.Car05);
         seed.User1.AddOfferedCar(seed.Car06);
         seed.User1.AddOfferedCar(seed.Car07);
         unitOfWork.RepositoryUser.Update(seed.User1);

         seed.User2.AddOfferedCar(seed.Car08);
         seed.User2.AddOfferedCar(seed.Car09);
         seed.User2.AddOfferedCar(seed.Car10);
         seed.User2.AddOfferedCar(seed.Car11);
         unitOfWork.RepositoryUser.Update(seed.User2);

         seed.DefaultUser.AddOfferedCar(seed.DefaultCar);
         */
      }
   }
}
