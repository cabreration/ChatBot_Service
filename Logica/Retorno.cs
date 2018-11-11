using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Service.Logica
{
    public class Retorno
    {
        public object valor;
        public string tipo;

        public Retorno(string tipo) {
            this.tipo = tipo;
            valor = null;
        }

        public Retorno(string tipo, object valor) {
            this.tipo = tipo;
            this.valor = valor;
        }

        public Retorno() {
            tipo = null;
            valor = null;
        }
    }
}
