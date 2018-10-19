using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Tabla_De_Simbolos;
using Irony.Parsing;

namespace ChatBot_Service.Global
{
    public class Procedimiento
    {
        public string tipo;
        public string identificador;
        public List<Simbolo> parametros;
        public ParseTreeNode root;

        public Procedimiento(string tipo, string identificador, ParseTreeNode raiz)
        {
            this.tipo = tipo;
            this.identificador = identificador;
            this.parametros = new List<Simbolo>();
            this.root = raiz;
        }

        public Procedimiento(string tipo, string identificador, ParseTreeNode root, List<Simbolo> parametros)
        {
            this.tipo = tipo;
            this.identificador = identificador;
            this.root = root;
            this.parametros = parametros;
        }
    }
}
