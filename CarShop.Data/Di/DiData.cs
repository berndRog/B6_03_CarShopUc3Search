using CarShop.Fakes;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.Di {
    public static class DiData {
        public static IServiceCollection AddData(this IServiceCollection services) {
            services.AddSingleton<IData, CDataFake>();
            return services;
        } 
    }
}
