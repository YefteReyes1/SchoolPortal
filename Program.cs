using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        //Console.WriteLine("🚀 PROGRAM.CS is running..."); line used for debugging

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
