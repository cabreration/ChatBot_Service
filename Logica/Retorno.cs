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
        public int estado; // 0 = normal, 1 = break, 2 = return void

        public Retorno(string tipo, int estado) {
            this.tipo = tipo;
            this.estado = estado;
        }

        public Retorno(string tipo, int estado, object valor) {
            this.tipo = tipo;
            this.estado = estado;
            this.valor = valor;
        }
    }
}
