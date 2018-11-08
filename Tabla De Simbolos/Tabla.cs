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


        public object obtenerElemento(string identificador) {
            if (!(contiene(identificador)))
                throw new Exception("La variable a la que esta intentando acceder no existe en el ambito actual");

            object retorno = elementos[identificador];
            return retorno;
        }
        public bool contiene(string identificador) {
            return this.elementos.Contains(identificador);
        }

        public void insertarSinValor(Simbolo simbolo) {
            if (contiene(simbolo.identificador))
                throw new Exception("La variable ha sido declarada en el mismo ambito anteriormente");

            this.elementos.Add(simbolo.identificador, simbolo);
        }

        public void guardarArreglo(Arreglo array) {
            if (contiene(array.identificador))
                throw new Exception("La variable ha sido declarada en el mismo ambito anteriormente");

            this.elementos.Add(array.identificador, array);
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


            if (elementos[identificador] is Simbolo)
            {
                Simbolo aux = (Simbolo)elementos[identificador];

                switch (aux.tipo)
                {
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
            else if (elementos[identificador] is Arreglo)
            {
                if (!(valor is Arreglo))
                    throw new Exception("No se puede asignar una variable primitiva a un arreglo");

                switch (((Arreglo)elementos[identificador]).tipo)
                {
                    case "Int":
                        if (!((Arreglo)valor).tipo.Equals("Int"))
                            throw new Exception("Los tipos de dato de los arreglos no coinciden");
                        break;

                    case "Double":
                        if (!((Arreglo)valor).tipo.Equals("Double"))
                            throw new Exception("Los tipos de dato de los arreglos no coinciden");
                        break;

                    case "String":
                        if (!((Arreglo)valor).tipo.Equals("String"))
                            throw new Exception("Los tipos de dato de los arreglos no coinciden");
                        break;

                    case "Bool":
                        if (!((Arreglo)valor).tipo.Equals("Bool"))
                            throw new Exception("Los tipos de dato de los arreglos no coinciden");
                        break;

                    case "Char":
                        if (!((Arreglo)valor).tipo.Equals("Char"))
                            throw new Exception("Los tipos de dato de los arreglos no coinciden");
                        break;
                }

                ((Arreglo)elementos[identificador]).size = ((Arreglo)valor).size;
                ((Arreglo)elementos[identificador]).array = ((Arreglo)valor).array;
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

        public Arreglo obtenerArreglo(string identificador) {
            if (!(contiene(identificador)))
                throw new Exception("La variable " + identificador + " no existe en el contexto actual");

            object aux = elementos[identificador];
            if (!(aux is Arreglo))
                throw new Exception("La variable a la que intenta acceder no representa un arreglo");

            return (Arreglo)aux;
        }
    }
}
