namespace CarShop.Domain.Entities {
   
   public record Address:  IEntity {

      public int    Id       { get; set; } = 0;
      public string StreetNr { get; set; } = string.Empty;
      public string ZipCity  { get; set; } = string.Empty;      
  
      public Address(){}

      public Address Init(int id, string streetNr, string zipCity) {
         Id       = id;
         StreetNr = streetNr;
         ZipCity  = zipCity;
         return this;
      }

      public bool IsEqual(Address address) 
         => Id       == address.Id &&
            StreetNr == address.StreetNr &&
            ZipCity  == address.ZipCity;
   }
}