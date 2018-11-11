using System;
using System.Collections;
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
    [Route("api/ControladorChat")]
    [EnableCors("AllowAllOrigins")]
    public class ControladorChat : Controller
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
        public Mensaje[] Post([FromBody]Mensaje value)
        {
            Mensaje mensa = new Mensaje();
            mensa.nombre = Data.usuarioActual;
            mensa.mensaje = value.mensaje;
            Data.mensajesActuales.Add(mensa);
            //desde aqui mando a llamar al Main con value como Parametro
            ArrayList parametros = new ArrayList();
            parametros.Add(value.mensaje);
            Acciones accion = new Acciones();
            try
            {
                Mensaje response = new Mensaje();
                response.nombre = "Bot";
                
                object respuesta = accion.ejecutarMain(parametros);
                if (!(respuesta is string))
                    response.mensaje = "Su mensaje no pudo ser analizado - Fatal error";
                else response.mensaje = (string)respuesta;

                Data.mensajesActuales.Add(response);
                Mensaje[] resp = new Mensaje[Data.mensajesActuales.Count];
                for (int i = 0; i < resp.Length; i++)
                {
                    resp[i] = (Mensaje)Data.mensajesActuales[i];
                }
                return resp;
            }
            catch (Exception e) {
                Mensaje[] resp = new Mensaje[Data.mensajesActuales.Count];
                for (int i = 0; i < resp.Length; i++)
                {
                    resp[i] = (Mensaje)Data.mensajesActuales[i];
                }
                return resp;
            }
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
    }
}
