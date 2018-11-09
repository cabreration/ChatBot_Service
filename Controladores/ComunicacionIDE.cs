using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Logica;
using Irony.Parsing;
using Irony.Ast;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using ChatBot_Service.Global;
using ChatBot_Service.Tabla_De_Simbolos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBot_Service.Controladores
{
    [Route("api/[controller]")]
    public class ComunicacionIDE : Controller
    {
        string codigo = null;

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value3", "value4" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody] string value)
        {
            Data.errores = new List<ErrorC>();
            string codigo = value;
            // aqui vamos a analizar el codigo
            // se va a regresar una cadena en formato json para ser parseada por el ide y sacar tanto los print
            // como los errores
            Gramatica grammar = new Gramatica();
            LanguageData lenguaje = new LanguageData(grammar);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(codigo);
            Data.raiz = arbol.Root;

            if (arbol.ParserMessages.Count > 0)
            {
                guardarErrores(arbol);
            }

            if (!(Data.raiz == null))
            {
                Data.ambitoGlobal = new Tabla();
                Data.procedimientos = new List<Procedimiento>();
                Data.impresiones = new List<string>();
                Acciones verdugo = new Acciones();
                verdugo.reconocer(Data.raiz, true);
                Tabla aux = Data.ambitoGlobal;
                List<Procedimiento> metodos = Data.procedimientos;
                List<string> imp = Data.impresiones;
                Procedimiento main = Data.main;
            }

            string retorno = crearJson(Data.impresiones, Data.errores).ToString();
            return retorno;
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

        public void guardarErrores(ParseTree arbol)
        {
            for (int i = 0; i < arbol.ParserMessages.Count; i++)
            {
                String descripcion = arbol.ParserMessages.ElementAt(i).Message;
                int fila = arbol.ParserMessages.ElementAt(i).Location.Line;
                int columna = arbol.ParserMessages.ElementAt(i).Location.Column;
                String tipo = "";
                if (arbol.ParserMessages.ElementAt(i).Message.Contains("Invalid"))
                    tipo = "Lexico";
                else tipo = "Sintactico";
                ErrorC error = new ErrorC(fila, columna, "lexema", tipo, descripcion);

                Data.errores.Add(error);
            }
        }

        public JObject crearJson(List<string> impresiones, List<ErrorC> errores)
        {
            JObject retorno = new JObject();
            JArray impres;
            JArray errs;

            if (impresiones != null && impresiones.Count > 0)
            {
                impres = new JArray(impresiones);
                retorno.Add("Impresiones", impres);
            }

            if (errores != null && errores.Count > 0)
            {
                errs = new JArray(errores);
                retorno.Add("Errores", errs);
            }
            
            return retorno;
        }
    }
}
