using FreshMvvm;
using Xamarin.Forms;

namespace Lebenslauf
{
	public class App : Application
	{
        
		public App ()
		{
            //**
            MainPage = new Lebenslauf.MainPage ();
            //**


            //InitializeComponent();
          

            //var page = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
            //var basicNavigationContainer = new FreshNavigationContainer(page);
            //MainPage = basicNavigationContainer;
            //MainPage = page;

          
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

