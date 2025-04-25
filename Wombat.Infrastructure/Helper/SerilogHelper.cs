using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wombat.Infrastructure
{
    /// <summary>
    /// 日志工具
    /// </summary>
    public static class SerilogHelper
    {
        static string LogFilePath(string LogEvent) =>
            $@"{AppDomain.CurrentDomain.BaseDirectory}/AppLogs/{DateTime.Now.Year}/{DateTime.Now.Month}_{DateTime.Now.Day}/{LogEvent}/.log";

        static object _instance = null;
        /// <summary>
        /// 启动
        /// </summary>
        public static void Build(params LogEventLevel[] levelsToFile)
        {
            if (_instance == null)
            {
                _instance = new object();
                var logger = new LoggerConfiguration()
                    .Enrich.With(new DateTimeNowEnricher())
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Override(nameof(Microsoft), LogEventLevel.Debug)
                    .WriteTo.Console();

                var logConfig = logger;

                foreach (var level in levelsToFile.Distinct())
                {
                    logConfig = logConfig.WriteTo.Logger(lg =>
                    {
                        lg.Filter.ByIncludingOnly(p => p.Level == level)
                          .WriteTo.File(LogFilePath(level.ToString()), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);
                    });
                }

                Serilog.Log.Logger = logConfig.CreateLogger();
            }
        }





        /// <summary>
        /// 日志对象
        /// </summary>
        public static ILogger Log => Serilog.Log.Logger;

        public static void LogException(Exception ex)
        {
            Log?.Error(ex.Message);
            Log?.Error(ex.Source);
            Log?.Error(ex.StackTrace);

        }
    }

    /// <summary>
    /// 时间处理
    /// </summary>
    class DateTimeNowEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "DateTimeNow", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }

}