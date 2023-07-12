using FreeSql;
using FreeSql.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Wombat.Infrastructure
{

    /// <summary>
    /// FreeSql 模块配置
    /// </summary>
    public static class FreeSqlCoreModule
    {
        /// <summary>
        /// 添加FreeSql服务配置。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="connectionStrings">连接字符串选项。</param>
        /// <param name="isUseAutoSyncStructure">是否使用自动同步结构，默认为false。</param>
        /// <param name="isUseLazyLoading">是否使用延迟加载，默认为false。</param>
        /// <param name="nameConvert">名称转换类型，默认为NameConvertType.None。</param>
        /// <param name="isUseNoneCommandParameter">是否使用非命令参数，默认为true。</param>
        /// <param name="isUseExitAutoDisposePool">是否使用退出时自动释放连接池，默认为true。</param>
        /// <param name="isUseMonitorCommand">是否使用命令监视器，默认为false。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddFreeSql(this IServiceCollection services, ConnectionStringsOptions connectionStrings,
            bool isUseAutoSyncStructure = false, bool isUseLazyLoading = false,NameConvertType nameConvert = NameConvertType.None, bool isUseNoneCommandParameter = true, bool isUseExitAutoDisposePool = true, bool isUseMonitorCommand = false)
        {

            var databaseType = connectionStrings.DefaultDatabaseType;
            var dataType = DataType.SqlServer;
            var connectionString = connectionStrings.DefaultSqlServer;

            if (databaseType == DefaultDatabaseType.SqlServer)
            {
                connectionString = connectionStrings.DefaultSqlServer;
                dataType = DataType.SqlServer;
            }
            if (databaseType == DefaultDatabaseType.MySql)
            {
                connectionString = connectionStrings.DefaultMySql;
                dataType = DataType.MySql;
            }
            if (databaseType == DefaultDatabaseType.PostgreSql)
            {
                connectionString = connectionStrings.DefaultPostgreSql;
                dataType = DataType.PostgreSQL;
            }
            if (databaseType == DefaultDatabaseType.Sqlite)
            {
                connectionString = connectionStrings.DefaultSqlite;
                dataType = DataType.Sqlite;

            }
            var freeSql = CreateFreeSql(connectionString, dataType,isUseAutoSyncStructure,isUseLazyLoading,nameConvert,isUseNoneCommandParameter,isUseExitAutoDisposePool,isUseMonitorCommand);

            services.AddSingleton(freeSql);

            //services.AddScoped<UnitOfWorkManager>();

            //var ttt = services.AddFreeRepository(null, GlobalAssemblies.GetAssemblyLis(w =>
            //{
            //    var name = w.GetName().Name;
            //    return name != null && name.StartsWith(assemblyFilter);
            //}).ToArray());

            return services;
        }


        /// <summary>
        /// 添加FreeSql服务配置。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="connectionStrings">连接字符串选项。</param>
        /// <param name="isUseAutoSyncStructure">是否使用自动同步结构，默认为false。</param>
        /// <param name="isUseLazyLoading">是否使用延迟加载，默认为false。</param>
        /// <param name="nameConvert">名称转换类型，默认为NameConvertType.None。</param>
        /// <param name="isUseNoneCommandParameter">是否使用非命令参数，默认为true。</param>
        /// <param name="isUseExitAutoDisposePool">是否使用退出时自动释放连接池，默认为true。</param>
        /// <param name="isUseMonitorCommand">是否使用命令监视器，默认为false。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddFreeSql(this IServiceCollection services, IConfiguration configuration, bool isUseAutoSyncStructure = false,
            bool isUseLazyLoading = false, NameConvertType nameConvert = NameConvertType.None, bool isUseNoneCommandParameter = true, bool isUseExitAutoDisposePool = true, bool isUseMonitorCommand = false)
        {
            return AddFreeSql(services, configuration.GetSection("ConnectionStrings").Get<ConnectionStringsOptions>(),isUseAutoSyncStructure,isUseLazyLoading,nameConvert,isUseNoneCommandParameter
               ,isUseExitAutoDisposePool,isUseMonitorCommand);
        }

        /// <summary>
        /// 创建 free sql 对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static IFreeSql CreateFreeSql(string connectionString, DataType dataType, bool isUseAutoSyncStructure = false, bool isUseLazyLoading = false,
           NameConvertType nameConvert = NameConvertType.None, bool isUseNoneCommandParameter = true, bool isUseExitAutoDisposePool = true, bool isUseMonitorCommand = false)
        {
            var freeSqlBuilder = new FreeSqlBuilder()
                .UseConnectionString(dataType, connectionString)
                .UseAutoSyncStructure(isUseAutoSyncStructure)//自动迁移实体的结构到数据库
                .UseLazyLoading(isUseLazyLoading)
                .UseNameConvert(nameConvert)
                .UseNoneCommandParameter(isUseNoneCommandParameter)
                .UseExitAutoDisposePool(isUseExitAutoDisposePool);

            if (isUseMonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => Console.Write(cmd.CommandText));
            }

            var freeSql = freeSqlBuilder.Build();

            // sql执行后
            freeSql.Aop.CurdAfter += (s, curdAfter) =>
            {
                if (curdAfter.ElapsedMilliseconds > 1000)
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append($"\r\n====[Sql Start 耗时: {curdAfter.ElapsedMilliseconds} ms]=========");
                    stringBuilder.Append($"\r\n{curdAfter.Sql}");
                    stringBuilder.Append($"\r\n====[Sql End 线程Id:{Environment.CurrentManagedThreadId}]=========");
                    Console.WriteLine(stringBuilder);
                    Log.Warning(stringBuilder.ToString());
                }
            };


            // 审计属性值
            //freeSql.Aop.AuditValue += (s, auditInfo) =>
            //{
            //    switch (auditInfo.AuditValueType)
            //    {
            //        case AuditValueType.Insert:
            //            {
            //                if (auditInfo.Property.Name == nameof(BaseModel.CreateTime) ||
            //                    auditInfo.Property.Name == nameof(BaseModel.UpdateTime))
            //                {
            //                    auditInfo.Value = DateTime.Now;
            //                }

            //                break;
            //            }
            //        case AuditValueType.Update:
            //        case AuditValueType.InsertOrUpdate:
            //            {
            //                if (auditInfo.Property.Name == nameof(BaseModel.UpdateTime))
            //                {
            //                    auditInfo.Value = DateTime.Now;
            //                }

            //                break;
            //            }
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }
            //};

            return freeSql;
        }

        /// <summary>
        /// 批量注入 Repository，可以参考代码自行调整
        /// </summary>
        /// <param name="services"></param>
        /// <param name="globalDataFilter"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeRepository(this IServiceCollection services, Action<FluentDataFilter> globalDataFilter = null, params Assembly[] assemblies)
        {
            if (globalDataFilter != null)
            {
                //DataFilterUtil._globalDataFilter = globalDataFilter;
                //如果看到了这里的代码，想自己调整，但因为 _globalDataFilter 是内部属性，无法修改？
                //请考虑改用 fsql.GlobalFilter.Apply
            }

            services.AddScoped(typeof(IBaseRepository<>), typeof(GuidRepository<>));
            services.AddScoped(typeof(BaseRepository<>), typeof(GuidRepository<>));

            services.AddScoped(typeof(IBaseRepository<,>), typeof(DefaultRepository<,>));
            services.AddScoped(typeof(BaseRepository<,>), typeof(DefaultRepository<,>));

            if (assemblies?.Any() == true)
                foreach (var asse in assemblies)
                    foreach (var repo in asse.GetTypes().Where(a => a.IsAbstract == false && typeof(IBaseRepository).IsAssignableFrom(a)))
                    {
                        services.AddScoped(repo);
                    }

            return services;
        }







    }

}