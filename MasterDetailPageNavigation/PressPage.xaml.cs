using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class PressPage : ContentPage
    {
        public string HelpText { get; private set; }
        string MyLocation = "";
        public PressPage()
        {

            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);
            HelpText = GlobalClass.HelpString;
            MyLocation = GlobalClass.MyPage;
            BindingContext = this;

        }
        void Back_Click(object sender, EventArgs e)
        {
            MainPage mv = new MainPage();
            switch (MyLocation)
            {

                case "PersonalInfo":

                    mv.Detail = new NavigationPage(new PersonalInfo());
                    break;
                case "Schools":
                    mv.Detail = new NavigationPage(new Schools(1, 1));
                    break;
                case "Ausbildung":
                    mv.Detail = new NavigationPage(new Ausbildung(1, 1));
                    break;

                case "Berufs":
                    mv.Detail = new NavigationPage(new Berufs(1, 1));
                    break;

                case "Computer":
                    mv.Detail = new NavigationPage(new Computer(1, 1));
                    break;
                case "ContactInfo":
                    mv.Detail = new NavigationPage(new ContactInfo());
                    break;
                case "CreateDocFile":
                    mv.Detail = new NavigationPage(new CreateDocFile());
                    break;

                case "DriveLicsense":
                    mv.Detail = new NavigationPage(new DriveLicsense(1,1));
                    break;

                case "FirstPage":
                    mv.Detail = new NavigationPage(new FirstPage());
                    break;

                case "Language":
                    mv.Detail = new NavigationPage(new Language(1, 1));
                    break;

                case "LanguagePage":
                    mv.Detail = new NavigationPage(new LanguagePage());
                    break;

                case "MainPage":
                    mv.Detail = new NavigationPage(new MainPage());
                    break;

                case "MoreInfo":
                    mv.Detail = new NavigationPage(new MoreInfo(1, 1));
                    break;

                case "AppFeedback":
                    mv.Detail = new NavigationPage(new AppFeedback());
                    break;

                case "NameOfCV":
                    mv.Detail = new NavigationPage(new NameOfCV());
                    break;

                case "PersonalInfo2":
                    mv.Detail = new NavigationPage(new PersonalInfo2());
                    break;

                case "Qualifikation":
                    mv.Detail = new NavigationPage(new Qualifikation(1, 1));
                    break;


            }
            App.Current.MainPage = mv;
        }

    }
}
