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

                case "INICIO":
                    if (raiz.ChildNodes.Count == 2)
                    {
                        reconocer(raiz.ChildNodes[0], actual);
                        reconocer(raiz.ChildNodes[1], actual);
                    }
                    else
                        reconocer(raiz.ChildNodes[0], actual);
                    break;

                case "ENCABEZADO":
                    foreach (ParseTreeNode root in raiz.ChildNodes)
                        reconocer(root, actual);
                    break;

                case "IMPORT":
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

                case "LISTA_ACCIONES":
                    foreach (ParseTreeNode root in raiz.ChildNodes)
                        reconocer(root, actual);
                    break;

                case "ACCION":
                    if (raiz.ChildNodes.Count == 1)
                        reconocer(raiz.ChildNodes[0], actual);
                    break;

                case "DECLARACION":
                    guardarVariable(raiz, Data.ambitoGlobal);
                    break;

                case "METODO":
                    guardarProcedimiento(raiz);
                    break;

                case "PRINCIPAL":
                    try {
                        if (actual)
                            guardarPrincipal(raiz);
                    }
                    catch (Exception e) {
                        //guardar error semantico
                    }
                    break;

                case "ASIGNACION":
                    if (raiz.ChildNodes[2].Term.Name.Equals("EXPRESION_LOGICA"))
                        actualizarValor(raiz, Data.ambitoGlobal);
                    else if (raiz.ChildNodes[2].Term.Name.Equals("LISTA_DATOS"))
                    { }
                    break;

                case "DECLARACION_ARREGLO":
                    guardarArreglo(raiz, Data.ambitoGlobal);
                    break;

                case "ASIGNACION_POSICION":
                    asignarAPosicion(raiz, Data.ambitoGlobal);
                    break;

                case "DINCREMENTO":
                    if (raiz.ChildNodes[1].Term.Name.Equals("++"))
                        incrementar(raiz.ChildNodes[0].FindTokenAndGetText(),
                            Data.ambitoGlobal);
                    else if (raiz.ChildNodes[1].Term.Name.Equals("--"))
                        decrementar(raiz.ChildNodes[0].FindTokenAndGetText(),
                            Data.ambitoGlobal);
                    break;

                case "FUNCION_PRINT":
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
                    Data.main = new Procedimiento(nombre, "Void", root.ChildNodes[2]);
                else {
                    tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                    Data.main = new Procedimiento(nombre, tipo, root.ChildNodes[2]);
                }
            }
            else if (root.ChildNodes.Count == 4) {
                if (root.ChildNodes[1].Term.Name.Equals("Void"))
                {
                    Data.main = new Procedimiento(nombre, "Void", root.ChildNodes[3]);
                    if (root.ChildNodes[2].ChildNodes.Count == 2)
                    {
                        Simbolo insert = parametroASimbolo(root.ChildNodes[2]);
                        Data.main.parametros.Add(insert);
                    }
                    else if (root.ChildNodes[2].ChildNodes.Count == 4)
                    {
                        Arreglo inserto = parametroArreglo(root.ChildNodes[2]);
                        Data.main.parametros.Add(inserto);
                    }
                }
                else {
                    tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
                    Data.main = new Procedimiento(nombre, tipo, root.ChildNodes[4]);
                    if (root.ChildNodes[2].ChildNodes.Count == 2)
                    {
                        Simbolo insert = parametroASimbolo(root.ChildNodes[2]);
                        Data.main.parametros.Add(insert);
                    }
                    else if (root.ChildNodes[2].ChildNodes.Count == 4)
                    {
                        Arreglo inserto = parametroArreglo(root.ChildNodes[2]);
                        Data.main.parametros.Add(inserto);
                    }
                }
            }
        }

        public Simbolo parametroASimbolo(ParseTreeNode parametro)
        {
            string tipo = parametro.ChildNodes[1].Term.Name;
            string id = parametro.ChildNodes[0].FindTokenAndGetText();
            Simbolo retorno = new Simbolo(tipo, id);
            return retorno;
        }

        public Arreglo parametroArreglo(ParseTreeNode parametro)
        {
            string tipo = parametro.ChildNodes[1].Term.Name;
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
                ambito.actualizarValor(id, valor);
            }
            catch (Exception e) {
                //guardar error semantico
            }
        }

        public void guardarArreglo(ParseTreeNode root, Tabla ambito) {

            int tamanio = (int)obtenerValor(root.ChildNodes[3], ambito);
            string identificador = root.ChildNodes[0].FindTokenAndGetText();
            string tipo = root.ChildNodes[1].ChildNodes[0].Term.Name;
            Arreglo array = new Arreglo(tamanio, identificador, tipo);

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

        //falta un elemento hasta el final
        public object obtenerValor(ParseTreeNode root, Tabla ambito) {
            if (root.Term.Name.Equals("EXPRESION_LOGICA"))
            {
                if (root.ChildNodes.Count == 3)
                    return logica3(root, ambito);
                else if (root.ChildNodes.Count == 2)
                    return logica2(root, ambito);
                else if (root.ChildNodes.Count == 1)
                    return obtenerValor(root.ChildNodes[0], ambito);
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
            else if (root.Term.Name.Equals("EXPRESION_RELACIONAL")) {
                if (root.ChildNodes.Count == 3)
                    return relacional(root, ambito);
                else if (root.ChildNodes.Count == 1)
                    return obtenerValor(root.ChildNodes[0], ambito);
            }
            else if (root.Term.Name.Equals("EXPRESION")) {
                if (root.ChildNodes.Count == 3)
                {
                    return aritmetica3(root, ambito);
                }
                else if (root.ChildNodes.Count == 2)
                {
                    return aritmetica2(root, ambito);
                }
                else if (root.ChildNodes.Count == 1)
                {
                    return aritmetica1(root.ChildNodes[0], ambito);
                }
                else if (root.ChildNodes.Count == 4)
                {

                }
            }
            return null;
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
            int indice;
            object valor;

            try
            {
                Arreglo aux = ambito.obtenerArreglo(identificador);
                indice = (int)obtenerValor(raiz.ChildNodes[2], ambito);
                valor = obtenerValor(raiz.ChildNodes[5], ambito);
                aux.insertarEnIndice(indice, valor);
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
                    retorno = Calculadora.negacion(valor);
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

        public bool relacional(ParseTreeNode root, Tabla ambito) {
            bool retorno = false;
            object arg1 = null;
            object arg2 = null;

            try
            {
                arg1 = obtenerValor(root.ChildNodes[0], ambito);
                arg2 = obtenerValor(root.ChildNodes[2], ambito);

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

        //no esta terminada, faltan llamadas y la funcion obtener usuario
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
                        // puede ser un arreglo tambien
                        //retorno = ambito.obtenerValor(root.FindTokenAndGetText());
                        break;

                    case "LLAMADA":
                        break;

                    case "OBTENER_USUARIO":
                        break;

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
                    return Calculadora.negativo(arg);
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
                object valor = Calculadora.incremento(antiguo);
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
                string print = Convert.ToString(obtenerValor(expresion, ambito));
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

                if (!(indice is int))
                    throw new Exception("El indice no es un entero");

                object cad = accesoArreglo(identificador, (int)indice, ambito);
                if (!(cad is string))
                    throw new Exception("El operando no es de tipo String, operacion invalida");

                string cadena = (string)cad;
                object cont = obtenerValor(datos.ChildNodes[4].ChildNodes[1], ambito);
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

        public object llamada(ParseTreeNode llamada, Tabla ambito)
        {
            object retorno = null;
            Procedimiento procedimiento = buscarProcedimiento(llamada, ambito);
            if (procedimiento == null)
                return null;

            Tabla scope = null;
            if (procedimiento.parametros.Count > 0 && llamada.ChildNodes.Count > 1)
            {
                try
                {
                    List<Parametro> pars = obtenerValoresDados(llamada.ChildNodes[1], ambito);
                    scope = construirTabla(procedimiento.parametros, pars, ambito);
                }
                catch (Exception e)
                {
                    // guardar error semantico
                }
            }
            else
            {
                scope = new Tabla(ambito);
                scope.heredar();
            }

            Retorno valor = ejecutarSentencias(procedimiento.root, scope);
            if (!(procedimiento.tipo.Equals(valor.tipo)))
                throw new Exception("El tipo de retorno esperado no concuerda con el recibido");

            retorno = valor.valor;
            return retorno;
        }

        //me falta validar que tambien los arreglos puedan ser parametros
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
            Retorno retorno = new Retorno("quemado", 0);
            foreach (ParseTreeNode hijo in sentencias.ChildNodes)
            {
                if (retorno.estado == 0)
                {
                    ParseTreeNode instruccion = hijo.ChildNodes[0];
                    switch (instruccion.Term.Name)
                    {
                        case "DECLARACION":
                            break;

                        case "ASIGNACION":
                            break;

                        case "DECLARACION_ARREGLO":
                            break;

                        case "ASIGNACION_POSICION":
                            break;

                        case "IF":
                            break;

                        case "FOR":
                            break;

                        case "SWITCH":
                            break;

                        case "WHILE":
                            break;

                        case "DO_WHILE":
                            break;

                        case "FUNCION_PRINT":
                            break;

                        case "DINCREMENTO":
                            break;

                        case "LLAMADA":
                            break;

                        case "RETORNO":
                            break;
                    }
                }
                else if (retorno.estado == 1)
                {

                }
                else if (retorno.estado == 2)
                {

                }
            }
            return retorno;
        }

        //no esta terminado
        public object While(ParseTreeNode root, Tabla ambito)
        {

            try
            {

            }
            catch (Exception e)
            {
                //guardar error semantico
            }
            return null;
        }
    }
}
