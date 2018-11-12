using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalExponencial : Señal
    {
        //Propiedades
        public double Alpha { get; set; }

        public SeñalExponencial()
        {
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        public SeñalExponencial(double alpha)
        {
            Alpha = alpha;
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        //Logica Evaluacion

        override public double evaluar(double tiempo)
        {
            double resultado = Math.Exp(tiempo*Alpha);

            return resultado;
        }

    }
}
