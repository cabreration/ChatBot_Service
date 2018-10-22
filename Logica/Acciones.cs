using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Global;
using ChatBot_Service.Tabla_De_Simbolos;
using Irony.Parsing;

namespace ChatBot_Service.Logica
{
    public class Acciones
    {
        public void reconocer(ParseTreeNode raiz) {

            switch (raiz.Term.Name) {

                case "INICIO":
                    if (raiz.ChildNodes.Count == 2)
                    {
                        reconocer(raiz.ChildNodes[0]);
                        reconocer(raiz.ChildNodes[1]);
                    }
                    else
                        reconocer(raiz.ChildNodes[0]);
                    break;

                case "ENCABEZADO":
                    if (raiz.ChildNodes.Count == 1)
                        foreach (ParseTreeNode root in raiz.ChildNodes)
                            reconocer(root);
                    break;

                case "LISTA_ACCIONES":
                    if (raiz.ChildNodes.Count == 1)
                        foreach (ParseTreeNode root in raiz.ChildNodes)
                            reconocer(root);
                    break;

                case "ACCION":
                    if (raiz.ChildNodes.Count == 1)
                        reconocer(raiz.ChildNodes[0]);
                    break;

                case "DECLARACION":
                    guardarVariable(raiz, Data.ambitoGlobal);
                    break;

                case "METODO":
                    guardarProcedimiento(raiz);
                    break;

                case "PRINCIPAL":
                    try {
                        guardarPrincipal(raiz);
                    }
                    catch (Exception e) {
                        //guardar error semantico
                    }
                    break;

                case "ASIGNACION":
                    break;

                case "DECLARACION_ARREGLO":
                    break;

                case "ASIGNACION_POSICION":
                    break;
            }
        }

        public void guardarVariable(ParseTreeNode root, Tabla ambito) {
            if (root.ChildNodes.Count == 2) // solo declaracion
            {
                string tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                foreach (ParseTreeNode id in root.ChildNodes[0].ChildNodes) {
                    string name = id.FindTokenAndGetText();
                    try
                    {
                        ambito.insertarSinValor(new Simbolo(tipo, name));
                    }
                    catch (Exception e) {
                        //capturar error semantico
                    }
                }
            }
            else if (root.ChildNodes.Count == 4) // declaracion y asignacion
            {
                try
                {
                    string tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                    object valor = null; //capturar valor
                    foreach (ParseTreeNode id in root.ChildNodes[0].ChildNodes)
                    {
                        string name = id.FindTokenAndGetText();

                    }
                }
                catch (Exception e) {
                    //capturar error semantico
                }
            }
        }

        public void guardarPrincipal(ParseTreeNode root) {
            if (Data.main != null)
                throw new Exception("Ya existe un metodo main declarado");

            string nombre = "main";
            string tipo = null;
            if (root.ChildNodes.Count == 3) {
                if (root.ChildNodes[1].Term.Name.Equals("Void"))
                    Data.main = new Procedimiento(nombre, "Void", root.ChildNodes[2]);
                else {
                    tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                    Data.main = new Procedimiento(nombre, tipo, root.ChildNodes[2]);
                }
            }
            else if (root.ChildNodes.Count == 5) {
                if (root.ChildNodes[1].Term.Name.Equals("Void"))
                {
                    Data.main = new Procedimiento(nombre, "Void", root.ChildNodes[4]);
                    Simbolo aux = new Simbolo(root.ChildNodes[4].ChildNodes[0].Term.Name,
                        root.ChildNodes[3].FindTokenAndGetText());
                    Data.main.parametros.Add(aux);
                }
                else {
                    tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                    Data.main = new Procedimiento(nombre, tipo, root.ChildNodes[4]);
                    Simbolo aux = new Simbolo(root.ChildNodes[4].ChildNodes[0].Term.Name,
                        root.ChildNodes[3].FindTokenAndGetText());
                    Data.main.parametros.Add(aux);
                }
            }
        }

        public void guardarProcedimiento(ParseTreeNode root) {
            string identificador = null;
            string tipo = null;
            ParseTreeNode sentencias = null;
            Procedimiento meth = null;

            identificador = root.ChildNodes[0].FindTokenAndGetText();
            if (root.ChildNodes[1].Term.Name.Equals("Void"))
                tipo = "Void";
            else tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;

            if (root.ChildNodes.Count == 4) { //incluye parametros       
                sentencias = root.ChildNodes[3];
                List<Simbolo> parametros = obtenerParametros(root.ChildNodes[2]);
                meth = new Procedimiento(tipo, identificador, sentencias, parametros);
            }
            else if (root.ChildNodes.Count == 3) { // no incluye parametros
                sentencias = root.ChildNodes[2];
                meth = new Procedimiento(tipo, identificador, sentencias);
            }

            try {
                Data.insertarMetodo(meth);
            }
            catch (Exception e) {
                //guardar error semantico
            }
        }

        public List<Simbolo> obtenerParametros(ParseTreeNode root) {
            List<Simbolo> retorno = new List<Simbolo>();

            foreach (ParseTreeNode hijo in root.ChildNodes) {
                string id = hijo.ChildNodes[0].FindTokenAndGetText();
                string tipo = hijo.ChildNodes[1].ChildNodes[0].Term.Name;
                Simbolo aux = new Simbolo(tipo, id);

                retorno.Add(aux);
            }
            return retorno;
        }
    }
}
