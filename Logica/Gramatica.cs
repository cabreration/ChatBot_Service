using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace ChatBot_Service.Logica
{
    public class Gramatica: Grammar
    {
        public Gramatica() : base(caseSensitive: true) {

            #region Terminales

            //comentarios
            CommentTerminal simple = new CommentTerminal("simple", "//", "\n", "\r\n");
            CommentTerminal multiple = new CommentTerminal("multiple", "/*", "*/");
            NonGrammarTerminals.Add(simple);
            NonGrammarTerminals.Add(multiple);

            //tipos de datos
            var entero = ToTerm("Int", "Int");
            var cadena = ToTerm("String", "String");
            var doble = ToTerm("Double", "Double");
            var booleano = ToTerm("Bool", "Bool");
            var caracter = ToTerm("Char", "Char");
            var vacio = ToTerm("Void", "Void");

            //palabras reservadas
            var importar = ToTerm("Import", "Import");
            var retornar = ToTerm("Return", "Return");
            var imprimir = ToTerm("Print", "Print");
            var principal = ToTerm("Main", "Main");
            var comparar = ToTerm("CompareTo", "CompareTo");
            var obtenerUsuario = ToTerm("GetUser", "GetUser");

            //palabras reservadas de sentencias de control
            var si = ToTerm("If", "If");
            var sino = ToTerm("Else", "Else");
            var cambiar = ToTerm("Switch", "Switch");
            var caso = ToTerm("Case", "Case");
            var defecto = ToTerm("Default", "Default");
            var quebrar = ToTerm("Break", "Break");
            var para = ToTerm("For", "For");
            var mientras = ToTerm("While", "While");
            var hacer = ToTerm("Do", "Do");

            //operadores aritmeticos
            var sumar = ToTerm("+", "+");
            var restar = ToTerm("-", "-");
            var multiplicar = ToTerm("*", "*");
            var dividir = ToTerm("/", "/");
            var modular = ToTerm("%", "%");
            var elevar = ToTerm("^", "^");
            var asignacion = ToTerm("=", "=");
            var incremento = ToTerm("++", "++");
            var decremento = ToTerm("--", "--");

            //operadores relacionales
            var menorQue = ToTerm("<", "<");
            var mayorQue = ToTerm(">", ">");
            var menorIgual = ToTerm("<=", "<=");
            var mayorIgual = ToTerm(">=", ">=");
            var equivalente = ToTerm("==", "==");
            var noEquivalente = ToTerm("!=", "!=");

            //operadores logicos
            var and = ToTerm("&&", "&&");
            var or = ToTerm("||", "||");
            var xor = ToTerm("|&", "|&");
            var not = ToTerm("!", "!");

            //punctuation mark
            var finSentencia = ToTerm(";", ";");
            var parentesisA = ToTerm("(", "(");
            var parentesisC = ToTerm(")", ")");
            var llaveA = ToTerm("{", "{");
            var llaveC = ToTerm("}", "}");
            var dosPuntos = ToTerm(":", ":");
            var corcheteA = ToTerm("[", "[");
            var corcheteC = ToTerm("]", "]");
            var coma = ToTerm(",", ",");
            var punto = ToTerm(".", ".");

            //valores
            var numero = TerminalFactory.CreateCSharpNumber("numero");
            var identificador = TerminalFactory.CreateCSharpIdentifier("identificador");
            var cad = TerminalFactory.CreateCSharpString("cadena");
            var falso = ToTerm("False", "falso");
            var verdadero = ToTerm("True", "verdadero");
            var car = TerminalFactory.CreateCSharpChar("caracter");

            #endregion

            #region Precedencias y puntuaciones
            this.MarkPunctuation(finSentencia, parentesisA, parentesisC,
                llaveA, llaveC, dosPuntos, corcheteA, corcheteC, coma, punto);

            RegisterOperators(1, Associativity.Left, sumar, restar);
            RegisterOperators(2, Associativity.Left, multiplicar, modular, dividir);
            RegisterOperators(3, Associativity.Right, elevar);
            RegisterOperators(5, equivalente, noEquivalente, menorQue, menorIgual, mayorQue, mayorIgual);
            RegisterOperators(6, Associativity.Left, or);
            RegisterOperators(7, Associativity.Left, xor);
            RegisterOperators(8, Associativity.Left, and);
            RegisterOperators(9, Associativity.Left, not);
            RegisterOperators(10, parentesisA, parentesisC);

            #endregion

            #region No Terminales

            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal DECLARACION = new NonTerminal("DECLARACION");
            NonTerminal PRINCIPAL = new NonTerminal("PRINCIPAL");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            NonTerminal WHILE = new NonTerminal("WHILE");
            NonTerminal DO_WHILE = new NonTerminal("DO_WHILE");
            NonTerminal IF = new NonTerminal("IF");
            NonTerminal ELSE = new NonTerminal("ELSE");
            NonTerminal FOR = new NonTerminal("FOR");
            NonTerminal SWITCH = new NonTerminal("SWITCH");
            NonTerminal LISTA_ACCIONES = new NonTerminal("LISTA_ACCIONES");
            NonTerminal ACCION = new NonTerminal("ACCION");
            NonTerminal IMPORT = new NonTerminal("IMPORT");
            NonTerminal ENCABEZADO = new NonTerminal("ENCABEZADO");
            NonTerminal TIPO_DATO = new NonTerminal("TIPO_DATO");
            NonTerminal LISTA_VARS = new NonTerminal("LISTA_VARS");
            NonTerminal EXPRESION_LOGICA = new NonTerminal("EXPRESION_LOGICA");
            NonTerminal EXPRESION_RELACIONAL = new NonTerminal("EXPRESION_RELACIONAL");
            NonTerminal EXPRESION = new NonTerminal("EXPRESION");
            NonTerminal DECLARACION_ARREGLO = new NonTerminal("DECLARACION_ARREGLO");
            NonTerminal ASIGNACION_ARREGLO = new NonTerminal("ASIGNACION_ARREGLO");
            NonTerminal LISTA_DATOS = new NonTerminal("LISTA_DATOS");
            NonTerminal ASIGNACION_POSICION = new NonTerminal("ASIGNACION_POSICION");
            NonTerminal SENTENCIA = new NonTerminal("SENTENCIA");
            NonTerminal LISTA_SENTENCIAS = new NonTerminal("LISTA_SENTENCIAS");
            NonTerminal PROCEDIMIENTO = new NonTerminal("PROCEDIMIENTO");
            NonTerminal LISTA_PARAMETROS = new NonTerminal("LISTA_PARAMETROS");
            NonTerminal PARAMETRO = new NonTerminal("PARAMETRO");
            NonTerminal FUNCION_PRINT = new NonTerminal("FUNCION_PRINT");
            NonTerminal FUNCION_COMPARAR = new NonTerminal("FUNCION_COMPARAR");
            NonTerminal RETORNO = new NonTerminal("RETORNO");
            NonTerminal OBTENER_USUARIO = new NonTerminal("OBTENER_USUARIO");
            NonTerminal OPERADOR_RELACIONAL = new NonTerminal("OPERADOR_RELACIONAL");
            NonTerminal LLAMADA = new NonTerminal("LLAMADA");
            NonTerminal DINCREMENTO = new NonTerminal("DINCREMENTO");
            NonTerminal CASE = new NonTerminal("CASE");
            NonTerminal DEFAULT = new NonTerminal("DEFAULT");
            NonTerminal CUERPO_SWITCH = new NonTerminal("CUERPO_SWITCH");
            NonTerminal LISTA_CASE = new NonTerminal("LISTA_CASE");
            NonTerminal VALOR = new NonTerminal("VALOR");
            NonTerminal BREAK = new NonTerminal("BREAK");
            NonTerminal OPCION_SENTENCIAS = new NonTerminal("OPCION_SENTENCIAS");

            #endregion

            #region reglas gramaticales

            //Encabezado
            ENCABEZADO.Rule = MakePlusRule(ENCABEZADO, IMPORT)
                | IMPORT;

            IMPORT.Rule = importar + cad + finSentencia;

            // Cuerpo principal
            this.Root = INICIO;

            INICIO.Rule = ENCABEZADO + LISTA_ACCIONES
                | ENCABEZADO
                | LISTA_ACCIONES;

            LISTA_ACCIONES.Rule = MakePlusRule(LISTA_ACCIONES, ACCION)
                | ACCION;

            ACCION.Rule = DECLARACION
                | PROCEDIMIENTO
                | PRINCIPAL
                | ASIGNACION
                | DECLARACION_ARREGLO
                | ASIGNACION_POSICION
                | DINCREMENTO + finSentencia
                | FUNCION_PRINT + finSentencia;

            PRINCIPAL.Rule = principal + dosPuntos + vacio + parentesisA + parentesisC + llaveA + OPCION_SENTENCIAS + llaveC
                | principal + dosPuntos + TIPO_DATO + parentesisA + identificador + dosPuntos + TIPO_DATO + parentesisC
                + llaveA + OPCION_SENTENCIAS + llaveC
                | principal + dosPuntos + vacio + parentesisA + identificador + dosPuntos 
                + TIPO_DATO + parentesisC + llaveA + OPCION_SENTENCIAS + llaveC
                | principal + dosPuntos + TIPO_DATO + parentesisA + parentesisC + llaveA + OPCION_SENTENCIAS + llaveC;

            // Sintaxis de las Declaraciones
            TIPO_DATO.Rule = entero | doble | cadena | caracter | booleano;

            LISTA_VARS.Rule = MakePlusRule(LISTA_VARS, coma, identificador);

            DECLARACION.Rule = LISTA_VARS + dosPuntos + TIPO_DATO + asignacion + EXPRESION_LOGICA + finSentencia
                | identificador + dosPuntos + TIPO_DATO + asignacion + EXPRESION_LOGICA + finSentencia
                | LISTA_VARS + dosPuntos + TIPO_DATO + finSentencia
                | identificador + dosPuntos + TIPO_DATO + finSentencia;

            // Sintaxis de las Asignaciones
            ASIGNACION.Rule = identificador + asignacion + EXPRESION_LOGICA + finSentencia
                | identificador + asignacion + llaveA + LISTA_DATOS + llaveC + finSentencia;

            // Sintaxis de los arreglos
            DECLARACION_ARREGLO.Rule = identificador + dosPuntos + TIPO_DATO
                + corcheteA + EXPRESION + corcheteC + finSentencia
                | identificador + dosPuntos + TIPO_DATO + corcheteA + EXPRESION + corcheteC
                + asignacion + ASIGNACION_ARREGLO + finSentencia;

            LISTA_DATOS.Rule = MakePlusRule(LISTA_DATOS, coma, EXPRESION_LOGICA)
                | EXPRESION_LOGICA;

            ASIGNACION_ARREGLO.Rule = llaveA + LISTA_DATOS + llaveC
                | identificador;

            ASIGNACION_POSICION.Rule = identificador + corcheteA + EXPRESION + corcheteC
                + asignacion + EXPRESION_LOGICA + finSentencia;

            //Sintaxis de Metodos y Funciones
            PROCEDIMIENTO.Rule = identificador + dosPuntos + TIPO_DATO
                + parentesisA + LISTA_PARAMETROS + parentesisC + llaveA + OPCION_SENTENCIAS + llaveC
                | identificador + dosPuntos + vacio
                + parentesisA + LISTA_PARAMETROS + parentesisC + llaveA + OPCION_SENTENCIAS + llaveC
                | identificador + dosPuntos + TIPO_DATO
                + parentesisA + parentesisC + llaveA + OPCION_SENTENCIAS + llaveC
                | identificador + dosPuntos + vacio
                + parentesisA + parentesisC + llaveA + OPCION_SENTENCIAS + llaveC;

            PARAMETRO.Rule = identificador + dosPuntos + TIPO_DATO;

            LISTA_PARAMETROS.Rule = MakePlusRule(LISTA_PARAMETROS, coma, PARAMETRO)
                | PARAMETRO;

            LISTA_SENTENCIAS.Rule = MakePlusRule(LISTA_SENTENCIAS, SENTENCIA)
                | SENTENCIA;

            OPCION_SENTENCIAS.Rule = LISTA_SENTENCIAS | Empty;

            SENTENCIA.Rule = DECLARACION
                | ASIGNACION
                | DECLARACION_ARREGLO
                | ASIGNACION_POSICION
                | IF
                | FOR
                | SWITCH
                | WHILE
                | DO_WHILE
                | FUNCION_PRINT + finSentencia
                | DINCREMENTO + finSentencia
                | RETORNO;

            RETORNO.Rule = retornar + EXPRESION_LOGICA + finSentencia
                | retornar + finSentencia;

            //Funciones nativas
            FUNCION_PRINT.Rule = imprimir + parentesisA + EXPRESION_LOGICA + parentesisC;

            FUNCION_COMPARAR.Rule = punto + comparar + parentesisA + EXPRESION_LOGICA + parentesisC;

            OBTENER_USUARIO.Rule = obtenerUsuario + parentesisA + parentesisC;

            //If Else
            IF.Rule = si + parentesisA + EXPRESION_LOGICA + parentesisC + llaveA + LISTA_SENTENCIAS + llaveC
                | si + parentesisA + EXPRESION_LOGICA + parentesisC + llaveA + llaveC
                | si + parentesisA + EXPRESION_LOGICA + parentesisC + llaveA + LISTA_SENTENCIAS + llaveC + ELSE
                | si + parentesisA + EXPRESION_LOGICA + parentesisC + llaveA + llaveC + ELSE;

            ELSE.Rule = sino + llaveA + LISTA_SENTENCIAS + llaveC
                | sino + llaveA + llaveC;

            //Break
            BREAK.Rule = quebrar | Empty;

            //While
            WHILE.Rule = mientras + parentesisA + EXPRESION_LOGICA + parentesisC 
                + llaveA + LISTA_SENTENCIAS + BREAK + llaveC
                | mientras + parentesisA + EXPRESION_LOGICA + parentesisC + llaveA + BREAK + llaveC;

            //Do While
            DO_WHILE.Rule = hacer + llaveA + LISTA_SENTENCIAS + BREAK + llaveC + mientras + parentesisA
                + EXPRESION_LOGICA + parentesisC + finSentencia
                | hacer + llaveA + BREAK + llaveC + mientras + parentesisA + EXPRESION_LOGICA + parentesisC + finSentencia;

            //For
            FOR.Rule = para + parentesisA + identificador + dosPuntos + TIPO_DATO + asignacion + EXPRESION
                + finSentencia + EXPRESION_LOGICA + finSentencia + DINCREMENTO + parentesisC + llaveA + LISTA_SENTENCIAS
                + BREAK + llaveC
                | para + parentesisA + identificador + dosPuntos + TIPO_DATO + asignacion + EXPRESION
                + finSentencia + EXPRESION_LOGICA + finSentencia + DINCREMENTO + parentesisC + llaveA + BREAK + llaveC;

            DINCREMENTO.Rule = identificador + incremento
                | identificador + decremento;

            //Switch
            SWITCH.Rule = cambiar + parentesisA + EXPRESION_LOGICA + parentesisC + llaveA + CUERPO_SWITCH + llaveC;

            CUERPO_SWITCH.Rule = LISTA_CASE
                | LISTA_CASE + DEFAULT;

            LISTA_CASE.Rule = MakePlusRule(LISTA_CASE, CASE)
                | CASE;

            CASE.Rule = caso + VALOR + dosPuntos + LISTA_SENTENCIAS + quebrar
                | caso + VALOR + dosPuntos + LISTA_SENTENCIAS
                | caso + VALOR + dosPuntos + quebrar
                | caso + VALOR + dosPuntos;

            VALOR.Rule = cad | numero | car | verdadero | falso;

            DEFAULT.Rule = defecto + dosPuntos + LISTA_SENTENCIAS + quebrar
                | defecto + dosPuntos + LISTA_SENTENCIAS
                | defecto + dosPuntos + quebrar
                | defecto + dosPuntos;

            //Llamadas
            LLAMADA.Rule = identificador + parentesisA + parentesisC;

            // Expresiones
            EXPRESION_LOGICA.Rule = EXPRESION_LOGICA + and + EXPRESION_LOGICA
                | EXPRESION_LOGICA + or + EXPRESION_LOGICA
                | EXPRESION_LOGICA + xor + EXPRESION_LOGICA
                | not + EXPRESION_LOGICA
                | identificador + FUNCION_COMPARAR
                | EXPRESION_RELACIONAL;

            EXPRESION_RELACIONAL.Rule = EXPRESION + OPERADOR_RELACIONAL + EXPRESION
                | EXPRESION;

            OPERADOR_RELACIONAL.Rule = menorQue
                | menorIgual
                | mayorQue
                | mayorIgual
                | equivalente
                | noEquivalente;

            EXPRESION.Rule = EXPRESION + sumar + EXPRESION
                | EXPRESION + restar + EXPRESION
                | EXPRESION + multiplicar + EXPRESION
                | EXPRESION + dividir + EXPRESION
                | EXPRESION + modular + EXPRESION
                | EXPRESION + elevar + EXPRESION
                | restar + EXPRESION
                | parentesisA + EXPRESION_LOGICA + parentesisC
                | identificador
                | LLAMADA
                | OBTENER_USUARIO
                | verdadero
                | falso
                | cad
                | car
                | numero;
            #endregion
        }
    }
}
