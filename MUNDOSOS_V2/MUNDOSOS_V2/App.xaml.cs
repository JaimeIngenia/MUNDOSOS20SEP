﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MUNDOSOS_V2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new EntradaUsuario());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
