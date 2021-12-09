using CarShop.Di;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarShop.Start {

   class CompositionRoot {

      private static IHost?                   host; 
      private static ILogger<CompositionRoot> Log {  get; set; }

      public CompositionRoot() {
         
         IHostBuilder hostBuilder = 
            Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(hostConfig => {
                  hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                  hostConfig.AddJsonFile("launchSettings.json", optional: false);
                  hostConfig.Build();
                })
              // Configure AppConfiguration  
             .ConfigureAppConfiguration(appConfig => {
                 appConfig.SetBasePath(Directory.GetCurrentDirectory());
                 appConfig.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
                 appConfig.AddJsonFile($"appSettings.development.json", optional: true);
                 appConfig.Build();
              })
              // Configure Logging
             .ConfigureLogging((context, logging) => {
                 logging.ClearProviders();
                 logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                 logging.AddDebug();
                 logging.AddSimpleConsole();
              })
              // Configure services
             .ConfigureServices((context, services) => {
                 services.AddOptions();
                 services.AddLogging(builder => builder.AddDebug());
                 services.AddPresentation();
                 services.AddApplication();
                 services.AddPersistence(context.Configuration);
              })
             .UseConsoleLifetime();

              host = hostBuilder.Build();

              
              // Service Locator Anti Pattern
              Log = host.Services.GetRequiredService<ILogger<CompositionRoot>>();
              Log.LogTrace("Host ready to use"); 
         }

      
      static void Main() {
         new CompositionRoot();
         var dialog = host?.Services.GetService<IDialog>();
         dialog?.ShowDialog();
         Console.ReadKey();
      }
   }
}
