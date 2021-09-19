using System;
using System.IO;
using FreeSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ForConsumption.Service
{
    public class Program
    {
        public static IFreeSql Fsql;

        public static void Main(string[] args)
        {
            StartDatabase();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }


        private static void StartDatabase()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Configs");
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            var litePath = Path.Combine(path, "source.SQLite");

            Fsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, $"Data Source={litePath}")
                .UseAutoSyncStructure(true) //�Զ�ͬ��ʵ��ṹ�����ݿ�
                .Build();
        }
    }
}