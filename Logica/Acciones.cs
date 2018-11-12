using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Global;
using ChatBot_Service.Tabla_De_Simbolos;
using Irony.Parsing;

namespace ChatBot_Service.Logica
{
    public class Acciones
    {
        public void reconocer(ParseTreeNode raiz, bool actual) {

            switch (raiz.Term.Name) {

                case "INICIO": // esta listo
                    if (raiz.ChildNodes.Count == 2)
                    {
                        reconocer(raiz.ChildNodes[0], actual);
                        reconocer(raiz.ChildNodes[1], actual);
                    }
                    else
                        reconocer(raiz.ChildNodes[0], actual);
                    break;

                case "ENCABEZADO": //esta listo
                    foreach (ParseTreeNode root in raiz.ChildNodes)
                        reconocer(root, actual);
                    break;

                case "IMPORT": //esta listo
                    try
                    {
                        string archivo = limpiarCadena(raiz.ChildNodes[1].FindTokenAndGetText());
                        importar(archivo);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                    break;

                case "LISTA_ACCIONES": // esta listo
                    foreach (ParseTreeNode root in raiz.ChildNodes)
                        reconocer(root, actual);
                    break;

                case "ACCION": //esta listo
                    if (raiz.ChildNodes.Count == 1)
                        reconocer(raiz.ChildNodes[0], actual);
                    break;

                case "DECLARACION": // esta listo
                    guardarVariable(raiz, Data.ambitoGlobal);
                    break;

                case "PROCEDIMIENTO": // esta listo
                    guardarProcedimiento(raiz);
                    break;

                case "PRINCIPAL": // esta listo
                    try {
                        if (actual)
                            guardarPrincipal(raiz);
                    }
                    catch (Exception e) {
                        //guardar error semantico
                    }
                    break;

                case "ASIGNACION": // esta listo
                    if (raiz.ChildNodes[2].Term.Name.Equals("EXPRESION_LOGICA"))
                        actualizarValor(raiz, Data.ambitoGlobal);
                    else if (raiz.ChildNodes[2].Term.Name.Equals("LISTA_DATOS"))
                    {
                        try
                        {
                            string id = raiz.ChildNodes[0].FindTokenAndGetText();
                            Arreglo array = buscarArreglo(id, Data.ambitoGlobal);
                            guardarConValorArreglo(raiz.ChildNodes[2], array, Data.ambitoGlobal);
                        }
                        catch (Exception e)
                        {
                            //guardar error semantico
                        }
                    }
                    break;

                case "DECLARACION_ARREGLO": // esta listo
                    guardarArreglo(raiz, Data.ambitoGlobal);
                    break;

                case "ASIGNACION_POSICION": // esta listo
                    try
                    {
                        asignarAPosicion(raiz, Data.ambitoGlobal);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                    break;

                case "DINCREMENTO":
                    if (raiz.ChildNodes[1].Term.Name.Equals("++"))
                        incrementar(raiz.ChildNodes[0].FindTokenAndGetText(),
                            Data.ambitoGlobal);
                    else if (raiz.ChildNodes[1].Term.Name.Equals("--"))
                        decrementar(raiz.ChildNodes[0].FindTokenAndGetText(),
                            Data.ambitoGlobal);
                    break;

                case "FUNCION_PRINT": // listo
                    Imprimir(raiz.ChildNodes[1], Data.ambitoGlobal);
                    break;
            }
        }

        public void guardarVariable(ParseTreeNode root, Tabla ambito) {
            if (root.ChildNodes.Count == 2) // solo declaracion
            {
                string tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                if (root.ChildNodes[0].Term.Name.Equals("identificador"))
                {
                    string nombre = root.ChildNodes[0].FindTokenAndGetText();
                    ambito.insertarSinValor(new Simbolo(tipo, nombre));
                    return;
                }

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

                    if (root.ChildNodes[0].Term.Name.Equals("identificador"))
                    {
                        object valor2 = obtenerValor(root.ChildNodes[3], ambito); //capturar valor
                        if (valor2 is Simbolo)
                            valor2 = ((Simbolo)valor2).valor;

                        string nombre = root.ChildNodes[0].FindTokenAndGetText();
                        Simbolo normal = new Simbolo(tipo, nombre, valor2);
                        ambito.insertarConValor(normal);
                        return;
                    }

                    for (int i = 0; i < root.ChildNodes[0].ChildNodes.Count-1; i++)
                    {
                        ParseTreeNode id = root.ChildNodes[0].ChildNodes[i];
                        string name = id.FindTokenAndGetText();
                        Simbolo aux = new Simbolo(tipo, name);
                        ambito.insertarSinValor(aux);
                    }

                    object valor = obtenerValor(root.ChildNodes[3], ambito); //capturar valor
                    if (valor is Simbolo)
                        valor = ((Simbolo)valor).valor;

                    int ult = root.ChildNodes[0].ChildNodes.Count-1;
                    string last = root.ChildNodes[0].ChildNodes[ult].FindTokenAndGetText();
                    Simbolo temp = new Simbolo(tipo, last, valor);
                    ambito.insertarConValor(temp);
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
                    Data.main = new Procedimiento("Void", nombre, root.ChildNodes[2]);
                else {
                    tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                    Data.main = new Procedimiento(tipo, nombre, root.ChildNodes[2]);
                }
            }
            else if (root.ChildNodes.Count == 4) {
                if (root.ChildNodes[1].Term.Name.Equals("Void"))
                {
                    Data.main = new Procedimiento("Void", nombre, root.ChildNodes[3]);
                    
                }
                else {
                    tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                    Data.main = new Procedimiento(tipo, nombre, root.ChildNodes[3]);
                }
                ArrayList parametros = obtenerParametros(root.ChildNodes[2]);
                Data.main.parametros = parametros;
            }
        }

        public Simbolo parametroASimbolo(ParseTreeNode parametro)
        {
            string tipo = parametro.ChildNodes[1].ChildNodes[0].Term.Name;
            string id = parametro.ChildNodes[0].FindTokenAndGetText();
            Simbolo retorno = new Simbolo(tipo, id);
            return retorno;
        }

        public Arreglo parametroArreglo(ParseTreeNode parametro)
        {
            string tipo = parametro.ChildNodes[1].ChildNodes[0].Term.Name;
            string id = parametro.ChildNodes[0].FindTokenAndGetText();
            Arreglo retorno = new Arreglo(0, id, tipo);
            return retorno;
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
                ArrayList parametros = obtenerParametros(root.ChildNodes[2]);
                meth = new Procedimiento(tipo, identificador, sentencias, parametros);
            }
            else if (root.ChildNodes.Count == 3) { // no incluye parametros
                sentencias = root.ChildNodes[2];
                meth = new Procedimiento(tipo, identificador, sentencias);
            }

            try {
                Data.insertarMetodo(meth);
                List<Procedimiento> aux = Data.procedimientos;
            }
            catch (Exception e) {
                //guardar error semantico
            }
        }

        public ArrayList obtenerParametros(ParseTreeNode root) {
            ArrayList retorno = new ArrayList();

            foreach (ParseTreeNode hijo in root.ChildNodes) {
                if (hijo.ChildNodes.Count == 2)
                {
                    Simbolo sim = parametroASimbolo(hijo);
                    retorno.Add(sim);
                }
                else if (hijo.ChildNodes.Count == 4)
                {
                    Arreglo arr = parametroArreglo(hijo);
                    retorno.Add(arr);
                }
            }
            return retorno;
        }

        public void actualizarValor(ParseTreeNode root, Tabla ambito) { 
            try {
                string id = root.ChildNodes[0].FindTokenAndGetText();
                object valor = obtenerValor(root.ChildNodes[2], ambito);
                if (valor is Simbolo)
                    valor = ((Simbolo)valor).valor;

                ambito.actualizarValor(id, valor);
            }
            catch (Exception e) {
                //guardar error semantico
            }
        }

        public void guardarArreglo(ParseTreeNode root, Tabla ambito) {

            object tamanio = obtenerValor(root.ChildNodes[3], ambito);
            if (tamanio is Simbolo)
                tamanio = ((Simbolo)tamanio).valor;

            if (!(tamanio is int))
                throw new Exception("El argumento que intenta usar como dimension del arreglo no es de tipo entero");
            string identificador = root.ChildNodes[0].FindTokenAndGetText();
            string tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
            Arreglo array = new Arreglo((int)tamanio, identificador, tipo);

            try
            {
                if (root.ChildNodes.Count == 7)
                {
                    try
                    {
                        guardarConValorArreglo(root.ChildNodes[6].ChildNodes[0], array, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                }
                else if (root.ChildNodes.Count == 5)
                    ambito.guardarArreglo(array);

                ambito.guardarArreglo(array);                   
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            
        }

        public object obtenerValor(ParseTreeNode root, Tabla ambito) {
            if (root.Term.Name.Equals("EXPRESION_LOGICA"))
            {
                if (root.ChildNodes.Count == 3)
                    try
                    {
                        return logica3(root, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                else if (root.ChildNodes.Count == 2)
                    try
                    {
                        return logica2(root, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                else if (root.ChildNodes.Count == 1)
                    return obtenerValor(root.ChildNodes[0], ambito);
            }
            else if (root.Term.Name.Equals("EXPRESION_RELACIONAL")) {
                if (root.ChildNodes.Count == 3)
                    try
                    {
                        return relacional(root, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                else if (root.ChildNodes.Count == 1)
                    return obtenerValor(root.ChildNodes[0], ambito);
            }
            else if (root.Term.Name.Equals("EXPRESION")) {

                if (root.ChildNodes.Count == 3)
                {
                    try
                    {
                        return aritmetica3(root, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                }
                else if (root.ChildNodes.Count == 2)
                {
                    try
                    {
                        return aritmetica2(root, ambito);
                    }
                    catch (Exception e)
                    {
                        //guadar error semantico
                    }
                }
                else if (root.ChildNodes.Count == 1)
                {
                    return aritmetica1(root.ChildNodes[0], ambito);
                }
                else if (root.ChildNodes.Count == 4)
                {
                    try
                    {
                        return arregloConNodo(root, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                }
                else if (root.ChildNodes.Count == 5)
                {
                    try
                    {
                        return CompareTo2(root, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                }
            }
            return null;
        }

        public object arregloConNodo(ParseTreeNode spec, Tabla ambito) {
            string identificador = spec.ChildNodes[0].FindTokenAndGetText();

            object num = obtenerValor(spec.ChildNodes[2], ambito);
            if (num is Simbolo)
                num = ((Simbolo)num).valor;

            if (!(num is int))
                throw new Exception("El parametro especificado no representa un entero para acceder al arreglo");

            return accesoArreglo(identificador, (int)num, ambito);
        }

        public Arreglo buscarArreglo(string identificador, Tabla ambito)
        {
            Arreglo array = null;
            try
            {
                array = ambito.obtenerArreglo(identificador);
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            if (array == null)
                throw new Exception("No se pudo acceder al arreglo");
            return array;
        }

        public void guardarConValorArreglo(ParseTreeNode root, Arreglo array, Tabla ambito) {
            if (root.Term.Name.Equals("LISTA_DATOS"))
            {
                if (root.ChildNodes.Count != array.size)
                    throw new Exception("Las dimensiones del arreglo no coinciden con los valores declarados");

                object valor;
                int count = 0;
                foreach (ParseTreeNode val in root.ChildNodes) {
                    valor = obtenerValor(val, ambito);
                    if (valor is Simbolo)
                        valor = ((Simbolo)valor).valor;
                    array.insertarEnIndice(count, valor);
                    count++;
                } 
            }
            else
            {
                string identificador = root.FindTokenAndGetText();
                try {
                    Arreglo auxiliar = ambito.obtenerArreglo(identificador);
                    if (auxiliar.size != array.size)
                        throw new Exception("El tamanio de los arreglos que intenta igualar no coincide");

                    for (int i = 0; i < auxiliar.size; i++)
                    {
                        array.insertarEnIndice(i, auxiliar.array[i]);
                    }
                }
                catch (Exception e)
                {
                    //guardar error semantico
                }
            }
        }

        public void asignarAPosicion(ParseTreeNode raiz, Tabla ambito) {
            string identificador = raiz.ChildNodes[0].FindTokenAndGetText();
            object indice;
            object valor;

            try
            {
                Arreglo aux = ambito.obtenerArreglo(identificador);
                indice = obtenerValor(raiz.ChildNodes[2], ambito);
                if (indice is Simbolo)
                    indice = ((Simbolo)indice).valor;

                if (!(indice is int))
                    throw new Exception("El argumento de posicion no es de tipo entero");

                valor = obtenerValor(raiz.ChildNodes[5], ambito);
                if (valor is Simbolo)
                    valor = ((Simbolo)valor).valor;
                aux.insertarEnIndice((int)indice, valor);
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
        }

        public string limpiarCadena(string cadena)
        {
            return cadena.Substring(1, cadena.Length - 2);
        }

        public bool logica3(ParseTreeNode root, Tabla ambito) {          
            object valor1 = null;
            object valor2 = null;
            bool retorno = false;

            try {
                valor1 = obtenerValor(root.ChildNodes[0], ambito);
                valor2 = obtenerValor(root.ChildNodes[2], ambito);

                if (valor1 is Arreglo || valor2 is Arreglo)
                    throw new Exception("No se pueden realizar operaciones sobre arreglos completos");

                if (valor1 is Simbolo)
                    valor1 = ((Simbolo)valor1).valor;

                if (valor2 is Simbolo)
                    valor2 = ((Simbolo)valor2).valor;

                if (valor1 == null || valor2 == null) return false;

                if (root.ChildNodes[1].Term.Name.Equals("||"))
                    retorno = Calculadora.or(valor1, valor2);
                else if (root.ChildNodes[1].Term.Name.Equals("&&"))
                    retorno = Calculadora.and(valor1, valor2);
                else if (root.ChildNodes[1].Term.Name.Equals("|&"))
                    retorno = Calculadora.xor(valor1, valor2);
            }
            catch (Exception e) {
                //guardar error semantico
            }

            return retorno;
        }

        public bool logica2(ParseTreeNode root, Tabla ambito) {
            bool retorno = false;
            if (root.ChildNodes[0].Term.Name.Equals("!"))
            {
                try
                {
                    object valor = obtenerValor(root.ChildNodes[1], ambito);

                    if (valor is Arreglo)
                        throw new Exception("No se pueden realizar operaciones sobre arreglos como tal");

                    if (valor is Simbolo)
                        valor = ((Simbolo)valor).valor;

                    retorno = Calculadora.negacion(valor);
                }
                catch (Exception e)
                {
                    //guardar error semantico
                }
            }

            return retorno;
        }

        public bool relacional(ParseTreeNode root, Tabla ambito) {
            bool retorno = false;
            object arg1 = null;
            object arg2 = null;

            try
            {
                arg1 = obtenerValor(root.ChildNodes[0], ambito);
                arg2 = obtenerValor(root.ChildNodes[2], ambito);

                if(arg1 is Arreglo || arg2 is Arreglo)
                    throw new Exception("No se pueden realizar operaciones sobre arreglos completos");

                if (arg1 is Simbolo)
                    arg1 = ((Simbolo)arg1).valor;

                if (arg2 is Simbolo)
                    arg2 = ((Simbolo)arg2).valor;

                switch (root.ChildNodes[1].ChildNodes[0].Term.Name)
                {
                    case "<":
                        retorno = Calculadora.menorQue(arg1, arg2);
                        break;

                    case ">":
                        retorno = Calculadora.mayorQue(arg1, arg2);
                        break;

                    case "<=":
                        retorno = Calculadora.menorIgual(arg1, arg2);
                        break;

                    case ">=":
                        retorno = Calculadora.mayorIgual(arg1, arg2);
                        break;

                    case "!=":
                        retorno = Calculadora.noEquivalente(arg1, arg2);
                        break;

                    case "==":
                        retorno = Calculadora.equivalente(arg1, arg2);
                        break;
                }
            }
            catch (Exception e) {
                //guardar error semantico
            }         
            return retorno;
        }

        public object aritmetica3(ParseTreeNode root, Tabla ambito) {
            object retorno = null;
            object arg1 = null;
            object arg2 = null;
            try {

                arg1 = obtenerValor(root.ChildNodes[0], ambito);
                arg2 = obtenerValor(root.ChildNodes[2], ambito);

                if(arg1 is Arreglo || arg2 is Arreglo)
                    throw new Exception("No se pueden realizar operaciones sobre arreglos completos");

                if (arg1 is Simbolo)
                    arg1 = ((Simbolo)arg1).valor;

                if (arg2 is Simbolo)
                    arg2 = ((Simbolo)arg2).valor;

                switch (root.ChildNodes[1].Term.Name)
                {
                    case "+":
                        retorno = Calculadora.sumar(arg1, arg2);
                        break;

                    case "-":
                        retorno = Calculadora.restar(arg1, arg2);
                        break;

                    case "*":
                        retorno = Calculadora.multiplicar(arg1, arg2);
                        break;

                    case "/":
                        retorno = Calculadora.dividir(arg1, arg2);
                        break;

                    case "^":
                        retorno = Calculadora.elevar(arg1, arg2);
                        break;

                    case "%":
                        retorno = Calculadora.modular(arg1, arg2);
                        break;
                }

            }
            catch (Exception e) {
                //guardar error semantico
            }
            return retorno;
        }

        public object aritmetica1(ParseTreeNode root, Tabla ambito) {
            object retorno = null;

            try
            {
                switch (root.Term.Name)
                {
                    case "EXPRESION_LOGICA":
                        retorno = obtenerValor(root, ambito);
                        break;

                    case "identificador":
                        try
                        {
                            string id = root.FindTokenAndGetText();
                            object en = ambito.obtenerElemento(id);
                            if (en is Simbolo)
                                return (Simbolo)en;
                            else if (en is Arreglo)
                                return (Arreglo)en;
                        } catch (Exception e)
                        {
                            //guardar error semantico
                        }
                        break;

                    case "LLAMADA":
                        try
                        {
                            retorno = llamada(root, ambito);
                        }
                        catch (Exception e)
                        {
                            //guardar error semantico
                        }
                        break;

                    case "OBTENER_USUARIO":
                        if (Data.usuarioActual == null)
                            return "Javier Cabrera";
                        else return Data.usuarioActual;

                    case "verdadero":
                        return true;

                    case "falso":
                        return false;                     

                    case "cadena":
                        retorno = limpiarCadena(root.FindTokenAndGetText());
                        break;

                    case "caracter":
                        retorno = Convert.ToChar(limpiarCadena(root.FindTokenAndGetText()));
                        break;

                    case "numero":
                        try {
                            int entero = Convert.ToInt32(root.FindTokenAndGetText());
                            return entero;
                        }
                        catch {
                            double doble = Convert.ToDouble(root.FindTokenAndGetText());
                            return doble;
                        }
                }
            }
            catch (Exception e) {
                // guardar error semantico
            }
            return retorno;
        }

        public object aritmetica2(ParseTreeNode root, Tabla ambito) {
            object retorno = null;
            if (root.ChildNodes[0].Term.Name.Equals("-"))
            {
                try
                {
                    object arg = obtenerValor(root.ChildNodes[1], ambito);
                    if (arg is Arreglo)
                        throw new Exception("No se puede realizar operaciones sobre arreglos como tal");

                    if (arg is Simbolo)
                        arg = ((Simbolo)arg).valor;

                    return Calculadora.negativo(arg);
                }
                catch (Exception e)
                {
                    //guardar error semantico
                }
            }
            else if (root.ChildNodes[0].Term.Name.Equals("identificador"))
            {
                try
                {
                    return CompareTo(root, ambito);
                }
                catch (Exception e)
                {
                    //guardar error semantico
                }
            }
            return retorno;
        }

        public void incrementar(string identificador, Tabla ambito)
        {
            try
            {
                object antiguo = ambito.obtenerValor(identificador);
                object valor = Calculadora.incremento(antiguo);
                ambito.actualizarValor(identificador, valor);
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
        }

        public void decrementar(string identificador, Tabla ambito)
        {
            try
            {
                object antiguo = ambito.obtenerValor(identificador);
                object valor = Calculadora.decremento(antiguo);
                ambito.actualizarValor(identificador, valor);
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
        }

        public void importar(string cadena)
        {

            if (File.Exists(cadena))
            {
                string contenido = File.ReadAllText(cadena);
                Gramatica grammar = new Gramatica();
                LanguageData lenguaje = new LanguageData(grammar);
                Parser parser = new Parser(lenguaje);
                ParseTree arbol = parser.Parse(contenido);
                ParseTreeNode root = arbol.Root;

                //chequear si hay errores o no en el archivo

                reconocer(root, false);
            }

            throw new Exception("El archivo que desea importar no existe en el folder fuente");
        }

        public void Imprimir(ParseTreeNode expresion, Tabla ambito)
        {
            try
            {
                object obj = obtenerValor(expresion, ambito);
                if (obj is Simbolo)
                    obj = ((Simbolo)obj).valor;
                string print = Convert.ToString(obj);
                Data.impresiones.Add(print);
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
        }

        public bool CompareTo(ParseTreeNode variable, Tabla ambito)
        {
            bool retorno = false;
            try
            {
                string cadena = (string)ambito.obtenerValor(variable.ChildNodes[0].FindTokenAndGetText());
                object cont = obtenerValor(variable.ChildNodes[1].ChildNodes[1], ambito);

                if (cont is Simbolo)
                    cont = ((Simbolo)cont).valor;

                if (!(cont is string))
                    throw new Exception("El parametro de comparacion no es una cadena");

                string contenido = (string)cont;
                retorno = contenido.Contains(contenido);
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            return retorno;
        }

        public bool CompareTo2(ParseTreeNode datos, Tabla ambito)
        {
            bool retorno = false;     
            try
            {
                string identificador = datos.ChildNodes[0].FindTokenAndGetText();
                object indice = obtenerValor(datos.ChildNodes[2], ambito);

                if (indice is Simbolo)
                    indice = ((Simbolo)indice).valor;

                if (!(indice is int))
                    throw new Exception("El indice no es un entero");

                object cad = accesoArreglo(identificador, (int)indice, ambito);
                if (!(cad is string))
                    throw new Exception("El operando no es de tipo String, operacion invalida");

                string cadena = (string)cad;
                object cont = obtenerValor(datos.ChildNodes[4].ChildNodes[1], ambito);

                if (cont is Simbolo)
                    cont = ((Simbolo)cont).valor;
                if (!(cont is string))
                    throw new Exception("El parametro de comparacion no es de tipo String, operacion invalida");

                string contenido = (string)cont;
                retorno = cadena.Contains(contenido);
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            return retorno;
        }

        public object accesoArreglo(string identificador, int index, Tabla ambito)
        {
            object retorno = null;
            Arreglo array = null;
            try
            {
                array = ambito.obtenerArreglo(identificador);
                if (index > array.array.Length - 1)
                    throw new Exception("El indice del arreglo se encuentra fuera de los limites");
                retorno = array.array[index];
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            return retorno;
        }

        //falta probar
        public object llamada(ParseTreeNode llamada, Tabla ambito)
        {
            Procedimiento procedimiento = buscarProcedimiento(llamada, ambito);
            if (procedimiento == null)
                return null;

            Tabla scope = null;
            if (procedimiento.parametros.Count > 0 && llamada.ChildNodes.Count == 2)
            {
                try
                {
                    List<Parametro> pars = obtenerValoresDados(llamada.ChildNodes[1], ambito);
                    scope = construirTabla(procedimiento.parametros, pars, Data.ambitoGlobal);
                }
                catch (Exception e)
                {
                    // guardar error semantico
                    return null;
                }
            }
            else
            {
                scope = new Tabla(Data.ambitoGlobal);
                scope.heredar();
            }

            if (procedimiento.root.ChildNodes.Count == 0)
                return null;

            Retorno valor = ejecutarSentencias(procedimiento.root.ChildNodes[0], scope);
            if (procedimiento.tipo.Equals("Void") && valor != null)
                if (!(procedimiento.tipo.Equals(valor.tipo)))
                    throw new Exception("El tipo de retorno esperado no concuerda con el recibido");

            //scope.escalarAmbito();
            Data.returner = false;
            Data.breaker = false;
            return valor.valor;
        }

        public Tabla construirTabla(ArrayList parametros, List<Parametro> valores, Tabla padre)
        {
            Tabla retorno = new Tabla(padre);
            retorno.heredar();
            if (parametros.Count != valores.Count)
                throw new Exception("No ha incluido todos los parametros en la llamada al, metodo");

            for (int i = 0; i < parametros.Count; i++)
            {
                if (parametros[i] is Simbolo)
                {
                    ((Simbolo)parametros[i]).valor = valores[i].valor;
                    retorno.insertarConValor((Simbolo)parametros[i]);
                }
                else if (parametros[i] is Arreglo)
                {
                    ((Arreglo)parametros[i]).array = (object[])valores[i].valor;
                    retorno.guardarArreglo((Arreglo)parametros[i]);
                }              
            }
            return retorno;
        }

        public Procedimiento buscarProcedimiento(ParseTreeNode llamada, Tabla ambito)
        {
            Procedimiento retorno = null;
            if (llamada.ChildNodes.Count == 1)
            {
                try
                {
                    string id = llamada.ChildNodes[0].FindTokenAndGetText();
                    retorno = Data.buscarProcedimiento(id, null);
                }
                catch (Exception e)
                {
                    //guardar error semantico
                }
            }
            else if (llamada.ChildNodes.Count == 2)
            {
                try
                {
                    string id = llamada.ChildNodes[0].FindTokenAndGetText();
                    List<Parametro> parametros = obtenerValoresDados(llamada.ChildNodes[1], ambito);
                    retorno = Data.buscarProcedimiento(id, parametros);
                }
                catch (Exception e)
                {
                    //guardar error semantico
                }
            }

            return retorno;
        }

        public List<Parametro> obtenerValoresDados(ParseTreeNode parametros, Tabla ambito)
        {
            List<Parametro> retorno = new List<Parametro>();
            if (parametros.ChildNodes.Count == 1)
            {
                object valor = obtenerValor(parametros.ChildNodes[0], ambito);
                if (valor is Simbolo)
                    valor = ((Simbolo)valor).valor;

                Parametro par;
                if (valor is Arreglo)
                {
                    par = new Parametro(2, valor);
                }
                else par = new Parametro(1, valor);
                retorno.Add(par);
            }
            else
            {
                foreach (ParseTreeNode hijo in parametros.ChildNodes)
                {
                    object valor = obtenerValor(hijo, ambito);
                    if (valor is Simbolo)
                        valor = ((Simbolo)valor).valor;

                    Parametro par;
                    if (valor is Arreglo)
                    {
                        par = new Parametro(2, valor);
                    }
                    else par = new Parametro(1, valor);
                    retorno.Add(par);
                }
            }
            return retorno;
        }

        public Retorno ejecutarSentencias(ParseTreeNode sentencias, Tabla ambito) {
            Retorno retorno = new Retorno();
            foreach (ParseTreeNode hijo in sentencias.ChildNodes)
            {
                ParseTreeNode instruccion = hijo.ChildNodes[0];
                switch (instruccion.Term.Name)
                {
                    case "DECLARACION":
                    guardarVariable(instruccion, ambito);
                    break;

                    case "ASIGNACION":
                    if (instruccion.ChildNodes[2].Term.Name.Equals("EXPRESION_LOGICA"))
                        actualizarValor(instruccion, ambito);
                    else if (instruccion.ChildNodes[2].Term.Name.Equals("LISTA_DATOS"))
                    {
                        try
                        {
                            string id = instruccion.ChildNodes[0].FindTokenAndGetText();
                            Arreglo array = buscarArreglo(id, ambito);
                            guardarConValorArreglo(instruccion.ChildNodes[2], array, Data.ambitoGlobal);
                        }
                        catch (Exception e)
                        {
                                    //guardar error semantico
                        }
                    }
                    break;

                    case "DECLARACION_ARREGLO":
                        guardarArreglo(instruccion, ambito);
                        break;

                    case "ASIGNACION_POSICION":
                        try
                        {
                            asignarAPosicion(instruccion, ambito);
                        }
                        catch (Exception e)
                        {
                            //guardar error semantico
                        }
                        break;

                    case "IF":
                        Tabla tablaIf = new Tabla(ambito);
                        tablaIf.heredar();
                        retorno = If_Else(instruccion, tablaIf);
                        if (Data.returner)
                            return retorno;
                        break;

                    case "FOR":
                        Tabla tablaFor = new Tabla(ambito);
                        tablaFor.heredar();
                        retorno = For(instruccion, tablaFor);
                        if (Data.returner)
                            return retorno;
                        break;

                    case "SWITCH":
                        Tabla tablaSwitch = new Tabla(ambito);
                        tablaSwitch.heredar();
                        retorno = Switch(instruccion, tablaSwitch);
                        if (Data.returner)
                            return retorno;
                        break;

                    case "WHILE":
                        Tabla tablaWhile = new Tabla(ambito);
                        tablaWhile.heredar();
                        retorno = While(instruccion, tablaWhile);
                        if (Data.returner)
                            return retorno;
                        break;

                    case "DO_WHILE":
                        Tabla tablaDo = new Tabla(ambito);
                        tablaDo.heredar();
                        retorno = Do_While(instruccion, tablaDo);
                        if (Data.returner)
                            return retorno;
                        break;

                    case "FUNCION_PRINT":
                        Imprimir(instruccion.ChildNodes[1], ambito);
                        break;

                    case "DINCREMENTO":
                        if (instruccion.ChildNodes[1].Term.Name.Equals("++"))
                            incrementar(instruccion.ChildNodes[0].FindTokenAndGetText(),
                                ambito);
                        else if (instruccion.ChildNodes[1].Term.Name.Equals("--"))
                            decrementar(instruccion.ChildNodes[0].FindTokenAndGetText(),
                                ambito);
                        break;

                    case "LLAMADA":
                        try {
                            llamada(instruccion, ambito);
                        }
                        catch (Exception e)
                        {
                            //guardar error semantico
                        }
                        break;

                    case "RETORNO":
                        Data.returner = true;
                        ambito.escalarAmbito();
                        retorno = determinarRetorno(instruccion, ambito);
                        return retorno;

                    case "Break":
                        Data.breaker = true;
                        break;
                }
            }
            ambito.escalarAmbito();
            return retorno;
        }

        //falta probar
        public Retorno While(ParseTreeNode root, Tabla ambito)
        {
            Retorno retorno = null;
            try
            {
                if (root.ChildNodes.Count == 3)
                {
                    object cond = obtenerValor(root.ChildNodes[1], ambito);
                    if (cond is Simbolo)
                        cond = ((Simbolo)cond).valor;

                    if (!(cond is bool))
                        throw new Exception("La condicion especificada no es de tipo booleano");
                    
                    bool condicion = (bool)cond;
                    while (condicion)
                    {
                        retorno = ejecutarSentencias(root.ChildNodes[2], ambito);
                        condicion = (bool)obtenerValor(root.ChildNodes[1], ambito);
                        ambito.escalarAmbito();
                        Tabla padre = ambito.padre;
                        ambito = new Tabla(padre);
                        ambito.heredar();
                        condicion = (bool)obtenerValor(root.ChildNodes[1], ambito);
                        if (Data.returner)
                        {
                            ambito.escalarAmbito();
                            return retorno;
                        }
                        else if (Data.breaker) {
                            Data.breaker = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            return retorno;
        }

        //falta probar
        public Retorno Do_While(ParseTreeNode root, Tabla ambito)
        {
            Retorno retorno = null;
            try
            {
                if (root.ChildNodes.Count == 4)
                {
                    object cond = null;
                    bool condicion = false;
                    do
                    {
                        retorno = ejecutarSentencias(root.ChildNodes[1], ambito);
                        cond = obtenerValor(root.ChildNodes[3], ambito);

                        if (cond is Simbolo)
                            cond = ((Simbolo)cond).valor;

                        if (!(cond is bool))
                            throw new Exception("La condicion indicada no evalua a un resultado booleano");

                        condicion = (bool)cond;
                        ambito.escalarAmbito();
                        Tabla padre = ambito.padre;
                        ambito = new Tabla(padre);
                        ambito.heredar();
                        if (Data.returner)
                        {
                            ambito.escalarAmbito();
                            return retorno;
                        }
                        else if (Data.breaker)
                        {
                            Data.breaker = false;
                            break;
                        }
                    }
                    while (condicion);
                }
            }
            catch (Exception e)
            {
                //guardar errores semanticos
            }
            ambito.escalarAmbito();
            return retorno;
        }

        //falta probar
        public Retorno If_Else(ParseTreeNode root, Tabla ambito)
        {
            Retorno retorno = null;
            object condicion = obtenerValor(root.ChildNodes[1], ambito);

            if (condicion is Simbolo)
                condicion = ((Simbolo)condicion).valor;

            if (!(condicion is bool))
                throw new Exception("La condicion especificada en sentencia If no evalua a un valor booleano");
            bool cond = (bool)condicion;
            try
            {
                if (root.ChildNodes.Count == 3)
                {
                    if (root.ChildNodes[2].Term.Name.Equals("ELSE"))
                    {
                        if (!cond)
                        {
                            if (root.ChildNodes[2].ChildNodes.Count == 2)
                            {
                                retorno = ejecutarSentencias(root.ChildNodes[2].ChildNodes[1], ambito);
                                if (Data.returner || Data.breaker)
                                {
                                    ambito.escalarAmbito();
                                    return retorno;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cond)
                        {
                            retorno = ejecutarSentencias(root.ChildNodes[2], ambito);
                            if (Data.returner || Data.breaker)
                            {
                                ambito.escalarAmbito();
                                return retorno;
                            }
                        }
                    }
                }
                else if (root.ChildNodes.Count == 4)
                {
                    if (cond)
                    {
                        retorno = ejecutarSentencias(root.ChildNodes[2], ambito);
                        if (Data.returner || Data.breaker)
                        {
                            ambito.escalarAmbito();
                            return retorno;
                        }
                    }
                    else
                    {
                        if (root.ChildNodes[3].ChildNodes.Count == 2)
                        {
                            retorno = ejecutarSentencias(root.ChildNodes[3].ChildNodes[1], ambito);
                            if (Data.returner || Data.breaker)
                            {
                                ambito.escalarAmbito();
                                return retorno;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            ambito.escalarAmbito();
            return retorno;
        }

        //falta probar
        public Retorno For(ParseTreeNode root, Tabla ambito)
        {
            Retorno retorno = null;
            try
            {
                if (root.ChildNodes.Count == 8)
                {
                    string id = root.ChildNodes[1].FindTokenAndGetText();
                    string tipo = root.ChildNodes[2].ChildNodes[0].Term.Name;
                    object valor = obtenerValor(root.ChildNodes[4], ambito);
                    Simbolo contador = new Simbolo(tipo, id, valor);
                    ambito.insertarConValor(contador);

                    object cond = obtenerValor(root.ChildNodes[5], ambito);

                    if (cond is Simbolo)
                        cond = ((Simbolo)cond).valor;

                    if (!(cond is bool))
                        throw new Exception("La condicion especificada en sentencia For no evalua a un valor booleano");

                    bool condicion = (bool)cond;
                    while (condicion)
                    {
                        Tabla nueva = new Tabla(ambito);
                        nueva.heredar();
                        retorno = ejecutarSentencias(root.ChildNodes[7], nueva);
                        if (root.ChildNodes[6].ChildNodes[1].Term.Name.Equals("++"))
                            incrementar(id, nueva);
                        else
                            decrementar(id, nueva);

                        nueva.escalarAmbito();

                        condicion = (bool)obtenerValor(root.ChildNodes[5], ambito);
                       
                        if (Data.returner)
                        {
                            nueva.escalarAmbito();
                            ambito.escalarAmbito();
                            return retorno;
                        }
                        else if (Data.breaker)
                        {
                            Data.breaker = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            ambito.escalarAmbito();
            return retorno;
        }

        //falta probar
        public Retorno Switch(ParseTreeNode root, Tabla ambito)
        {
            Retorno retorno = null;
            bool ejecutado = false;
            try {              
                object recurso = aritmetica1(root.ChildNodes[1], ambito);

                if (!(recurso is Simbolo))
                    throw new Exception("No se permiten este tipo de condiciones en la sentencia Switch");

                Simbolo simbolo = (Simbolo)recurso;
             
                foreach (ParseTreeNode caso in root.ChildNodes[2].ChildNodes[0].ChildNodes)
                {
                    if (caso.ChildNodes.Count == 3)
                    {
                        object valorDelCaso = aritmetica1(caso.ChildNodes[1].ChildNodes[0], ambito);
                        if (simbolo.tipo.Equals("String"))
                        {
                            if (!(valorDelCaso is string))
                                throw new Exception("El valor de comparacion no es del tipo especificado " +
                                    "en la sentencia Switch");

                            if (((string)valorDelCaso).Equals((string)simbolo.valor))
                            {
                                Tabla tablaCaso = new Tabla(ambito);
                                tablaCaso.heredar();
                                foreach (ParseTreeNode sentencia in caso.ChildNodes[2].ChildNodes)
                                {
                                    retorno = ejecutarSentencia(sentencia.ChildNodes[0], tablaCaso);
                                    ejecutado = true;
                                    if (Data.returner) {
                                        tablaCaso.escalarAmbito();
                                        ambito.escalarAmbito();
                                        return retorno;
                                    }
                                    else if (Data.breaker)
                                    {
                                        Data.breaker = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (simbolo.tipo.Equals("Double"))
                        {
                            if (!(valorDelCaso is double))
                                throw new Exception("El valor de comparacion no es del tipo especificado" +
                                    " en la sentencia Switch");

                            if ((double)valorDelCaso == (double)simbolo.valor)
                            {
                                Tabla tablaCaso = new Tabla(ambito);
                                tablaCaso.heredar();
                                foreach (ParseTreeNode sentencia in caso.ChildNodes[2].ChildNodes)
                                {
                                    retorno = ejecutarSentencia(sentencia.ChildNodes[0], tablaCaso);
                                    ejecutado = true;
                                    if (Data.returner)
                                    {
                                        tablaCaso.escalarAmbito();
                                        ambito.escalarAmbito();
                                        return retorno;
                                    }
                                    else if (Data.breaker)
                                    {
                                        Data.breaker = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (simbolo.tipo.Equals("Int"))
                        {
                            if (!(valorDelCaso is int))
                                throw new Exception("El valor de comparacion no es del tipo especificado " +
                                    "en la sentencia Switch");

                            if ((int)valorDelCaso == (int)simbolo.valor)
                            {
                                Tabla tablaCaso = new Tabla(ambito);
                                tablaCaso.heredar();
                                foreach (ParseTreeNode sentencia in caso.ChildNodes[2].ChildNodes)
                                {
                                    retorno = ejecutarSentencia(sentencia.ChildNodes[0], tablaCaso);
                                    ejecutado = true;
                                    if (Data.returner)
                                    {
                                        tablaCaso.escalarAmbito();
                                        ambito.escalarAmbito();
                                        return retorno;
                                    }
                                    else if (Data.breaker)
                                    {
                                        Data.breaker = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (root.ChildNodes[2].ChildNodes.Count == 2 && !ejecutado)
                {
                    Tabla tablaCaso = new Tabla(ambito);
                    tablaCaso.heredar();
                    ParseTreeNode def = root.ChildNodes[2].ChildNodes[1];
                    if (def.ChildNodes.Count == 2) {
                        foreach (ParseTreeNode sentencia in def.ChildNodes[1].ChildNodes)
                        {
                            retorno = ejecutarSentencia(sentencia.ChildNodes[0], tablaCaso);
                            if (Data.returner)
                            {
                                tablaCaso.escalarAmbito();
                                ambito.escalarAmbito();
                                return retorno;
                            }
                            else if (Data.breaker)
                            {
                                Data.breaker = false;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //guardar error semantico
            }

            ambito.escalarAmbito();
            return retorno;
        }

        //falta probar
        public Retorno ejecutarSentencia(ParseTreeNode sentencia, Tabla ambito)
        {
            Retorno retorno = null;
            switch (sentencia.Term.Name)
            {
                case "DECLARACION":
                    guardarVariable(sentencia, ambito);
                    break;

                case "ASIGNACION":
                    if (sentencia.ChildNodes[2].Term.Name.Equals("EXPRESION_LOGICA"))
                        actualizarValor(sentencia, ambito);
                        else if (sentencia.ChildNodes[2].Term.Name.Equals("LISTA_DATOS"))
                        {
                            try
                            {
                                string id = sentencia.ChildNodes[0].FindTokenAndGetText();
                                Arreglo array = buscarArreglo(id, ambito);
                                guardarConValorArreglo(sentencia.ChildNodes[2], array, Data.ambitoGlobal);
                            }
                            catch (Exception e)
                            {
                                    //guardar error semantico
                            }
                        }
                    break;

                case "DECLARACION_ARREGLO":
                    guardarArreglo(sentencia, ambito);
                    break;

                case "ASIGNACION_POSICION":
                    try
                    {
                        asignarAPosicion(sentencia, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                    break;

                case "IF":
                    Tabla tablaIf = new Tabla(ambito);
                    tablaIf.heredar();
                    retorno = If_Else(sentencia, tablaIf);
                    if (Data.returner)
                        return retorno;
                    break;

                case "FOR":
                    Tabla tablaFor = new Tabla(ambito);
                    tablaFor.heredar();
                    retorno = For(sentencia, tablaFor);
                    if (Data.returner)
                        return retorno;
                    break;

                case "SWITCH":
                    Tabla tablaSwitch = new Tabla(ambito);
                    tablaSwitch.heredar();
                    retorno = Switch(sentencia, tablaSwitch);
                    if (Data.returner)
                        return retorno;
                    break;

                case "WHILE":
                    Tabla tablaWhile = new Tabla(ambito);
                    tablaWhile.heredar();
                    retorno = While(sentencia, tablaWhile);
                    if (Data.returner)
                        return retorno;
                    break;

                case "DO_WHILE":
                    Tabla tablaDo = new Tabla(ambito);
                    tablaDo.heredar();
                    retorno = Do_While(sentencia, tablaDo);
                    if (Data.returner)
                        return retorno;
                    break;

                case "FUNCION_PRINT":
                    Imprimir(sentencia.ChildNodes[1], ambito);
                    break;

                case "DINCREMENTO":
                    if (sentencia.ChildNodes[1].Term.Name.Equals("++"))
                            incrementar(sentencia.ChildNodes[0].FindTokenAndGetText(),
                            ambito);
                    else if (sentencia.ChildNodes[1].Term.Name.Equals("--"))
                               decrementar(sentencia.ChildNodes[0].FindTokenAndGetText(),
                               ambito);
                    break;

                case "LLAMADA":
                    try
                    {
                        llamada(sentencia, ambito);
                    }
                    catch (Exception e)
                    {
                        //guardar error semantico
                    }
                    break;

                case "RETORNO":
                    Data.returner = true;
                    retorno = determinarRetorno(sentencia, ambito);
                    return retorno;

                case "Break":
                    Data.breaker = true;
                    break;              
            }
            return retorno;
        }

        //falta probar
        public Retorno determinarRetorno(ParseTreeNode valor, Tabla ambito)
        {
            Retorno retorno = null;

            if (valor.ChildNodes.Count == 2)
            {
                object val = obtenerValor(valor.ChildNodes[1], ambito);

                if (val is Simbolo)
                    val = ((Simbolo)val).valor;

                if (val is Arreglo)
                    throw new Exception("No se permite retornar arreglos");

                if (val is string)
                    retorno = new Retorno("String", (string)val);
                else if (val is int)
                    retorno = new Retorno("Int", (int)val);
                else if (val is double)
                    retorno = new Retorno("Double", (double)val);
                else if (val is bool)
                    retorno = new Retorno("Bool", (bool)val);
                else if (val is char)
                    retorno = new Retorno("Char", (char)val);
            }
            else if (valor.ChildNodes.Count == 1)
                retorno = new Retorno("Void");
            return retorno;
        }

        //falta probar
        public object ejecutarMain(ArrayList parameters)
        {
            if (Data.main == null)
                throw new Exception("No hay un metodo Main escrito en el programa");

            string tipo = Data.main.tipo;
            ParseTreeNode sentencias = Data.main.root;

            if (Data.main.parametros.Count != parameters.Count)
                throw new Exception("Los parametros dados no coinciden con la declaracion del metodo main");

            Tabla tablaMain = new Tabla(Data.ambitoGlobal);
            tablaMain.heredar();

            for (int i = 0; i < parameters.Count; i++)
            {
                if (Data.main.parametros[i] is Arreglo)
                    throw new Exception("No se permiten arreglos como parametros del Main");

                Simbolo parametro = (Simbolo)Data.main.parametros[i];
                if (parametro.tipo.Equals("String"))
                {
                    parametro.valor = Convert.ToString(parameters[i]);
                }
                else if (parametro.tipo.Equals("Int"))
                {
                    if (!(parameters[i] is int))
                        throw new Exception("Se esperaba un valor entero como parametro no. " + i + 1);
                    parametro.valor = (int)parameters[i];
                }
                else if (parametro.tipo.Equals("Double"))
                {
                    if (!(parameters[i] is double) || !(parameters[i] is int))
                        throw new Exception("Se esperaba un valor decimal como parametro no. " + i + 1);
                    parametro.valor = Convert.ToDouble(parameters[i]);
                }
                else if (parametro.tipo.Equals("Bool"))
                {
                    if (!(parameters[i] is bool))
                        throw new Exception("Se esperaba un valor booleano como parametro no. " + i+1);
                }
                else if (parametro.tipo.Equals("Char"))
                {
                    if (!(parameters[i] is char))
                        throw new Exception("Se esperaba un valor de caracter como parametro no. " + i + 1);
                }
                tablaMain.insertarConValor(parametro);
            }

            if (sentencias.ChildNodes.Count == 0)
                return null;

            Retorno returner = ejecutarSentencias(sentencias.ChildNodes[0], tablaMain);

            switch (tipo)
            {
                case "String":
                    if (!(returner.tipo.Equals("String")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: String, Recibido: " + returner.tipo);
                    break;

                case "Int":
                    if (!(returner.tipo.Equals("Int")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Int, Recibido: " + returner.tipo);
                    break;

                case "Double":
                    if (!(returner.tipo.Equals("Double")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Double, Recibido: " + returner.tipo);
                    break;

                case "Bool":
                    if (!(returner.tipo.Equals("Bool")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Bool, Recibido: " + returner.tipo);
                    break;

                case "Char":
                    if (!(returner.tipo.Equals("Char")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Char, Recibido: " + returner.tipo);
                    break;

                case "Void":
                    if (returner !=  null && !(returner.tipo.Equals("Void")))
                        throw new Exception("El metodo Main no deberia haber recibido ningun tipo de retorno");
                    break;
            }

            Data.returner = false;
            Data.breaker = false;

            if (returner != null && !returner.tipo.Equals("Void"))
                return returner.valor;
            else return null;
        }

        public object ejecutarMain()
        {
            object retorno = null;
            if (Data.main == null)
                return null;

            string tipo = Data.main.tipo;
            if (Data.main.root.ChildNodes.Count == 0)
                return null;

            ParseTreeNode sentencias = Data.main.root.ChildNodes[0];
            Tabla ambitoMain = new Tabla(Data.ambitoGlobal);
            ambitoMain.heredar();
            Retorno returner = ejecutarSentencias(sentencias, ambitoMain);

            switch (tipo)
            {
                case "String":
                    if (!(returner.tipo.Equals("String")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: String, Recibido: " + returner.tipo);
                    break;

                case "Int":
                    if (!(returner.tipo.Equals("Int")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Int, Recibido: " + returner.tipo);
                    break;

                case "Double":
                    if (!(returner.tipo.Equals("Double")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Double, Recibido: " + returner.tipo);
                    break;

                case "Bool":
                    if (!(returner.tipo.Equals("Bool")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Bool, Recibido: " + returner.tipo);
                    break;

                case "Char":
                    if (!(returner.tipo.Equals("Char")))
                        throw new Exception("El tipo de retorno no concuerda con el tipo declarado en el metodo Main. " +
                            "Declarado: Char, Recibido: " + returner.tipo);
                    break;

                case "Void":
                    if (returner != null && !(returner.tipo.Equals("Void")))
                        throw new Exception("El metodo Main no deberia haber recibido ningun tipo de retorno");
                    break;
            }

            Data.returner = false;
            Data.breaker = false;

            if (returner != null && !returner.tipo.Equals("Void"))
                return returner.valor;
            else return null;
        }
    }
}
