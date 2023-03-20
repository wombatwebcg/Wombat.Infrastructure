using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Wombat.Infrastructure
{
   public class InfrastructureTemplete
    {

        /// <summary>
        /// 在DI容器中注册所有的服务类型 
        /// </summary>
        /// <param name="services"></param>
        public static void SimpleConfigureServices(IServiceCollection services, Assembly[] globalAssemblies = null, Func<IConfiguration,object> otherConfiguration = null)
        {

            LogHelper.Build();
            //register configuration
            IConfigurationBuilder cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange:true)                             
                ;
            IConfiguration configuration = cfgBuilder.Build();
            var appConfiguration = new SystemConnectionConfiguration(configuration);
            services.AddSingleton(configuration);
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
            services.AddFreeSql(SystemConnectionConfiguration.ConnectionStrings());

            services.AddOptions();


            //services.UseServiceProvider(SystemGlobal.AllAssemblies);

            services.UseServiceProvider(globalAssemblies);

        }

    }
}
