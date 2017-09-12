using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace VisionBuddy
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();           
        }

        protected override void OnStart()
        {
            // Set MainPage XAML as a root page of the Navigation Architecture
            var mainPage = new NavigationPage(new MainPage());                       
            MainPage = mainPage;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
