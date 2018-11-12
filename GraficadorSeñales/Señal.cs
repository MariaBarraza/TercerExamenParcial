using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales 
{
    abstract class Señal
    {
        //Es un arreglo dinamico
        public List<Muestra> Muestras { get; set; }

        public double AmplitudMaxima { get; set; }
        public double TiempoInicial { get; set; }
        public double TiempoFinal { get; set; }
        public double FrecuenciaMuestreo { get; set; }
        

        public abstract double evaluar(double tiempo);

        public void construirSeñalDigital()
        {
            //calcular el periodo 
            double periodoMuestreo = 1 / FrecuenciaMuestreo; //esto se llama sacar el inverso

            //Se construye la señal digital, se delimita el tiempo con el for
            for (double i = TiempoInicial; i <= TiempoFinal; i += periodoMuestreo)
            {
                double valorMuestra = evaluar(i);

                //se calcula el numero mas alto que puede tomar la señal
                if (Math.Abs(valorMuestra) > AmplitudMaxima)
                {
                   AmplitudMaxima = Math.Abs(valorMuestra);
                }

                //se van añadiendo las muestras a las listas
                Muestras.Add(new Muestra(i, valorMuestra));
            }

        }

        public void escalar(double factor)
        {
            //por cara muestra se va a realizar esto
           foreach(Muestra muestra in Muestras)
            {
                //se multiplica por Y para escalar, no por X para conservar el instante de tiempo
                muestra.Y *= factor;
            }
        }

        public void actualizarAmplitudMaxima()
        {
            AmplitudMaxima = 0;
            
            foreach(Muestra muestra in Muestras)
            {
                if(Math.Abs(muestra.Y) > AmplitudMaxima)
                {
                    AmplitudMaxima = Math.Abs(muestra.Y);
                }
            }
        }

        public void desplazar(double factor)
        {
            //por cada muestra se va a realizar esto
            foreach (Muestra muestra in Muestras)
            {
                //se suma en Y para desplazar, no por X para conservar el instante de tiempo
                muestra.Y += factor;
            }
        }

        public void truncar(double umbral)
        {
            //por cada muestra se va a realizar esto
            foreach (Muestra muestra in Muestras)
            {
                //se suma en Y para desplazar, no por X para conservar el instante de tiempo
                if(muestra.Y>umbral)
                {
                    muestra.Y = umbral;
                }
                else
                    if(muestra.Y < -umbral)
                {
                    muestra.Y = -umbral;
                }
            }
        }
        //se hace abstracta para no necesitar crear instancias
        public static Señal sumar(Señal sumando1, Señal sumando2)
        {

            //construimos la señal resultado
            SeñalPersonalizada resultado = new SeñalPersonalizada();

            //tomamos los valores de tiempo y muestreo
            resultado.TiempoInicial = sumando1.TiempoInicial;
            resultado.TiempoFinal = sumando1.TiempoFinal;
            resultado.FrecuenciaMuestreo = sumando1.FrecuenciaMuestreo;

            //sumamos muestra por muestra
            //recorremos 1 lista de muestras y a la 2 señal accedemos por un indice
            int indice = 0;
            foreach(Muestra muestra in sumando1.Muestras)
            {
                Muestra muestraResultado = new Muestra();
                muestraResultado.X = muestra.X;
                muestraResultado.Y = muestra.Y + sumando2.Muestras[indice].Y;
                indice++;
                resultado.Muestras.Add(muestraResultado);
            }

            return resultado;
        }

        public static Señal multiplicar(Señal factor1,Señal factor2)
        {
            SeñalPersonalizada resultado = new SeñalPersonalizada();
            resultado.TiempoInicial = factor1.TiempoInicial;
            resultado.TiempoFinal = factor1.TiempoFinal;
            resultado.FrecuenciaMuestreo = factor1.FrecuenciaMuestreo;

            int indice = 0;
            foreach (Muestra muestra in factor1.Muestras)
            {
                Muestra muestraResultado = new Muestra();
                muestraResultado.X = muestra.X;
                muestraResultado.Y = muestra.Y * factor2.Muestras[indice].Y;
                indice++;
                resultado.Muestras.Add(muestraResultado);
            }

            return resultado;
        }

       
        public static Señal convolucionar(Señal operando1,Señal operando2)
        {
            SeñalPersonalizada resultado = new SeñalPersonalizada();

            //asi se deteminan los limites de una convolucion, es la suma de los limites interiores y la suma de los limites superiores
            resultado.TiempoInicial = operando1.TiempoInicial + operando2.TiempoInicial;
            resultado.TiempoFinal = operando1.TiempoFinal + operando2.TiempoFinal;

            resultado.FrecuenciaMuestreo = operando1.FrecuenciaMuestreo;

            double periodoMuestreo = 1 / resultado.FrecuenciaMuestreo;

            double duracionSeñal = resultado.TiempoFinal - resultado.TiempoInicial;

            double cantidadMuestrasResultado = duracionSeñal * resultado.FrecuenciaMuestreo;

            double instanteActual = resultado.TiempoInicial;
            for (int n = 0; n<cantidadMuestrasResultado;n++)
            {
                double valorMuestraY = 0;
                for(int k=0; k < operando2.Muestras.Count;k++)
                {
                    // el .count es una propiedad que permite contar el numero de valores que tiene una lista
                    if((n-k) >= 0 && (n-k) < operando2.Muestras.Count)
                    {
                        valorMuestraY += operando1.Muestras[k].Y * operando2.Muestras[n - k].Y;
                    } 
                }

                valorMuestraY /= resultado.FrecuenciaMuestreo;

                Muestra muestra = new Muestra(instanteActual, valorMuestraY);
                resultado.Muestras.Add(muestra);
                instanteActual += periodoMuestreo;
            }

                return resultado;
        }

        public static Señal correlacionar(Señal operando1, Señal operando2)
        {
            SeñalPersonalizada resultado = new SeñalPersonalizada();

            //asi se deteminan los limites de una convolucion, es la suma de los limites interiores y la suma de los limites superiores
            resultado.TiempoInicial = operando1.TiempoInicial - (operando2.TiempoFinal - operando2.TiempoInicial);
            resultado.TiempoFinal = operando1.TiempoFinal;

            resultado.FrecuenciaMuestreo = operando1.FrecuenciaMuestreo;

            double periodoMuestreo = 1 / resultado.FrecuenciaMuestreo;

            double duracionSeñal = resultado.TiempoFinal - resultado.TiempoInicial;

            double cantidadMuestrasResultado = duracionSeñal * resultado.FrecuenciaMuestreo;

            double instanteActual = resultado.TiempoInicial;
            for (int n = 0; n < cantidadMuestrasResultado; n++)
            {
                double valorMuestraY = 0;
                for (int k = 0; k < operando2.Muestras.Count; k++)
                {
                    // el .count es una propiedad que permite contar el numero de valores que tiene una lista
                    if ((n + k) >= 0 && (n + k) < operando2.Muestras.Count)
                    {
                        valorMuestraY += operando1.Muestras[k].Y * operando2.Muestras[n + k].Y;
                    }
                }

                valorMuestraY /= resultado.FrecuenciaMuestreo;

                Muestra muestra = new Muestra(instanteActual, valorMuestraY);
                resultado.Muestras.Add(muestra);
                instanteActual += periodoMuestreo;
            }



            return resultado;
        }

    }
}
