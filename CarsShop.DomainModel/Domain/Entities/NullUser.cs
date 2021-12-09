namespace CarShop.Domain.Entities {

   public record NullUser: User {
      public NullUser():base() {
         Id = -1;
      }
   }
}