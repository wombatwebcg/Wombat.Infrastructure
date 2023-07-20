using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Wombat.Infrastructure
{
    public class InfrastructureTemplete
    {

        /// <summary>
        /// 在DI容器中注册所有的服务类型 
        /// </summary>
        /// <param name="services"></param>
        public static void SimpleConfigureServices(IServiceCollection services, Func<IConfiguration,object> otherConfiguration = null, params string[] assemblyNames)
        {

            LogHelper.Build();
            ////register configuration
            //IConfigurationBuilder cfgBuilder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange:true)                             
            //    ;
            IConfiguration configuration = services.AddAppSettings();
            configuration.UseCustomConfigurationProvider();
            var  _connectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStringsOptions>();

            if (otherConfiguration != null)
            {
                services.AddSingleton(otherConfiguration.Invoke(configuration));
            }

            services.AddSingleton(new SnowflakeHelper(0, 0));

            //register logger
            services.AddLogging(build =>
            {
                object p = build.AddSerilog(logger: LogHelper.Log);
            });

            //注入freesql
            services.AddFreeSql(_connectionStrings,true,true);

            services.AddOptions();

            services.AddServicesPoxy(assemblyNames);

            services.BuildServiceProvider().UseCustomServiceProvider();

        }

        public static void SimpleConfigureServicesDebug(IServiceCollection services, Func<IConfiguration, object> otherConfiguration = null, params string[] assemblyNames)
        {

            LogHelper.Build();
            ////register configuration
            //IConfigurationBuilder cfgBuilder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange:true)                             
            //    ;
            IConfiguration configuration = services.AddAppSettings();
            configuration.UseCustomConfigurationProvider();
            var _connectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStringsOptions>();

            if (otherConfiguration != null)
            {
                services.AddSingleton(otherConfiguration.Invoke(configuration));
            }

            services.AddSingleton(new SnowflakeHelper(0, 0));

            //register logger
            services.AddLogging(build =>
            {
                object p = build.AddSerilog(logger: LogHelper.Log);
            });

            //注入freesql
            services.AddFreeSql(_connectionStrings, true, isUseMonitorCommand:true);

            services.AddOptions();

            services.AddServicesPoxy(assemblyNames);

            services.BuildServiceProvider().UseCustomServiceProvider();

        }

    }
}
