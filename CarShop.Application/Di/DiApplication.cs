using CarShop.Application;
using Microsoft.Extensions.DependencyInjection;
namespace CarShop.Di {
    public static class DiApplication {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddSingleton<IAppCore,        CAppCore>();
            services.AddSingleton<IAppUc1User,     CAppUc1User>();
            services.AddSingleton<IAppUc2OfferCar, CAppUc2OfferCar>();
            return services;
        } 
    }
}
