using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window


    {
        //Guardar la amplitud de la señal mas grande para que ninguna se salga de rango
        double amplitudMaxima = 1;

        //instancia de la clase señal senoidal
        Señal señal;
        Señal segundaSeñal;
        Señal señalResultado;

        public MainWindow()
        {
            InitializeComponent(); //cualquier codigo que se ponga debajo de esta se ejecuta al iniciar el programa



        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            //para obtener el valor del text box se usa la propiedad .Text

            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);


            switch (cbTipoSeñal.SelectedIndex)
            {
                //señal senoidal
                case 0:
                    //el primer hijo del panel configuracion es la configuracion senoidal y es de otro tipo (ui collection)asi que se hace un casting y asi se puede acceder a sus propiedades, ademas como txtamplitud es tipo texto se usa parse
                    //nota: los hijos son los elementos de los contenedores
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtFrecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                //rampa
                case 1:
                    señal = new SeñalRampa();
                    break;
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)panelConfiguracion.Children[0]).txtAlpha.Text);
                    señal = new SeñalExponencial(alpha);
                    break;
                case 3:
                    señal = new SeñalRectangular();
                    break;
                default:
                    señal = null;
                    break;
            }

            //Señal 2
            switch (cbTipoSeñal_SegundaSeñal.SelectedIndex)
            {
                //señal senoidal
                case 0:
                    //el primer hijo del panel configuracion es la configuracion senoidal y es de otro tipo (ui collection)asi que se hace un casting y asi se puede acceder a sus propiedades, ademas como txtamplitud es tipo texto se usa parse
                    //nota: los hijos son los elementos de los contenedores
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion_SegundaSeñal.Children[0]).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion_SegundaSeñal.Children[0]).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)panelConfiguracion_SegundaSeñal.Children[0]).txtFrecuencia.Text);

                    segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                //rampa
                case 1:
                    segundaSeñal = new SeñalRampa();
                    break;
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)panelConfiguracion_SegundaSeñal.Children[0]).txtAlpha.Text);
                    segundaSeñal = new SeñalExponencial(alpha);
                    break;
                case 3:
                    segundaSeñal = new SeñalRectangular();
                    break;
                default:
                    segundaSeñal = null;
                    break;
            }

            //se establecen los valores para la funcion
            señal.TiempoInicial = tiempoInicial;
            señal.TiempoFinal = tiempoFinal;
            señal.FrecuenciaMuestreo = frecuenciaMuestreo;

            segundaSeñal.TiempoInicial = tiempoInicial;
            segundaSeñal.TiempoFinal = tiempoFinal;
            segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;


            //se ejecuta la funcion
            señal.construirSeñalDigital();
            segundaSeñal.construirSeñalDigital();

            if ((bool)cbAmplitud.IsChecked)
            {
                //Escalar
                double factorEscala = double.Parse(txtFactorEscalaAmplitud.Text);
                señal.escalar(factorEscala);
            }

            if ((bool)cbDesplazar.IsChecked)
            {
                //Desplazar
                double factorDesplazar = double.Parse(txtFactorDesplazamiento.Text);
                señal.desplazar(factorDesplazar);
            }

            if ((bool)cbTruncar_SegundaSeñal.IsChecked)
            {
                //Truncar
                double factorTruncar = double.Parse(txtFactorTruncar.Text);
                señal.truncar(factorTruncar);
            }

            //Segunda Señal
            if ((bool)cbAmplitud_SegundaSeñal.IsChecked)
            {
                //Escalar
                double factorEscala = double.Parse(txtFactorEscalaAmplitud_SegundaSeñal.Text);
                segundaSeñal.escalar(factorEscala);
            }

            if ((bool)cbDesplazar_SegundaSeñal.IsChecked)
            {
                //Desplazar
                double factorDesplazar = double.Parse(txtFactorDesplazamiento_SegundaSeñal.Text);
                segundaSeñal.desplazar(factorDesplazar);
            }

            if ((bool)cbTruncar_SegundaSeñal.IsChecked)
            {
                //Truncar
                double factorTruncar = double.Parse(txtFactorTruncar_SegundaSeñal.Text);
                segundaSeñal.truncar(factorTruncar);
            }

            //Aqui se toma la amplitud maxima de la señal para poder hacer que se tome el valor de la mayor para que se adapte el panel
            señal.actualizarAmplitudMaxima();
            segundaSeñal.actualizarAmplitudMaxima();
            amplitudMaxima = señal.AmplitudMaxima;
            
            if(segundaSeñal.AmplitudMaxima> amplitudMaxima)
            {
                amplitudMaxima = segundaSeñal.AmplitudMaxima;
            }

            //cambia los valores de la etiqueta
            //La F es que da el formato para redondear a 2 decimales, la funcion ToString puede recibir un parametro que es el que va a decidir en que formato va a estar,existen varios parametros
            lblAmplitudMaximaPositivaY.Text = amplitudMaxima.ToString("F");
            lblAmplitudMaximaNegativaY.Text = "-" + amplitudMaxima.ToString("F");

            // limpiar la grafica
            plnGrafica.Points.Clear();
            plnGraficaDos.Points.Clear();

            //hacerlo si la señal no es nula
           if(señal != null)
            {
                //recorrer una coleccion o arreglo
                //muestra toma el valor de señal.muestra en cada recorrido del ciclo
                foreach (Muestra muestra in señal.Muestras)
                {
                    //se evalua la señal, luego se ajusta y de ahi se agrega el punto
                    plnGrafica.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y / amplitudMaxima * ((scrContenedor.Height / 2) - 30) * -1) + (scrContenedor.Height / 2)));
                }

            }
            // hacerlo si la Segunda señal no es nula
            if (segundaSeñal != null)
            {
                //recorrer una coleccion o arreglo
                //muestra toma el valor de señal.muestra en cada recorrido del ciclo
                foreach (Muestra muestra in segundaSeñal.Muestras)
                {
                    //se evalua la señal, luego se ajusta y de ahi se agrega el punto
                    plnGraficaDos.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y / amplitudMaxima * ((scrContenedor.Height / 2) - 30) * -1) + (scrContenedor.Height / 2)));
                }

            }

            //Graficando el eje de X
            plnEjeX.Points.Clear();
            //Punto de inicio.
            plnEjeX.Points.Add(new Point(0, (scrContenedor.Height / 2)));
            //Punto de fin.
            plnEjeX.Points.Add(new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width, (scrContenedor.Height / 2)));

            //Graficando el eje de Y
            plnEjeY.Points.Clear();
            //Punto de inicio.
            plnEjeY.Points.Add(new Point(0 - tiempoInicial * scrContenedor.Width, scrContenedor.Height));
            //Punto de fin.
            plnEjeY.Points.Add(new Point(0 - tiempoInicial * scrContenedor.Width, scrContenedor.Height * -1));

        }

        private void btnGraficarRampa_Click(object sender, RoutedEventArgs e)
        {
            //tomar valores
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            //instancia
            SeñalRampa señal = new SeñalRampa();

            //calcular periodomuestreo
            double periodoMuestreo = 1 / frecuenciaMuestreo;

            //limpiar puntos
            plnGrafica.Points.Clear();

            //ciclo para conseguir las muestras
            for (double i = tiempoInicial; i <= tiempoFinal; i += periodoMuestreo)
            {
                double valorMuestra = señal.evaluar(i);
                señal.Muestras.Add(new Muestra(i, valorMuestra));

                //se van añadiendo las muestras a las listas
                señal.Muestras.Add(new Muestra(i, valorMuestra)); 

            }

            //ciclo para recorrer muestras
            foreach (Muestra muestra in señal.Muestras)
            {
                //se evalua la señal, luego se ajusta y de ahi se agrega el punto
                plnGrafica.Points.Add(new Point(muestra.X * scrContenedor.Width, (muestra.Y * ((scrContenedor.Height / 2) - 30) * -1) + (scrContenedor.Height / 2)));
            }

        }

        private void cbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
                //quita los elementos que puede tener el panel
                panelConfiguracion.Children.Clear();

                switch (cbTipoSeñal.SelectedIndex)
                {
                    case 0: //senoidal
                        //se añaden los elementos al panel (los hijos)
                        panelConfiguracion.Children.Add(
                          new ConfiguracionSeñalSenoidal() 
                            );

                        break;
                    case 1: //Rampa
                        break;
                    case 2:
                    panelConfiguracion.Children.Add(
                  new ConfiguracionSeñalExponencial()
                    );
                    break;
                default:
                        break;
                } 
        }

        private void cbAmplitud_Checked(object sender, RoutedEventArgs e)
        {
            txtFactorEscalaAmplitud.IsEnabled = true;
           
        }

        private void cbAmplitud_UnChecked(object sender, RoutedEventArgs e)
        {
            txtFactorEscalaAmplitud.IsEnabled = false;
        }

        

        private void cbDesplazar_Checked(object sender, RoutedEventArgs e)
        {
            txtFactorDesplazamiento.IsEnabled = true;

        }

        private void cbDesplazar_UnChecked(object sender, RoutedEventArgs e)
        {
            txtFactorDesplazamiento.IsEnabled = false;
        }


        private void cbTruncar_Checked(object sender, RoutedEventArgs e)
        {
            txtFactorTruncar.IsEnabled = true;

        }

        private void cbTruncar_UnChecked(object sender, RoutedEventArgs e)
        {
            txtFactorTruncar.IsEnabled = false;
        }

        private void cbTipoSeñal_SegundaSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //quita los elementos que puede tener el panel
            panelConfiguracion_SegundaSeñal.Children.Clear();

            switch (cbTipoSeñal_SegundaSeñal.SelectedIndex)
            {
                case 0: //senoidal
                        //se añaden los elementos al panel (los hijos)
                    panelConfiguracion_SegundaSeñal.Children.Add(
                      new ConfiguracionSeñalSenoidal()
                        );

                    break;
                case 1: //Rampa
                    break;
                case 2:
                    panelConfiguracion_SegundaSeñal.Children.Add(
                  new ConfiguracionSeñalExponencial()
                    );
                    break;
                default:
                    break;
            }
        }

        private void btnRealizarOperacion_Click(object sender, RoutedEventArgs e)
        {
            señalResultado = null;
            switch (cbTipoOperacion.SelectedIndex)
            {
                case 0: //Suma
                    //los metodos estaticos no necesitan una instancia
                    señalResultado = Señal.sumar(señal, segundaSeñal);
                    break;
                case 1: //multiplicacion
                    señalResultado = Señal.multiplicar(señal, segundaSeñal);
                    break;
                case 2: //convolucion
                    señalResultado = Señal.convolucionar(señal, segundaSeñal);
                    break;
                case 3:
                    señalResultado = Señal.correlacionar(señal, segundaSeñal);
                    break;
                default:
                    break;
            }

            //se actualiza la amplitud maxima del resultado 
            señalResultado.actualizarAmplitudMaxima();

            plnGraficaResultado.Points.Clear();

            //cambia los valores de la etiqueta
            //La F es que da el formato para redondear a 2 decimales, la funcion ToString puede recibir un parametro que es el que va a decidir en que formato va a estar,existen varios parametros
            lblAmplitudMaximaPositivaY_Resultado.Text = señalResultado.AmplitudMaxima.ToString("F");
            lblAmplitudMaximaNegativaY_Resultado.Text = "-" + señalResultado.AmplitudMaxima.ToString("F");

            //hacerlo si la señal no es nula
            if (señalResultado != null)
            {
                //recorrer una coleccion o arreglo
                //muestra toma el valor de señal.muestra en cada recorrido del ciclo
                foreach (Muestra muestra in señalResultado.Muestras)
                {
                    //se evalua la señal, luego se ajusta y de ahi se agrega el punto
                    plnGraficaResultado.Points.Add(new Point((muestra.X - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width, (muestra.Y / señalResultado.AmplitudMaxima * ((scrContenedor_Resultado.Height / 2) - 30) * -1) + (scrContenedor_Resultado.Height / 2)));
                }

            }
   
            //Graficando el eje de X
            plnEjeXResultado.Points.Clear();
            //Punto de inicio.
            plnEjeXResultado.Points.Add(new Point(0, (scrContenedor_Resultado.Height / 2)));
            //Punto de fin.
            plnEjeXResultado.Points.Add(new Point((señalResultado.TiempoFinal - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width, (scrContenedor_Resultado.Height / 2)));

            //Graficando el eje de Y
            plnEjeYResultado.Points.Clear();
            //Punto de inicio.
            plnEjeYResultado.Points.Add(new Point((0 - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width,(((scrContenedor_Resultado.Height/2.0)-30)*-1) + (scrContenedor_Resultado.Height/2)));
            //Punto de fin.
            plnEjeYResultado.Points.Add(new Point((0 - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width, (-1 * ((scrContenedor_Resultado.Height / 2.0)-30) * -1) + (scrContenedor_Resultado.Height/2)));

        }
    }
}

