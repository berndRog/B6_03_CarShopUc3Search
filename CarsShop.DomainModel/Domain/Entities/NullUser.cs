namespace CarShop.Domain.Entities {

   public class NullUser: User {
      public NullUser():base() {
         Id = -1;
      }
   }
}