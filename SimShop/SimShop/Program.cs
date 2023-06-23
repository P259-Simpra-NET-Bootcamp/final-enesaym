using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimShop.Service;
using Serilog;

namespace SimShop.Service;

public class Program
{
    public static void Main(string[] args)
    {

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
        Log.Information("Application is starting...");

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseSerilog()
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
}
//public class Program
//{
//    public static void Main(string[] args)
//    {
//        CreateHostBuilder(args).Build().Run();
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//       Host.CreateDefaultBuilder(args)
//       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
//       .ConfigureContainer<ContainerBuilder>(builder =>
//       {
//           builder.RegisterModule(new AutoFacConfiguration());
//       })
//           .ConfigureWebHostDefaults(webBuilder =>
//           {
//               webBuilder.UseStartup<Startup>();
//           });
//}
