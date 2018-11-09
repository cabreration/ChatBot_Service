using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Service.Tabla_De_Simbolos
{
    public class Simbolo
    {
        public string tipo;
        public object valor;
        public string identificador;

        public Simbolo()
        {
            this.tipo = null;
            this.valor = null;
            this.identificador = null;
        }

        public Simbolo(string tipo, string identificador)
        {
            this.tipo = tipo;
            this.identificador = identificador;

            switch (tipo)
            {
                case "Int":
                    valor = 0;
                    break;
                case "Double":
                    valor = 0.0;
                    break;
                case "String":
                    valor = "";
                    break;
                case "Char":
                    valor = '\u0000';
                    break;
                case "Bool":
                    valor = false;
                    break;
            }
        }

        public Simbolo(string tipo, string identificador, object valor)
        {
            this.tipo = tipo;
            this.identificador = identificador;
            this.valor = valor;
        }
    }
}
