using System;
using System.Collections;
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
        public ArrayList parametros;
        public ParseTreeNode root;

        public Procedimiento(string tipo, string identificador, ParseTreeNode raiz)
        {
            this.tipo = tipo;
            this.identificador = identificador;
            this.parametros = new ArrayList();
            this.root = raiz;
        }

        public Procedimiento(string tipo, string identificador, ParseTreeNode root, ArrayList parametros)
        {
            this.tipo = tipo;
            this.identificador = identificador;
            this.root = root;
            this.parametros = parametros;
        }

        public void agregarParametro(Simbolo parametro) {
            parametros.Add(parametro);
        }
    }
}
