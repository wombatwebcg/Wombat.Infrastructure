using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Wombat.Infrastructure;

namespace Wombat.Infrastructure
{

    /// <summary>
    /// 程序配置信息映射类 appsettings.json
    /// </summary>
    /// 
    public class SystemConnectionConfiguration
    {

        static ConnectionStringsOptions _connectionStrings;
        static InfluxDbOptions _influxDbSetting;

        public SystemConnectionConfiguration(IConfiguration configuration)
        {
            _connectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStringsOptions>();
            _influxDbSetting = configuration.GetSection(nameof(InfluxDbConfiguration)).Get<InfluxDbOptions>();

        }

        public static ConnectionStringsOptions ConnectionStrings
        {
            get { return _connectionStrings; }
        }




        public static InfluxDbOptions InfluxDbConfiguration
        {
            get { return _influxDbSetting; }
        }




    }





}