using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalRampa : Señal
    {
       

        //Propiedades
        public SeñalRampa()
        {
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }
        
        //Logica Evaluacion
 
        override public double evaluar(double tiempo)
        {
            double resultado = 0;

            if (tiempo >= 0)
            {
                resultado = tiempo;
            }
            else
                if (tiempo < 0)
            {
                resultado = 0;
            }

            return resultado;
        }

    }
}
