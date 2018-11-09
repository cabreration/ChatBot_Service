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
using Newtonsoft.Json.Linq;

namespace ChatBot_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Data.errores = new List<ErrorC>();
            Data.procedimientos = null;
            Data.ambitoGlobal = null;
            Data.main = null;
            Data.raiz = null;
            Data.impresiones = null;
            Data.usuarioActual = null;
            cargarUsuarios();
            CreateWebHostBuilder(args).Build().Run();
        }

        private static void cargarUsuarios()
        {
            if (!(File.Exists("usuarios.json")))
            {
                Data.usuarios = new System.Collections.Hashtable();
                return;
            }

            Data.usuarios = new System.Collections.Hashtable();
            string json = File.ReadAllText("usuarios.json");
            JArray arreglo = JArray.Parse(json);
            Usuario usuario;
            foreach (JObject user in arreglo)
            {
                usuario = new Usuario();
                usuario.nombre = Convert.ToString(user.GetValue("nombre"));
                usuario.apellido = Convert.ToString(user.GetValue("apellido"));
                usuario.dpi = Convert.ToString(user.GetValue("dpi"));
                usuario.correo = Convert.ToString(user.GetValue("correo"));
                usuario.password = Convert.ToString(user.GetValue("password"));
                Data.usuarios.Add(usuario.correo, usuario);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
