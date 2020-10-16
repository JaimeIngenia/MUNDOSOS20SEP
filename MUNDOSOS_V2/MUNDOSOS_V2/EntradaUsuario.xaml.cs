using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MUNDOSOS_V2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntradaUsuario : ContentPage
    {
        public EntradaUsuario()
        {
            InitializeComponent();
        }

        private async void login_Clicked(object sender, EventArgs e)
        {
            WSClient client = new WSClient(); //LLAMADO DEL WEBSERVICE
            List<WSlogin> d = await client.Get<WSlogin>("https://gensyslabs.net/login.php?correo="+email.Text+"&doc="+pass.Text);

            if (d.Count>0)
            {
                //await DisplayAlert("Alert", "Bienvenido", "OK");
                await Navigation.PushAsync(new MenuPage());
            }
            else
            {
                await DisplayAlert("Alert", "Malvenido", "OK");
            }

        }
    }
}