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
        public string Post([FromBody] string value)
        {
            string codigo = value;
            // aqui vamos a analizar el codigo
            // se va a regresar una cadena en formato json para ser parseada por el ide y sacar tanto los print
            // como los errores
            Gramatica grammar = new Gramatica();
            LanguageData lenguaje = new LanguageData(grammar);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(codigo);
            
            //ParseTreeNode root = arbol.Root;
            return "{ \"Errores\" : [ \"uno\", \"dos\" ], \"Impresiones\": [\"tres\", \"cuatro\"]}";
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
