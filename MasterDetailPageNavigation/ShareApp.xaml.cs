using Plugin.Share;
using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class ShareApp : ContentPage
    {
        public ShareApp()
        {
            InitializeComponent();

            CrossShare.Current.Share("Leider, unser Link ist noch nicht Verfügbar", "Lebenslauf");
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new FirstPage());
            App.Current.MainPage = mv;

        }
    }
}
