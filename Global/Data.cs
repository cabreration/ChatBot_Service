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

        private static bool contieneMetodo(Procedimiento procedimiento) {

            foreach (Procedimiento proc in Data.procedimientos) {
                if (proc.identificador.Equals(procedimiento.identificador))
                {
                    if (proc.parametros.Count == procedimiento.parametros.Count) {
                        bool flag = true;
                        for (int i = 0; i < proc.parametros.Count; i++)
                        {
                            flag = flag && (proc.parametros[i].tipo.Equals(procedimiento.parametros[i].tipo));
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

        public static ParseTreeNode buscarProcedimiento(string identificador, ArrayList parametros) {

            ParseTreeNode acciones = null;
            foreach (Procedimiento met in Data.procedimientos) {
                if (met.identificador.Equals(identificador)) {
                    if (met.parametros.Count == parametros.Count)
                    {
                        if (validarParametros(parametros, met.parametros))
                            acciones = met.root;
                    }
                }
            }

            if (acciones == null)
                throw new Exception("No existe tal metodo");
            return acciones;
        }

        public static bool validarParametros(ArrayList valores, List<Simbolo> parametros) {
            bool flag = true;
            for (int i = 0; i < valores.Count; i++)
            {
                switch (parametros[i].tipo) {

                    case "Int":
                        flag = flag && (valores[i] is int); 
                        break;

                    case "String":
                        flag = flag && (valores[i] is string);
                        break;

                    case "Char":
                        flag = flag && (valores[i] is char);
                        break;

                    case "Double":
                        flag = flag && (valores[i] is double);
                        break;

                    case "Bool":
                        flag = flag && (valores[i] is bool);
                        break;
                }
            }
            return flag;
        }
    }
}
