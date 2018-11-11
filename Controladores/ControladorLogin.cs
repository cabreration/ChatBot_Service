using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Global;
using ChatBot_Service.Logica;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBot_Service.Controladores
{
    [Route("api/ControladorLogin")]
    [EnableCors("AllowAllOrigins")]
    public class ControladorLogin : Controller
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
        public bool Post([FromBody]LogIn value)
        {
            if (logIn(value.correo, value.password))
            {
                setCurrentUser(value.correo);
                Data.mensajesActuales = new List<Mensaje>();
                return true;
            }
            else
                return false;
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

        public class Response {
            public int estado { get; set; }

            public Response(int estado)
            {
                this.estado = estado;
            }
        }

        public bool logIn(string correo, string password)
        {
            if (Data.usuarios == null)
                return false;

            if (!(Data.usuarios.Contains(correo)))
                return false;

            Usuario user = ((Usuario)Data.usuarios[correo]);
            if (!(user.password.Equals(password)))
                return false;

            return true;
        }

        public void setCurrentUser(string llave)
        {
            Data.usuarioActual = ((Usuario)Data.usuarios[llave]).nombre + " "
                + ((Usuario)Data.usuarios[llave]).apellido;
        }
    }
}
