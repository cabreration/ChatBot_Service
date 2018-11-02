using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Global;
using ChatBot_Service.Logica;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChatBot_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            Data.errores = new List<ErrorC>();
            Data.procedimientos = null;
            Data.ambitoGlobal = null;
            Data.main = null;
            Data.raiz = null;
            Data.impresiones = null;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
