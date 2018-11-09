using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Global;
using ChatBot_Service.Logica;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBot_Service.Controladores
{
    [Route("api/ControladorRegistro")]
    [EnableCors("AllowAllOrigins")]
    public class ControladorRegistro : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public Respuesta Post([FromBody]Usuario user)
        {
            Usuario us = user;
            bool respuesta = insertarUsuario(user);

            if (respuesta)
                return new Respuesta(1);
            else return new Respuesta(0);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public class Respuesta {
            int estado;

            public Respuesta(int estado) {
                this.estado = estado;
            }
        }

        public bool insertarUsuario(Usuario user) {
            if (Data.usuarios == null)
                Data.usuarios = new Hashtable();

            if (Data.usuarios.Contains(user.correo))
            return false;

            Data.usuarios.Add(user.correo, user);

            string usuarios = stringifyUsers();

            System.IO.File.WriteAllText("usuarios.json", usuarios);

            return true;
        }

        public string stringifyUsers()
        {
            JArray usuarios = new JArray();
            //List<Usuario> usuarios2 = new List<Usuario>();
            
            foreach (DictionaryEntry obj in Data.usuarios) {
                Usuario us = (Usuario)(Data.usuarios[obj.Key]);
                JObject user = JObject.FromObject(us);
                usuarios.Add(user);
            }

            return usuarios.ToString();
        }
    }
}
