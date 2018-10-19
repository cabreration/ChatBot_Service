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
                    break;

                case "PRINCIPAL":
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
    }
}
