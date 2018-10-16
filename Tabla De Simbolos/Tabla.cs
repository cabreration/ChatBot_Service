using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Service.Tabla_De_Simbolos
{
    public class Tabla
    {
        public Tabla padre;
        public Hashtable elementos;

        public Tabla(Tabla padre) {
            this.padre = padre;
            this.elementos = new Hashtable();
        }

        public Tabla() {
            padre = null;
            elementos = new Hashtable();
        }

        public bool contiene(string identificador) {
            return this.elementos.Contains(identificador);
        }

        public void insertarSinValor(Simbolo simbolo) {
            if (contiene(simbolo.identificador))
                throw new Exception("La variable ha sido declarada en el mismo ambito anteriormente");

            this.elementos.Add(simbolo.identificador, simbolo);
        }

        public void insertarConValor(Simbolo simbolo) {
            if (contiene(simbolo.identificador))
                throw new Exception("La variable ha sido declarada en el mismo ambito anteriormente");

            switch (simbolo.tipo)
            {
                case "Int":
                    if (simbolo.valor is int)
                        elementos.Add(simbolo.identificador, simbolo);
                    else throw new Exception("el valor asignado a la variable " + simbolo.identificador
                        + " no es de tipo Int");
                    break;

                case "Double":
                    if (simbolo.valor is double)
                        elementos.Add(simbolo.identificador, simbolo);
                    else throw new Exception("el valor asignado a la variable " + simbolo.identificador
                        + " no es de tipo Double");
                    break;

                case "String":
                    if (simbolo.valor is string)
                        elementos.Add(simbolo.identificador, simbolo);
                    else throw new Exception("el valor asignado a la variable " + simbolo.identificador
                        + " no es de tipo String");
                    break;

                case "Bool":
                    if (simbolo.valor is bool)
                        elementos.Add(simbolo.identificador, simbolo);
                    else throw new Exception("el valor asignado a la variable " + simbolo.identificador
                        + " no es de tipo Bool");
                    break;

                case "Char":
                    if (simbolo.valor is char)
                        elementos.Add(simbolo.identificador, simbolo);
                    else throw new Exception("el valor asignado a la variable " + simbolo.identificador
                        + " no es de tipo Char");
                    break;
            }
        }

        public void actualizarValor(string identificador, object valor) {
            if (!(contiene(identificador)))
                throw new Exception("La variable " + identificador + " no existe en el contexto actual");

            Simbolo aux = (Simbolo)elementos[identificador];

            switch (aux.tipo) {
                case "Int":
                    if (valor is int)
                        ((Simbolo)elementos[identificador]).valor = valor;
                    else throw new Exception("el valor que intenta darle a la variable " + identificador
                        + " no es de tipo Int");
                    break;

                case "Double":
                    if (valor is double)
                        ((Simbolo)elementos[identificador]).valor = valor;
                    else throw new Exception("el valor que intenta darle a la variable " + identificador
                        + " no es de tipo Double");
                    break;

                case "String":
                    if (valor is string)
                        ((Simbolo)elementos[identificador]).valor = valor;
                    else throw new Exception("el valor que intenta darle a la variable " + identificador
                        + " no es de tipo String");
                    break;

                case "Bool":
                    if (valor is bool)
                        ((Simbolo)elementos[identificador]).valor = valor;
                    else throw new Exception("el valor que intenta darle a la variable " + identificador
                        + " no es de tipo Bool");
                    break;

                case "Char":
                    if (valor is char)
                        ((Simbolo)elementos[identificador]).valor = valor;
                    else throw new Exception("el valor que intenta darle" +
                        " a la variable " + identificador
                        + " no es de tipo Char");
                    break;
            }
        }

        public object obtenerValor(string identificador) {
            if (!contiene(identificador))
                throw new Exception("La variable " + identificador + " no existe en el contexto actual");

            return ((Simbolo)elementos[identificador]).valor;
        }

        public void heredar() {
            foreach (DictionaryEntry simbolo in padre.elementos) {
                this.elementos.Add(simbolo.Key, simbolo.Value);
            }
        }

        public void escalarAmbito() {
            Tabla darth = this.padre;
            Tabla luke = this;
            while (darth != null)
            {
                foreach (DictionaryEntry item in luke.elementos)
                {
                    if (darth.contiene(((Simbolo)item.Value).identificador))
                        ((Simbolo)(darth.elementos[((Simbolo)item.Value).identificador])).valor = ((Simbolo)item.Value).valor;
                }
                luke = darth;
                darth = darth.padre;
            }
        }
    }
}
