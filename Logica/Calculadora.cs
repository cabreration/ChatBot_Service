using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Service.Logica
{
    public class Calculadora
    {

        public static object sumar(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando contiene un valor no existente");
            if (arg2 == null)
                throw new Exception("El segundo operando contiene un valor no existente");

            if (arg1 is string) {
                return (string)arg1 + Convert.ToString(arg2);
            }
            else if (arg1 is int) {
                if (arg2 is string)
                    return Convert.ToString((int)arg1) + (string)arg2;
                else if (arg2 is int)
                    return (int)arg1 + (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble((int)arg1) + (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2)
                        return (int)arg1 + 1;
                    else return (int)arg1;
                }
                else if (arg2 is char)
                    return (int)arg1 + Convert.ToInt32((char)arg2);
            }
            else if (arg1 is double) {
                if (arg2 is string)
                    return Convert.ToString((double)arg1) + (string)arg2;
                else if (arg2 is int)
                    return (double)arg1 + Convert.ToDouble((int)arg2);
                else if (arg2 is double)
                    return (double)arg1 + (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2)
                        return (double)arg1 + 1;
                    else return (double)arg1;
                }
                else if (arg2 is char)
                    return (double)arg1 + Convert.ToDouble((Convert.ToInt32((char)arg2)));                  
            }
            else if (arg1 is bool) {
                if (arg2 is string)
                    return Convert.ToString((bool)arg1) + (string)arg2;
                else if (arg2 is int)
                {
                    if ((bool)arg1)
                        return (int)arg2 + 1;
                    else return (int)arg2;
                }
                else if (arg2 is double)
                {
                    if ((bool)arg1)
                        return (double)arg2 + 1;
                    else return (double)arg2;
                }
                else if (arg2 is bool)
                {
                    if ((bool)arg1)
                    {
                        if ((bool)arg1)
                            return 2.0;
                        else return 1.0;
                    }
                    else {
                        if ((bool)arg2) return 1.0;
                    }
                }
                else if (arg2 is char)
                    throw new Exception("No se pueden sumar booleanos con caracteres");
            }
            else if (arg1 is char) {
                if (arg2 is string)
                    return Convert.ToString((char)arg1) + (string)arg2;
                else if (arg2 is int)
                    return Convert.ToInt32((char)arg1) + (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble(Convert.ToInt32((char)arg1)) + (double)arg2;
                else if (arg2 is bool)
                    throw new Exception("No puede sumar caracteres con booleanos");
                else if (arg2 is char)
                    return Convert.ToInt32((char)arg1) + Convert.ToInt32((char)arg2);
            }

            throw new Exception("Error inesperado, no fue posible realizar la suma");
        }

        public static object restar(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando tiene un valor no existente");
            if (arg2 == null)
                throw new Exception("El segundo operando tiene un valor no existente");

            if (arg1 is string)
                throw new Exception("Restas no pueden realizarse sobre cadenas de caracteres");
            else if (arg1 is int) {
                if (arg2 is string)
                    throw new Exception("La operacion de resta no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return (int)arg1 - (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble((int)arg1) - (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (int)arg1 - 1;
                    else return (int)arg1;
                }
                else if (arg2 is char)
                    return (int)arg1 - Convert.ToInt32((char)arg2);
            }
            else if (arg1 is double) {
                if (arg2 is string)
                    throw new Exception("La operacion de resta no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return (double)arg1 - Convert.ToDouble((int)arg2);
                else if (arg2 is double)
                    return (double)arg1 - (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (double)arg1 - 1;
                    else return (double)arg1;
                }
                else if (arg2 is char)
                    return (double)arg1 - Convert.ToDouble(Convert.ToInt32((char)arg2));               
            }
            else if (arg1 is bool) {
                if (arg2 is string)
                    throw new Exception("La operacion de resta no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                {
                    if ((bool)arg1)
                        return 1 - (int)arg2;
                    else return -(int)arg2;
                }
                else if (arg2 is double)
                {
                    if ((bool)arg1)
                        return 1 - (double)arg2;
                    else return -(double)arg2;
                }
                else if (arg2 is bool)
                    throw new Exception("La operacion de resta no puede realizarse con dos operandos booleanos");
                else if (arg2 is char)
                    throw new Exception("La operacion de resta no puede realizarse" +
                        " con un operando booleano y otro caracter");
            }
            else if (arg1 is char) {
                if (arg2 is string)
                    throw new Exception("No puede realizarse la operacion de resta sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Convert.ToInt32((char)arg1) - (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble(Convert.ToInt32((char)arg1)) - (double)arg2;
                else if (arg2 is bool)
                    throw new Exception("No pueden realizarse restas entre caracteres y booleanos");
                else if (arg2 is char)
                    return Convert.ToInt32((char)arg1) - Convert.ToInt32((char)arg2);
            }

            throw new Exception("Error inesperado, no fue posible realizar la resta");
        }

        public static object multiplicar(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando tiene un valor no existente");
            if (arg2 == null)
                throw new Exception("El segundo operando tiene un valor no existente");

            if (arg1 is string)
                throw new Exception("Multiplicaciones no pueden realizarse sobre cadenas de caracteres");
            else if (arg1 is int)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de multiplicacion no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return (int)arg1 * (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble((int)arg1) * (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (int)arg1;
                    else return 0;
                }
                else if (arg2 is char)
                    return (int)arg1 * Convert.ToInt32((char)arg2);
            }
            else if (arg1 is double)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de multiplicacion no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return (double)arg1 * Convert.ToDouble((int)arg2);
                else if (arg2 is double)
                    return (double)arg1 * (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (double)arg1;
                    else return 0.0;
                }
                else if (arg2 is char)
                    return (double)arg1 * Convert.ToDouble(Convert.ToInt32((char)arg2));
            }
            else if (arg1 is bool)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de multiplicacion no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                {
                    if ((bool)arg1)
                        return (int)arg2;
                    else return 0;
                }
                else if (arg2 is double)
                {
                    if ((bool)arg1)
                        return (double)arg2;
                    else return 0.0;
                }
                else if (arg2 is bool)
                    throw new Exception("La operacion de multiplicacion no puede realizarse con dos operandos booleanos");
                else if (arg2 is char)
                    throw new Exception("La operacion de multiplicacion no puede realizarse" +
                        " con un operando booleano y otro caracter");
            }
            else if (arg1 is char)
            {
                if (arg2 is string)
                    throw new Exception("No puede realizarse la operacion de multiplicacion sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Convert.ToInt32((char)arg1) * (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble(Convert.ToInt32((char)arg1)) * (double)arg2;
                else if (arg2 is bool)
                    throw new Exception("No pueden realizarse multiplicaciones entre caracteres y booleanos");
                else if (arg2 is char)
                    return Convert.ToInt32((char)arg1) * Convert.ToInt32((char)arg2);
            }

            throw new Exception("Error inesperado, no fue posible realizar la multiplicacion");
        }

        public static object dividir(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando tiene un valor no existente");
            if (arg2 == null)
                throw new Exception("El segundo operando tiene un valor no existente");

            if (arg1 is string)
                throw new Exception("Divisiones no pueden realizarse sobre cadenas de caracteres");
            else if (arg1 is int)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de division no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Convert.ToDouble((int)arg1) / Convert.ToDouble((int)arg2);
                else if (arg2 is double)
                    return Convert.ToDouble((int)arg1) / (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (int)arg1;
                    else throw new Exception("No puede dividir sobre 0");
                }
                else if (arg2 is char)
                    return Convert.ToDouble((int)arg1) / Convert.ToDouble(Convert.ToInt32((char)arg2));
            }
            else if (arg1 is double)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de division no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return (double)arg1 / Convert.ToDouble((int)arg2);
                else if (arg2 is double)
                    return (double)arg1 / (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (double)arg1;
                    else throw new Exception("No puede dividir sobre 0");
                }
                else if (arg2 is char)
                    return (double)arg1 / Convert.ToDouble(Convert.ToInt32((char)arg2));
            }
            else if (arg1 is bool)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de division no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                {
                    if ((bool)arg1)
                        return 1.0 / Convert.ToDouble((int)arg2);
                    else return 0.0;
                }
                else if (arg2 is double)
                {
                    if ((bool)arg1)
                        return 1.0 / (double)arg2;
                    else return 0;
                }
                else if (arg2 is bool)
                    throw new Exception("La operacion de division no puede realizarse con dos operandos booleanos");
                else if (arg2 is char)
                    throw new Exception("La operacion de division no puede realizarse" +
                        " con un operando booleano y otro caracter");
            }
            else if (arg1 is char)
            {
                if (arg2 is string)
                    throw new Exception("No puede realizarse la operacion de division sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Convert.ToDouble(Convert.ToInt32((char)arg1)) / Convert.ToDouble((int)arg2);
                else if (arg2 is double)
                    return Convert.ToDouble(Convert.ToInt32((char)arg1)) / (double)arg2;
                else if (arg2 is bool)
                    throw new Exception("No pueden realizarse divisiones entre caracteres y booleanos");
                else if (arg2 is char)
                    return Convert.ToDouble(Convert.ToInt32((char)arg1)) / Convert.ToDouble(Convert.ToInt32((char)arg2));
            }

            throw new Exception("Error inesperado, no fue posible realizar la division");
        }

        public static object modular(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando tiene un valor no existente");
            if (arg2 == null)
                throw new Exception("El segundo operando tiene un valor no existente");

            if (arg1 is string)
                throw new Exception("Modulaciones no pueden realizarse sobre cadenas de caracteres");
            else if (arg1 is int)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de modulacion no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return (int)arg1 % (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble((int)arg1) % (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return 0;
                    else throw new Exception("No puede modular sobre 0");
                }
                else if (arg2 is char)
                    return (int)arg1 % Convert.ToInt32((char)arg2);
            }
            else if (arg1 is double)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de modulacion no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return (double)arg1 % Convert.ToDouble((int)arg2);
                else if (arg2 is double)
                    return (double)arg1 % (double)arg2;
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return 0.0;
                    else throw new Exception("No puede modular sobre 0");
                }
                else if (arg2 is char)
                    return (double)arg1 % Convert.ToDouble(Convert.ToInt32((char)arg2));
            }
            else if (arg1 is bool)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de modulacion no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                {
                    if ((bool)arg1)
                        return 1.0 % Convert.ToDouble((int)arg2);
                    else return 0.0;
                }
                else if (arg2 is double)
                {
                    if ((bool)arg1)
                        return 1.0 % (double)arg2;
                    else return 0;
                }
                else if (arg2 is bool)
                    throw new Exception("La operacion de modulacion no puede realizarse con dos operandos booleanos");
                else if (arg2 is char)
                    throw new Exception("La operacion de modulacion no puede realizarse" +
                        " con un operando booleano y otro caracter");
            }
            else if (arg1 is char)
            {
                if (arg2 is string)
                    throw new Exception("No puede realizarse la operacion de modulacion sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Convert.ToInt32((char)arg1) % (int)arg2;
                else if (arg2 is double)
                    return Convert.ToDouble(Convert.ToInt32((char)arg1)) % (double)arg2;
                else if (arg2 is bool)
                    throw new Exception("No pueden realizarse modulaciones entre caracteres y booleanos");
                else if (arg2 is char)
                    return Convert.ToInt32((char)arg1) % Convert.ToInt32((char)arg2);
            }

            throw new Exception("Error inesperado, no fue posible realizar la modulacion");
        }

        public static object elevar(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando tiene un valor no existente");
            if (arg2 == null)
                throw new Exception("El segundo operando tiene un valor no existente");

            if (arg1 is string)
                throw new Exception("Potencias no pueden realizarse sobre cadenas de caracteres");
            else if (arg1 is int)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de potencia no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Convert.ToInt32(Math.Pow((int)arg1, (int)arg2));
                else if (arg2 is double)
                    return Convert.ToInt32(Math.Pow(Convert.ToDouble((int)arg1), (double)arg2));
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (int)arg1;
                    else return 0;
                }
                else if (arg2 is char)
                    return Convert.ToInt32(Math.Pow(Convert.ToDouble((int)arg1),
                        Convert.ToDouble(Convert.ToInt32((char)arg2))));
            }
            else if (arg1 is double)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de potencia no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Math.Pow((double)arg1, Convert.ToDouble((int)arg2));
                else if (arg2 is double)
                    return Math.Pow((double)arg1, (double)arg2);
                else if (arg2 is bool)
                {
                    if ((bool)arg2) return (double)arg1;
                    else return 0.0;
                }
                else if (arg2 is char)
                    return Math.Pow((double)arg1, Convert.ToDouble(Convert.ToInt32((char)arg2)));
            }
            else if (arg1 is bool)
            {
                if (arg2 is string)
                    throw new Exception("La operacion de potencia no puede realizarse sobre cadenas de caracteres");
                else if (arg2 is int)
                {
                    if ((bool)arg1)
                        return 1;
                    else return 0;
                }
                else if (arg2 is double)
                {
                    if ((bool)arg1)
                        return 1.0;
                    else return 0.0;
                }
                else if (arg2 is bool)
                    throw new Exception("La operacion de potencia no puede realizarse con dos operandos booleanos");
                else if (arg2 is char)
                    throw new Exception("La operacion de potencia no puede realizarse" +
                        " con un operando booleano y otro caracter");
            }
            else if (arg1 is char)
            {
                if (arg2 is string)
                    throw new Exception("No puede realizarse la operacion de modulacion sobre cadenas de caracteres");
                else if (arg2 is int)
                    return Convert.ToInt32(Math.Pow(Convert.ToDouble(Convert.ToInt32((char)arg1)),
                        Convert.ToDouble((int)arg2)));
                else if (arg2 is double)
                    return Math.Pow(Convert.ToDouble(Convert.ToInt32((char)arg1)), (double)arg2);
                else if (arg2 is bool)
                    throw new Exception("No pueden realizarse potencias entre caracteres y booleanos");
                else if (arg2 is char)
                    return Convert.ToInt32(Math.Pow(Convert.ToDouble(Convert.ToInt32((char)arg1)),
                        Convert.ToDouble(Convert.ToInt32((char)arg2))
));
            }

            throw new Exception("Error inesperado, no fue posible realizar la potencia");
        }

        public static object negativo(object arg1) {
            if (arg1 == null) throw new Exception("El argumento es de valor nulo");
            else if (arg1 is string) throw new Exception("Imposible hacer una cadena negativa");
            else if (arg1 is bool)
            {
                if ((bool)arg1) return -1;
                else return 0;
            }
            else if (arg1 is int)
                return -(int)arg1;
            else if (arg1 is double)
                return -(double)arg1;
            else if (arg1 is char)
                return -Convert.ToInt32((char)arg1);

            throw new Exception("Imposible realizar la operacion");
        }

        public static bool and(object arg1, object arg2) {
            if (arg1 == null) throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null) throw new Exception("El segundo argumento es de valor nulo");

            if (arg1 is string)
                throw new Exception("No puede realizar conjunciones sobre cadenas de caracteres");
            else if (arg1 is int)
                throw new Exception("No puede realizar conjunciones sobre enteros");
            else if (arg1 is double)
                throw new Exception("No puede realizar conjunciones sobre decimales");
            else if (arg1 is bool) {
                if (arg2 is string)
                    throw new Exception("No puede realizar conjunciones entre valores booleanos y cadenas");
                else if (arg2 is int)
                    throw new Exception("No puede realizar conjunciones entre valores booleanos y enteros");
                else if (arg2 is double)
                    throw new Exception("No puede realizar conjunciones entre valores booleanos y decimales");
                else if (arg2 is bool)
                    return (bool)arg1 && (bool)arg2;
                else if (arg2 is char)
                    throw new Exception("No puede realizar conjunciones entre valores booleanos y caracteres");
            }
            else if (arg1 is char)
                throw new Exception("No puede realizar conjunciones sobre caracteres");

            throw new Exception("Error inesperado, no se pudo realizar la conjuncion");
        }

        public static bool or(object arg1, object arg2) {
            if (arg1 == null) throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null) throw new Exception("El segundo argumento es de valor nulo");

            if (arg1 is string)
                throw new Exception("No puede realizar disyunciones sobre cadenas de caracteres");
            else if (arg1 is int)
                throw new Exception("No puede realizar disyunciones sobre enteros");
            else if (arg1 is double)
                throw new Exception("No puede realizar disyunciones sobre decimales");
            else if (arg1 is bool)
            {
                if (arg2 is string)
                    throw new Exception("No puede realizar disyunciones entre valores booleanos y cadenas");
                else if (arg2 is int)
                    throw new Exception("No puede realizar disyunciones entre valores booleanos y enteros");
                else if (arg2 is double)
                    throw new Exception("No puede realizar disyunciones entre valores booleanos y decimales");
                else if (arg2 is bool)
                    return (bool)arg1 || (bool)arg2;
                else if (arg2 is char)
                    throw new Exception("No puede realizar disyunciones entre valores booleanos y caracteres");
            }
            else if (arg1 is char)
                throw new Exception("No puede realizar disyunciones sobre caracteres");

            throw new Exception("Error inesperado, no se pudo realizar la disyuncion");
        }

        public static bool xor(object arg1, object arg2) {
            if (arg1 == null) throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null) throw new Exception("El segundo argumento es de valor nulo");

            if (arg1 is string)
                throw new Exception("No puede realizar disyunciones exclusivas sobre cadenas de caracteres");
            else if (arg1 is int)
                throw new Exception("No puede realizar disyunciones exclusivas sobre enteros");
            else if (arg1 is double)
                throw new Exception("No puede realizar disyunciones exclusivas sobre decimales");
            else if (arg1 is bool)
            {
                if (arg2 is string)
                    throw new Exception("No puede realizar disyunciones exclusivas entre valores booleanos y cadenas");
                else if (arg2 is int)
                    throw new Exception("No puede realizar disyunciones exclusivas entre valores booleanos y enteros");
                else if (arg2 is double)
                    throw new Exception("No puede realizar disyunciones exclusivas entre valores booleanos y decimales");
                else if (arg2 is bool)
                    return (bool)arg1 ^ (bool)arg2;
                else if (arg2 is char)
                    throw new Exception("No puede realizar disyunciones exclusivas entre valores booleanos y caracteres");
            }
            else if (arg1 is char)
                throw new Exception("No puede realizar disyunciones exclusivas sobre caracteres");

            throw new Exception("Error inesperado, no se pudo realizar la disyuncion exclusiva");
        }

        public static bool negacion(object arg1) {
            if (arg1 == null)
                throw new Exception("El operando es de valor nulo");

            if (arg1 is string)
                throw new Exception("No se pueden realizar negaciones sobre cadenas de caracteres");
            else if (arg1 is int)
                throw new Exception("No se pueden realizar negaciones logicas sobre enteros");
            else if (arg1 is double)
                throw new Exception("No se pueden realizar negaciones logicas sobre decimales");
            else if (arg1 is bool)
                return !(bool)arg1;
            else if (arg1 is char)
                throw new Exception("No se pueden realizar negaciones logicas sobre caracteres");

            throw new Exception("Error inesperado, no se pudo realizar la negacion logica");
        }

        public static bool menorQue(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null)
                throw new Exception("El segundo operando es de valor nulo");

            if (arg1 is string) {
                if (arg2 is string) return menorQueCadena((string)arg1, (string)arg2);
                else throw new Exception("Las comparaciones de cadenas solo pueden darse entre cadena y cadena");
            }
            else if (arg1 is int) {
                if (arg2 is int) return (int)arg1 < (int)arg2;
                else if (arg2 is double) return Convert.ToDouble((int)arg1) < (double)arg2;
                else throw new Exception("Las comparaciones de enteros solo pueden darse entre entero y entero");
            }
            else if (arg1 is double) {
                if (arg2 is double) return (double)arg1 < (double)arg2;
                else if (arg2 is int) return (double)arg1 < Convert.ToDouble((int)arg2);
                else throw new Exception("Las comparaciones de decimales solo pueden darse entre decimal y decimal");
            }
            else if (arg1 is bool) {
                if (arg2 is bool)
                    return booleanoEntero((bool)arg1) < booleanoEntero((bool)arg2);
                else throw new Exception("Las comparaciones de booleanos solo pueden darse entre booleano y booleano");              
            }
            else if (arg1 is char) {
                if (arg2 is char)
                    return Convert.ToInt32((char)arg1) < Convert.ToInt32((char)arg2);
                else throw new Exception("Las comparaciones de caracteres solo pueden darse entre caracter y caracter");
            }

            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        public static bool mayorQue(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null)
                throw new Exception("El segundo operando es de valor nulo");

            if (arg1 is string)
            {
                if (arg2 is string) return mayorQueCadena((string)arg1, (string)arg2);
                else throw new Exception("Las comparaciones de cadenas solo pueden darse entre cadena y cadena");
            }
            else if (arg1 is int)
            {
                if (arg2 is int) return (int)arg1 > (int)arg2;
                else if (arg2 is double) return Convert.ToDouble((int)arg1) > (double)arg2;
                else throw new Exception("Las comparaciones de enteros solo pueden darse entre entero y entero");
            }
            else if (arg1 is double)
            {
                if (arg2 is double) return (double)arg1 > (double)arg2;
                else if (arg2 is int) return (double)arg1 > Convert.ToDouble((int)arg2);
                else throw new Exception("Las comparaciones de decimales solo pueden darse entre decimal y decimal");
            }
            else if (arg1 is bool)
            {
                if (arg2 is bool)
                    return booleanoEntero((bool)arg1) > booleanoEntero((bool)arg2);
                else throw new Exception("Las comparaciones de booleanos solo pueden darse entre booleano y booleano");
            }
            else if (arg1 is char)
            {
                if (arg2 is char)
                    return Convert.ToInt32((char)arg1) > Convert.ToInt32((char)arg2);
                else throw new Exception("Las comparaciones de caracteres solo pueden darse entre caracter y caracter");
            }

            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        public static bool menorIgual(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null)
                throw new Exception("El segundo operando es de valor nulo");

            if (arg1 is string)
            {
                if (arg2 is string) return menorIgualCadena((string)arg1, (string)arg2);
                else throw new Exception("Las comparaciones de cadenas solo pueden darse entre cadena y cadena");
            }
            else if (arg1 is int)
            {
                if (arg2 is int) return (int)arg1 <= (int)arg2;
                else if (arg2 is double) return Convert.ToDouble((int)arg1) <= (double)arg2;
                else throw new Exception("Las comparaciones de enteros solo pueden darse entre entero y entero");
            }
            else if (arg1 is double)
            {
                if (arg2 is double) return (double)arg1 <= (double)arg2;
                else if (arg2 is int) return (double)arg1 <= Convert.ToDouble((int)arg2);
                else throw new Exception("Las comparaciones de decimales solo pueden darse entre decimal y decimal");
            }
            else if (arg1 is bool)
            {
                if (arg2 is bool)
                    return booleanoEntero((bool)arg1) <= booleanoEntero((bool)arg2);
                else throw new Exception("Las comparaciones de booleanos solo pueden darse entre booleano y booleano");
            }
            else if (arg1 is char)
            {
                if (arg2 is char)
                    return Convert.ToInt32((char)arg1) <= Convert.ToInt32((char)arg2);
                else throw new Exception("Las comparaciones de caracteres solo pueden darse entre caracter y caracter");
            }

            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        public static bool mayorIgual(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null)
                throw new Exception("El segundo operando es de valor nulo");

            if (arg1 is string)
            {
                if (arg2 is string) return mayorIgualCadena((string)arg1, (string)arg2);
                else throw new Exception("Las comparaciones de cadenas solo pueden darse entre cadena y cadena");
            }
            else if (arg1 is int)
            {
                if (arg2 is int) return (int)arg1 >= (int)arg2;
                else if (arg2 is double) return Convert.ToDouble((int)arg1) >= (double)arg2;
                else throw new Exception("Las comparaciones de enteros solo pueden darse entre entero y entero");
            }
            else if (arg1 is double)
            {
                if (arg2 is double) return (double)arg1 >= (double)arg2;
                else if (arg2 is int) return (double)arg1 >= Convert.ToDouble((int)arg2);
                else throw new Exception("Las comparaciones de decimales solo pueden darse entre decimal y decimal");
            }
            else if (arg1 is bool)
            {
                if (arg2 is bool)
                    return booleanoEntero((bool)arg1) >= booleanoEntero((bool)arg2);
                else throw new Exception("Las comparaciones de booleanos solo pueden darse entre booleano y booleano");
            }
            else if (arg1 is char)
            {
                if (arg2 is char)
                    return Convert.ToInt32((char)arg1) >= Convert.ToInt32((char)arg2);
                else throw new Exception("Las comparaciones de caracteres solo pueden darse entre caracter y caracter");
            }

            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        public static bool equivalente(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null)
                throw new Exception("El segundo operando es de valor nulo");

            if (arg1 is string)
            {
                if (arg2 is string) return ((string)arg1).Equals((string)arg2);
                else throw new Exception("Las comparaciones de cadenas solo pueden darse entre cadena y cadena");
            }
            else if (arg1 is int)
            {
                if (arg2 is int) return (int)arg1 == (int)arg2;
                else if (arg2 is double) return Convert.ToDouble((int)arg1) == (double)arg2;
                else throw new Exception("Las comparaciones de enteros solo pueden darse entre entero y entero");
            }
            else if (arg1 is double)
            {
                if (arg2 is double) return (double)arg1 == (double)arg2;
                else if (arg2 is int) return (double)arg1 == Convert.ToDouble((int)arg2);
                else throw new Exception("Las comparaciones de decimales solo pueden darse entre decimal y decimal");
            }
            else if (arg1 is bool)
            {
                if (arg2 is bool)
                    return booleanoEntero((bool)arg1) == booleanoEntero((bool)arg2);
                else throw new Exception("Las comparaciones de booleanos solo pueden darse entre booleano y booleano");
            }
            else if (arg1 is char)
            {
                if (arg2 is char)
                    return Convert.ToInt32((char)arg1) == Convert.ToInt32((char)arg2);
                else throw new Exception("Las comparaciones de caracteres solo pueden darse entre caracter y caracter");
            }

            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        public static bool noEquivalente(object arg1, object arg2) {
            if (arg1 == null)
                throw new Exception("El primer operando es de valor nulo");
            if (arg2 == null)
                throw new Exception("El segundo operando es de valor nulo");

            if (arg1 is string)
            {
                if (arg2 is string) return !((string)arg1).Equals((string)arg2);
                else throw new Exception("Las comparaciones de cadenas solo pueden darse entre cadena y cadena");
            }
            else if (arg1 is int)
            {
                if (arg2 is int) return (int)arg1 != (int)arg2;
                else if (arg2 is double) return Convert.ToDouble((int)arg1) != (double)arg2;
                else throw new Exception("Las comparaciones de enteros solo pueden darse entre entero y entero");
            }
            else if (arg1 is double)
            {
                if (arg2 is double) return (double)arg1 != (double)arg2;
                else if (arg2 is int) return (double)arg1 != Convert.ToDouble((int)arg2);
                else throw new Exception("Las comparaciones de decimales solo pueden darse entre decimal y decimal");
            }
            else if (arg1 is bool)
            {
                if (arg2 is bool)
                    return booleanoEntero((bool)arg1) != booleanoEntero((bool)arg2);
                else throw new Exception("Las comparaciones de booleanos solo pueden darse entre booleano y booleano");
            }
            else if (arg1 is char)
            {
                if (arg2 is char)
                    return Convert.ToInt32((char)arg1) != Convert.ToInt32((char)arg2);
                else throw new Exception("Las comparaciones de caracteres solo pueden darse entre caracter y caracter");
            }

            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        private static bool menorQueCadena(string cadena1, string cadena2) {
            if (cadena1.Length < cadena2.Length) {
                for (int i = 0; i < cadena1.Length; i++) {
                    if (Convert.ToInt32(cadena1[i]) > Convert.ToInt32(cadena2[i])) return false;
                    else if (Convert.ToInt32(cadena1[i]) < Convert.ToInt32(cadena2[i])) return true;
                }
                return true;
            }
            else if (cadena1.Length >= cadena2.Length) {
                for (int i = 0; i < cadena1.Length; i++)
                {
                    if (Convert.ToInt32(cadena1[i]) > Convert.ToInt32(cadena2[i])) return false;
                    else if (Convert.ToInt32(cadena1[i]) < Convert.ToInt32(cadena2[i])) return true;
                }
                return false;
            }
            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        private static bool mayorQueCadena(string cadena1, string cadena2) {
            if (cadena1.Length <= cadena2.Length)
            {
                for (int i = 0; i < cadena1.Length; i++)
                {
                    if (Convert.ToInt32(cadena1[i]) > Convert.ToInt32(cadena2[i])) return true;
                    else if (Convert.ToInt32(cadena1[i]) < Convert.ToInt32(cadena2[i])) return false;
                }
                return false;
            }
            else if (cadena1.Length > cadena2.Length)
            {
                for (int i = 0; i < cadena1.Length; i++)
                {
                    if (Convert.ToInt32(cadena1[i]) > Convert.ToInt32(cadena2[i])) return true;
                    else if (Convert.ToInt32(cadena1[i]) < Convert.ToInt32(cadena2[i])) return false;
                }
                return true;
            }
            throw new Exception("Error inesperado, la comparacion no se llevo a cabo");
        }

        private static bool menorIgualCadena(string cadena1, string cadena2) {
            if (cadena1.Equals(cadena2)) return true;

            return menorQueCadena(cadena1, cadena2);
        }

        private static bool mayorIgualCadena(string cadena1, string cadena2) {
            if (cadena1.Equals(cadena2)) return true;

            return mayorQueCadena(cadena1, cadena2);
        }

        private static int booleanoEntero(bool arg) {
            if (arg) return 1;
            else return 0;
        }
    }
}
