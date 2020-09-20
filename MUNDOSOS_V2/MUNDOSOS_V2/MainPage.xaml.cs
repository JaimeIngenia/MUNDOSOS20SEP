using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace MUNDOSOS_V2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Btngraph_Clicked(object sender, EventArgs e)
        {
            List<DatosPanel> dp;
            dp = new List<DatosPanel>(); 
            WSClient client = new WSClient(); //LLAMADO DEL WEBSERVICE
            List<WSResult> d = await client.Get<WSResult>("https://gensyslabs.net/listado_panel.php");//https://gensyslabs.net/listado3.php//https://gensyslabs.net/listado3.php****https://gensyslabs.net/listado_panel.php?h=2//http://practica2020.ml/prueba1.php
            
            int voltaje = 110;
            int j = 0;
            double c = 0;
            while (j < d.Count-1)
            {
                int h1 = int.Parse(d[j].h); // leer la primera hora
                int h2 = int.Parse(d[j + 1].h);//leer la segunda hora
                float c1 = float.Parse(d[j].corriente);

                if (h2 == h1) // Si las horas son iguales entonces convertimos los minutos y segundos a horas
                {
                    int restMin = ((int.Parse(d[j + 1].m) - int.Parse(d[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 0;
                    if (int.Parse(d[j + 1].s) < int.Parse(d[j].s))
                    {
                        restSeg = (int.Parse(d[j + 1].s) + 60) - int.Parse(d[j].s);
                        restMin = restMin - 60;
                    }
                    else
                    {
                        restSeg = int.Parse(d[j + 1].s) - int.Parse(d[j].s);//resta segundos en segundos
                    }
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = (SumSeg / 3600);//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado


                }
                else
                {
                    int restMin = ((59 - int.Parse(d[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 60 - int.Parse(d[j].s);//resta segundos en segundos
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = SumSeg / 3600;//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado

                    //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
                    DatosPanel temp = new DatosPanel();//--------------->datos panel                       
                    temp.corriente = c;
                    temp.hora = h1.ToString();
                    dp.Add(temp);

                    c = 0;
                    restMin = (int.Parse(d[j + 1].m) * 60);//resta mninutos en segundos
                    restSeg = (int.Parse(d[j + 1].s));//resta segundos en segundos
                    float SumSeg2 = restMin + restSeg;
                    TiempoHoras = SumSeg2 / 3600;//horas totales de los segundos sumados
                    t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado
                    //c = c + (TiempoHoras) * c1 * voltaje;//Consumo calculado
                }
                j++;
            }
            //codigo
            float c1_ult = float.Parse(d[d.Count-1].corriente);
            int restMin_ult = ((59 - int.Parse(d[d.Count - 1].m)) * 60);//resta mninutos en segundos
            int restSeg_ult = 60 - int.Parse(d[d.Count - 1].s);//resta segundos en segundos
            float SumSeg_ult = restMin_ult + restSeg_ult;
            double TiempoHoras_ult = SumSeg_ult / 3600;//horas totales de los segundos sumados
            double t_ult = Math.Round((TiempoHoras_ult * c1_ult * voltaje), 9, MidpointRounding.AwayFromZero);
            c = Math.Round((c + t_ult), 9, MidpointRounding.AwayFromZero); //Consumo calculado
            //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
            int h1_ult = int.Parse(d[d.Count - 1].h); // leer la primera hora
            DatosPanel temp_ult = new DatosPanel();//--------------->datos panel                       
            temp_ult.corriente = c;
            temp_ult.hora = h1_ult.ToString();
            dp.Add(temp_ult);

        }
    }
}
