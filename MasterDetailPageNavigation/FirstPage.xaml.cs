
using System;
using Xamarin.Forms;

namespace Lebenslauf
{

    public partial class FirstPage : ContentPage
    {

        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        public string Label2 { get; private set; }
        public string Label3 { get; private set; }
        DBStorageCV dbcv;

        public FirstPage()
        {
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);

            CheckLang();
            dbcv = new DBStorageCV();
            BindingContext = this;
        }
        void Create_Click(object sender, EventArgs e)
        {
            // new MainPage().Detail.Navigation.PushModalAsync(new PersonalInfo());
            // ((MasterDetailPage)Parent).IsPresented = false;
            //this.Navigation.PushAsync(new PersonalInfo());

            dbcv.AddNewCV(); //Neuen Lebenslauf erstellen
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new NameOfCV());
            App.Current.MainPage = mv;

        }
        void Edit_Click(object sender, EventArgs e)
        {
            // new MainPage().Detail.Navigation.PushModalAsync(new PersonalInfo());
            // ((MasterDetailPage)Parent).IsPresented = false;
            //this.Navigation.PushAsync(new PersonalInfo());
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new LebenslaufList());
            App.Current.MainPage = mv;

        }

        void Set_Click(object sender, EventArgs e)
        {
            // new MainPage().Detail.Navigation.PushModalAsync(new PersonalInfo());
            // ((MasterDetailPage)Parent).IsPresented = false;
            //this.Navigation.PushAsync(new PersonalInfo());
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new LanguagePage());
            App.Current.MainPage = mv;

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch  (Lang)
            {
                case 1:
                    MainLabel = "Lebenslauf";
                    Label1 = "Lebenslauf Erstellen";
                    Label2 = "Lebenslauf Bearbeiten";
                    Label3 = "Einstellungen";
                    break;
                case 2:
                    MainLabel = "Curriculum vitae";
                    Label1 = "Create a CV";
                    Label2 = "Edit a CV";
                    Label3 = "Settings";
                    break;
                case 4:
                    MainLabel = "Curriculum vitae";
                    Label1 = "produire le CV";
                    Label2 = "Modifier le CV";
                    Label3 = "Paramètres";
                    break;
                case 3:
                    MainLabel = "السيرة الذاتية";
                    Label1 = "انشاء سيرة ذاتية";
                    Label2 = "تعديل سيرة ذاتية";
                    Label3 = "الإعدادات";
                    break;


            }

        }

    }
}

