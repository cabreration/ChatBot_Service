using ChatBot_Service.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Irony.Parsing;
using ChatBot_Service.Tabla_De_Simbolos;
using System.Collections;

namespace ChatBot_Service.Global
{
    public class Data
    {
        public static List<Procedimiento> procedimientos;
        public static List<ErrorC> errores;
        public static Procedimiento main;
        public static Tabla ambitoGlobal;
        public static ParseTreeNode raiz;
        public static List<string> impresiones;
        public static Hashtable usuarios;
        public static string usuarioActual;
        public static bool returner;
        public static bool breaker;

        private static bool contieneMetodo(Procedimiento procedimiento) {

            foreach (Procedimiento proc in Data.procedimientos) {
                if (proc.identificador.Equals(procedimiento.identificador))
                {
                    if (proc.parametros.Count == procedimiento.parametros.Count) {
                        bool flag = true;
                        for (int i = 0; i < proc.parametros.Count; i++)
                        {
                            flag = flag && validarParametros(proc.parametros[i], procedimiento.parametros[i]);
                        }
                        if (flag) return true;
                    }
                }
            }
            return false;
        }

        public static void insertarMetodo(Procedimiento procedimiento) {
            if (contieneMetodo(procedimiento))
                throw new Exception("El metodo " + procedimiento.identificador + " ya existe con la misma cantidad" +
                    " y tipos de parametros");

            Data.procedimientos.Add(procedimiento);
        }

        public static Procedimiento buscarProcedimiento(string identificador, List<Parametro> parametros) {

            Procedimiento acciones = null;
            if (parametros == null)
            {
                foreach (Procedimiento meth in Data.procedimientos)
                {
                    if (meth.identificador.Equals(identificador))
                        acciones = meth;
                }
            }
         
            foreach (Procedimiento met in Data.procedimientos) {
                if (met.identificador.Equals(identificador)) {
                    if (met.parametros.Count == parametros.Count)
                    {
                        if (validarParametros(parametros, met.parametros))
                            acciones = met;
                    }
                }
            }

            if (acciones == null)
                throw new Exception("No existe tal metodo");
            return acciones;
        }

        public static bool validarParametros(object arg1, object arg2)
        {
            if (arg1 is Simbolo)
            {
                if (arg2 is Simbolo)
                {
                    Simbolo uno = (Simbolo)arg1;
                    Simbolo dos = (Simbolo)arg2;
                    if (uno.tipo.Equals(dos.tipo))
                        return true;
                }
                else return false;
            }
            else if (arg1 is Arreglo)
            {
                if (arg2 is Arreglo)
                {
                    Arreglo one = (Arreglo)arg1;
                    Arreglo two = (Arreglo)arg2;
                    if (one.tipo.Equals(two.tipo))
                        return true;
                }
                else return false;
            }
            return false;
        }

        public static bool validarParametros(List<Parametro> valores, ArrayList parametros) {
            bool flag = true;
            for (int i = 0; i < valores.Count; i++)
            {
                if (parametros[i] is Simbolo)
                {
                    if (valores[i].tipo == 1)
                    {
                        flag = flag && validarTipo((Simbolo)parametros[i], valores[i]);
                    }
                    else
                        return false;
                }
                else if (parametros[i] is Arreglo)
                {
                    if (valores[i].tipo == 2)
                    {
                        try
                        {
                            object[] arre = (object[])valores[i].valor;
                            flag = flag && validarArreglo((Arreglo)parametros[i], arre);
                        }
                        catch (Exception e)
                        {
                            //guardar error semantico
                        }
                    }
                    else
                        return false;
                }              
            }
            return flag;
        }

        private static bool validarTipo(Simbolo sim, Parametro para)
        {
            switch (sim.tipo)
            {
                case "Int":
                    if (!(para.valor is int))
                        return false;
                    break;

                case "String":
                    if (!(para.valor is string))
                        return false;
                    break;

                case "Double":
                    if (!(para.valor is double))
                        return false;
                    break;

                case "Bool":
                    if (!(para.valor is bool))
                        return false;
                    break;

                case "Char":
                    if (!(para.valor is char))
                        return false;
                    break;
            }
            return true;
        }

        private static bool validarArreglo(Arreglo arr, object[] arreglo)
        {
            if (arreglo[0] == null)
                throw new Exception("El arreglo que intenta usar no tiene valores asignados");

            switch (arr.tipo)
            {
                case "Int":
                    if (!(arreglo[0] is int))
                        return false;
                    break;

                case "String":
                    if (!(arreglo[0] is string))
                        return false;
                    break;

                case "Double":
                    if (!(arreglo[0] is double))
                        return false;
                    break;

                case "Bool":
                    if (!(arreglo[0] is bool))
                        return false;
                    break;

                case "Char":
                    if (!(arreglo[0] is char))
                        return false;
                    break;
            }
            return true;
        }
    }
}
