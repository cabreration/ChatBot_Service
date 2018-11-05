using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Service.Tabla_De_Simbolos
{
    public class Arreglo
    {
        public int size;
        public string identificador;
        public string tipo;
        public object[] array;


        public void cambiarTamanio(int size)
        {
            this.size = size;
            object[] mrBlueSky = new object[size];

            if (array[0] == null)
            {
                array = mrBlueSky;
                return;
            }

            if (array.Length > mrBlueSky.Length)
            {
                for (int i = 0; i < mrBlueSky.Length; i++)
                {
                    mrBlueSky[i] = array[i];
                }
            }
            else if (array.Length <= mrBlueSky.Length)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    mrBlueSky[i] = array[i];
                }
            }

            array = mrBlueSky;

            object valor = null;
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

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                    array[i] = valor;
            }

        }

        public Arreglo(int size, string identificador, string tipo) {
            this.size = size;
            this.identificador = identificador;
            this.tipo = tipo;

            array = new object[size];
            object valor = null;
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

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = valor;
            }
        }

        public void insertarEnIndice(int indice, object valor) {

            try
            {
                if ((validar(valor)))
                    this.array[indice] = valor;
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
        }

        public bool validar(object valor) {

            if (tipo.Equals("Int")) {
                if (!(valor is int))
                    throw new Exception("El valor que esta intentando insertar en el arreglo no es de tipo entero");
            }
            else if (tipo.Equals("Double")) {
                if (!(valor is double))
                    throw new Exception("El valor que esta intentado insertar en el arreglo no es de tipo double");
            }
            else if (tipo.Equals("String")) {
                if (!(valor is string))
                    throw new Exception("El valor que esta intentado insertar en el arreglo no es de tipo String");
            }
            else if (tipo.Equals("Char")) {
                if (!(valor is char))
                    throw new Exception("El valor que esta intentando insertar en el arreglo no es de tipo char");
            }
            else if (tipo.Equals("Boolean")) {
                if (!(valor is bool))
                    throw new Exception("El valor que esta intentando insertar en el arreglo no es de tipo boolean");
            }

            return true;
        }
    }
}
