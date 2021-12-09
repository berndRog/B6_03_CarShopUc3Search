// Value Objects
using CarShop.Domain.Entities;
namespace CarShop.Domain.ValueObjects {

   public record CarTs {

      public string Make      { get; set; } = "Alle";
      public string Model     { get; set; } = "Alle";
      public double PriceFrom { get; set; } 
      public double PriceTo   { get; set; }

      public CarTs() {}

      public CarTs Init(
         string make      = "", 
         string model     = "",
         double priceFrom =         0.0,
         double priceTo   = 1_000_000.0
        
      ) {
         Make      = make;
         Model     = model;
         PriceFrom = priceFrom;
         PriceTo   = priceTo;
         return this;
      }

      //public bool Filter(Car car) 
      //   => car.Make     == Make      &&
      //      car.Model    == Model     &&
      //      car.Price    >= PriceFrom &&
      //      car.Price    <= PriceTo  ;
   }
}
