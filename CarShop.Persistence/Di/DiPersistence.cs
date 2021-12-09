using CarShop.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop {

   public static class DiPersistence {
      public static IServiceCollection AddPersistence(
         this IServiceCollection services,
         IConfiguration configuration
      ) {

         services.AddSingleton<IUnitOfWork,CUnitOfWork>();

         var connectionString = configuration.GetConnectionString("LocalDb");
         services.AddDbContext<CDbContext>(options =>
            options.UseSqlServer(connectionString));
         return services;
      } 
   }
}