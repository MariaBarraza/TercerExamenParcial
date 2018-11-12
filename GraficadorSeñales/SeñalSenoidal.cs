using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalSenoidal : Señal
    {

        //Propiedades de la clase necesarios para la funcion senoidal
        public double Amplitud { get; set; }
        public double Fase { get; set; }
        public double Frecuencia { get; set; }

            
        //Constructor de la clase

        public SeñalSenoidal()
        {
            Amplitud = 1.0;
            Fase = 0.0;
            Frecuencia = 1.0;
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        public SeñalSenoidal(double amplitud, double fase, double frecuencia)
        {
            Amplitud = amplitud;
            Fase = fase;
            Frecuencia = frecuencia;
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        //funcion matematica que permite evaluar segun un instante de tiempo
        override public double evaluar(double tiempo)
        {
            double resultado = Amplitud * Math.Sin(((2 * Math.PI * Frecuencia) * tiempo) + Fase);
            return resultado;
        }

    }
}
