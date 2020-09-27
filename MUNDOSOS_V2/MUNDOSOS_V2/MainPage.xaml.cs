using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;

namespace MUNDOSOS_V2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private PlotView _opv = new PlotView();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Btngraph_Clicked(object sender, EventArgs e)
        {
            WSClient client = new WSClient(); //LLAMADO DEL WEBSERVICE
            List<WSResult> d = await client.Get<WSResult>("https://gensyslabs.net/listado_panel.php");//https://gensyslabs.net/listado3.php//https://gensyslabs.net/listado3.php****https://gensyslabs.net/listado_panel.php?h=2//http://practica2020.ml/prueba1.php
            List<WSResult> d1 = await client.Get<WSResult>("https://gensyslabs.net/listado_t1.php");
            List<WSResult> d2 = await client.Get<WSResult>("https://gensyslabs.net/listado_t2.php");
            List<WSResult> d3 = await client.Get<WSResult>("https://gensyslabs.net/listado_t3.php");
            
            List<DatosPanel> dp;
            dp = new List<DatosPanel>();
            double max = 0;
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
                    if (c>max)
                    {
                        max = c;
                    }

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
            if (c > max)
            {
                max = c;
            }

            //----------------------------------segundoa lista t1----------------------------------------------
            List<DatosPanel> dt1;
            dt1 = new List<DatosPanel>();
            max = 0;
            j = 0;
            c = 0;
            while (j < d1.Count - 1)
            {
                int h1 = int.Parse(d1[j].h); // leer la primera hora
                int h2 = int.Parse(d1[j + 1].h);//leer la segunda hora
                float c1 = float.Parse(d1[j].corriente);

                if (h2 == h1) // Si las horas son iguales entonces convertimos los minutos y segundos a horas
                {
                    int restMin = ((int.Parse(d1[j + 1].m) - int.Parse(d1[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 0;
                    if (int.Parse(d1[j + 1].s) < int.Parse(d1[j].s))
                    {
                        restSeg = (int.Parse(d1[j + 1].s) + 60) - int.Parse(d1[j].s);
                        restMin = restMin - 60;
                    }
                    else
                    {
                        restSeg = int.Parse(d1[j + 1].s) - int.Parse(d1[j].s);//resta segundos en segundos
                    }
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = (SumSeg / 3600);//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado


                }
                else
                {
                    int restMin = ((59 - int.Parse(d1[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 60 - int.Parse(d1[j].s);//resta segundos en segundos
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = SumSeg / 3600;//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado

                    //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
                    DatosPanel temp = new DatosPanel();//--------------->datos panel                       
                    temp.corriente = c;
                    temp.hora = h1.ToString();
                    dt1.Add(temp);
                    if (c > max)
                    {
                        max = c;
                    }

                    c = 0;
                    restMin = (int.Parse(d1[j + 1].m) * 60);//resta mninutos en segundos
                    restSeg = (int.Parse(d1[j + 1].s));//resta segundos en segundos
                    float SumSeg2 = restMin + restSeg;
                    TiempoHoras = SumSeg2 / 3600;//horas totales de los segundos sumados
                    t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado
                    //c = c + (TiempoHoras) * c1 * voltaje;//Consumo calculado
                }
                j++;
            }
            //codigo
            float c1_ult_t1 = float.Parse(d1[d1.Count - 1].corriente);
            int restMin_ult_t1 = ((59 - int.Parse(d1[d1.Count - 1].m)) * 60);//resta mninutos en segundos
            int restSeg_ult_t1 = 60 - int.Parse(d1[d1.Count - 1].s);//resta segundos en segundos
            float SumSeg_ult_t1 = restMin_ult_t1 + restSeg_ult_t1;
            double TiempoHoras_ult_t1 = SumSeg_ult_t1 / 3600;//horas totales de los segundos sumados
            double t_ult_t1 = Math.Round((TiempoHoras_ult_t1 * c1_ult_t1 * voltaje), 9, MidpointRounding.AwayFromZero);
            double c_t1 = Math.Round((c + t_ult_t1), 9, MidpointRounding.AwayFromZero); //Consumo calculado
            //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
            int h1_ult_t1 = int.Parse(d1[d1.Count - 1].h); // leer la primera hora   
            DatosPanel temp_ult_t1 = new DatosPanel();//--------------->datos panel
            temp_ult_t1.corriente = c_t1;
            temp_ult_t1.hora = h1_ult_t1.ToString();
            dt1.Add(temp_ult_t1);
            if (c > max)
            {
                max = c;
            }
            
            //---------------------------------- lista t2----------------------------------------------
            
            List<DatosPanel> dt2;
            dt2 = new List<DatosPanel>();
            max = 0;
            j = 0;
            c = 0;
            while (j < d2.Count - 1)
            {
                int h1 = int.Parse(d2[j].h); // leer la primera hora
                int h2 = int.Parse(d2[j + 1].h);//leer la segunda hora
                float c1 = float.Parse(d2[j].corriente);

                if (h2 == h1) // Si las horas son iguales entonces convertimos los minutos y segundos a horas
                {
                    int restMin = ((int.Parse(d2[j + 1].m) - int.Parse(d2[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 0;
                    if (int.Parse(d2[j + 1].s) < int.Parse(d2[j].s))
                    {
                        restSeg = (int.Parse(d2[j + 1].s) + 60) - int.Parse(d2[j].s);
                        restMin = restMin - 60;
                    }
                    else
                    {
                        restSeg = int.Parse(d2[j + 1].s) - int.Parse(d2[j].s);//resta segundos en segundos
                    }
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = (SumSeg / 3600);//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado


                }
                else
                {
                    int restMin = ((59 - int.Parse(d2[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 60 - int.Parse(d2[j].s);//resta segundos en segundos
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = SumSeg / 3600;//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado

                    //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
                    DatosPanel temp = new DatosPanel();//--------------->datos panel                       
                    temp.corriente = c;
                    temp.hora = h1.ToString();
                    dt2.Add(temp);
                    if (c > max)
                    {
                        max = c;
                    }

                    c = 0;
                    restMin = (int.Parse(d2[j + 1].m) * 60);//resta mninutos en segundos
                    restSeg = (int.Parse(d2[j + 1].s));//resta segundos en segundos
                    float SumSeg2 = restMin + restSeg;
                    TiempoHoras = SumSeg2 / 3600;//horas totales de los segundos sumados
                    t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado
                    //c = c + (TiempoHoras) * c1 * voltaje;//Consumo calculado
                }
                j++;
            }
            //codigo
            float c1_ult_t2 = float.Parse(d2[d2.Count - 1].corriente);
            int restMin_ult_t2 = ((59 - int.Parse(d2[d2.Count - 1].m)) * 60);//resta mninutos en segundos
            int restSeg_ult_t2 = 60 - int.Parse(d2[d2.Count - 1].s);//resta segundos en segundos
            float SumSeg_ult_t2 = restMin_ult_t2 + restSeg_ult_t2;
            double TiempoHoras_ult_t2 = SumSeg_ult_t2 / 3600;//horas totales de los segundos sumados
            double t_ult_t2 = Math.Round((TiempoHoras_ult_t2 * c1_ult_t2 * voltaje), 9, MidpointRounding.AwayFromZero);
            double c_t2 = Math.Round((c + t_ult_t2), 9, MidpointRounding.AwayFromZero); //Consumo calculado
            //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
            int h1_ult_t2 = int.Parse(d2[d2.Count - 1].h); // leer la primera hora   
            DatosPanel temp_ult_t2 = new DatosPanel();//--------------->datos panel
            temp_ult_t2.corriente = c_t2;
            temp_ult_t2.hora = h1_ult_t2.ToString();
            dt2.Add(temp_ult_t2);

            if (c > max)
            {
                max = c;
            }

            //----------------------------------Tercera lista t3----------------------------------------------
            List<DatosPanel> dt3;
            dt3 = new List<DatosPanel>();
            max = 0;
            j = 0;
            c = 0;
            while (j < d3.Count - 1)
            {
                int h1 = int.Parse(d3[j].h); // leer la primera hora
                int h2 = int.Parse(d3[j + 1].h);//leer la segunda hora
                float c1 = float.Parse(d3[j].corriente);

                if (h2 == h1) // Si las horas son iguales entonces convertimos los minutos y segundos a horas
                {
                    int restMin = ((int.Parse(d3[j + 1].m) - int.Parse(d3[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 0;
                    if (int.Parse(d3[j + 1].s) < int.Parse(d3[j].s))
                    {
                        restSeg = (int.Parse(d3[j + 1].s) + 60) - int.Parse(d3[j].s);
                        restMin = restMin - 60;
                    }
                    else
                    {
                        restSeg = int.Parse(d3[j + 1].s) - int.Parse(d3[j].s);//resta segundos en segundos
                    }
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = (SumSeg / 3600);//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado


                }
                else
                {
                    int restMin = ((59 - int.Parse(d3[j].m)) * 60);//resta mninutos en segundos
                    int restSeg = 60 - int.Parse(d3[j].s);//resta segundos en segundos
                    float SumSeg = restMin + restSeg;
                    double TiempoHoras = SumSeg / 3600;//horas totales de los segundos sumados
                    double t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado

                    //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
                    DatosPanel temp = new DatosPanel();//--------------->datos panel                       
                    temp.corriente = c;
                    temp.hora = h1.ToString();
                    dt3.Add(temp);
                    if (c > max)
                    {
                        max = c;
                    }

                    c = 0;
                    restMin = (int.Parse(d3[j + 1].m) * 60);//resta mninutos en segundos
                    restSeg = (int.Parse(d3[j + 1].s));//resta segundos en segundos
                    float SumSeg2 = restMin + restSeg;
                    TiempoHoras = SumSeg2 / 3600;//horas totales de los segundos sumados
                    t = Math.Round((TiempoHoras * c1 * voltaje), 9, MidpointRounding.AwayFromZero);
                    c = Math.Round((c + t), 9, MidpointRounding.AwayFromZero); //Consumo calculado
                    //c = c + (TiempoHoras) * c1 * voltaje;//Consumo calculado
                }
                j++;
            }
            //codigo
            float c1_ult_t3 = float.Parse(d3[d3.Count - 1].corriente);
            int restMin_ult_t3 = ((59 - int.Parse(d3[d3.Count - 1].m)) * 60);//resta mninutos en segundos
            int restSeg_ult_t3 = 60 - int.Parse(d3[d3.Count - 1].s);//resta segundos en segundos
            float SumSeg_ult_t3 = restMin_ult_t3 + restSeg_ult_t3;
            double TiempoHoras_ult_t3 = SumSeg_ult_t3 / 3600;//horas totales de los segundos sumados
            double t_ult_t3 = Math.Round((TiempoHoras_ult_t3 * c1_ult_t3 * voltaje), 9, MidpointRounding.AwayFromZero);
            double c_t3 = Math.Round((c + t_ult_t3), 9, MidpointRounding.AwayFromZero); //Consumo calculado
            //Aqui es donde va el codigo donde se pasará el consumo y la hora en la nueva lista para ser graficada chuleado
            int h1_ult_t3 = int.Parse(d3[d3.Count - 1].h); // leer la primera hora   
            DatosPanel temp_ult_t3 = new DatosPanel();//--------------->datos panel
            temp_ult_t3.corriente = c_t3;
            temp_ult_t3.hora = h1_ult_t3.ToString();
            dt3.Add(temp_ult_t3);

            if (c > max)
            {
                max = c;
            }


            var plotModel = new PlotModel { Title = "Grafica de consumo" };
            CategoryAxis xaxis = new CategoryAxis();
            Title = "Series 1";
            xaxis.Position = AxisPosition.Bottom;
            xaxis.MajorGridlineStyle = LineStyle.Solid;
            xaxis.MinorGridlineStyle = LineStyle.Dot;
            xaxis.Labels.Add("8:00");
            xaxis.Labels.Add("9:00");
            xaxis.Labels.Add("10:00");
            ColumnSeries s1 = new ColumnSeries();
            s1.Title = "Panel";
            s1.IsStacked = false;

            foreach (var item in dp) // ya se agregaron las horas ya que se sustituyò este codigo //xaxis.Labels.Add("8:00");
            {
                xaxis.Labels.Add(item.hora);
                s1.Items.Add(new ColumnItem(item.corriente));
            };


            ColumnSeries s2 = new ColumnSeries();
            s2.Title = "Consumo";
            s2.IsStacked = false;
            //s2.Items.Add(new ColumnItem(1.5));
            //s2.Items.Add(new ColumnItem(1.6));
            //s2.Items.Add(new ColumnItem(0.4));/////////amarilla

            foreach (var item in dt1) // ya se agregaron las horas ya que se sustituyò este codigo //xaxis.Labels.Add("8:00");
            {
                s2.Items.Add(new ColumnItem(item.corriente));
            };
            
            ColumnSeries s3 = new ColumnSeries();
            s3.IsStacked = false;
            foreach (var item in dt2) // ya se agregaron las horas ya que se sustituyò este codigo //xaxis.Labels.Add("8:00");
            {
                s3.Items.Add(new ColumnItem(item.corriente));
            };

            //s3.Items.Add(new ColumnItem(1.2));
            //s3.Items.Add(new ColumnItem(1.3));
            //s3.Items.Add(new ColumnItem(1.4));
            //s3.Items.Add(new ColumnItem(1.5));


            ColumnSeries s4 = new ColumnSeries();
            s4.IsStacked = false;
            foreach (var item in dt3) // ya se agregaron las horas ya que se sustituyò este codigo //xaxis.Labels.Add("8:00");
            {
                s4.Items.Add(new ColumnItem(item.corriente));
            };
            //s4.Items.Add(new ColumnItem(1.5));//azul1
            //s4.Items.Add(new ColumnItem(1.4));//azul2
            //s4.Items.Add(new ColumnItem(1.3));
            //s4.Items.Add(new ColumnItem(1.2));
            
            plotModel.Series.Add(s1);
            plotModel.Series.Add(s2);
            plotModel.Series.Add(s3);
            plotModel.Series.Add(s4);

            plotModel.Axes.Add(xaxis);
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = max });

            _opv.Model = plotModel;
            Content = _opv;

        }
    }
}
