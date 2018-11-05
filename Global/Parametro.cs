using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Service.Global
{
    public class Parametro
    {
        public int tipo; // 1 es simbolo, 2 es arreglo
        public string dato;
        public object valor;

        public Parametro(int tipo, string dato, object valor)
        {
            this.tipo = tipo;
            this.dato = dato;
            this.valor = valor;
        }

        public Parametro(int tipo, object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }
    }
}
