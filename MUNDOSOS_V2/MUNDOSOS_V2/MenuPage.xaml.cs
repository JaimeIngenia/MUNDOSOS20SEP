using System;
using Xamarin.Forms;


namespace MUNDOSOS_V2
{
    
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private async void Btngraph_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GraficaPage());
        }
    }
}