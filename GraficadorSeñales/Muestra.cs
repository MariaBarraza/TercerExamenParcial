using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class Muestra
    {
        //Aqui se van a guardar los datos de las muestras para poder tener altura y tiempo de cada una de ellas

            //El instante del tiempo en que fue tomada la muestra
        public double X { get; set; }
            //El valor de esa muestra en ese instante
        public double Y { get; set; }


        //Constructor que inicializa valores, los parametros van en minusculas  y las propiedades en mayusculas
        public Muestra(double x, double y)
        {

            X = x;

            Y = y;
        }

        //Constructor sin parametros
        public Muestra()
        {
            X = 0.0;
            Y = 0.0;
        }

    }
}
