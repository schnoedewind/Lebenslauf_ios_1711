using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class PersonalInfo2 : ContentPage
    {
        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        public string Label2 { get; private set; }
        public string Label3 { get; private set; }
        public string Label4 { get; private set; }
        PersonendatenTableItem pdRow;
        DBStorageCV dbcv;
        //Farben festlegen A.S.
        Color EntryBackColor = Color.FromRgb(80, 20, 20);
        Color EntryTextColor = Color.White;

        public PersonalInfo2()
        {
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);

            //UI Plattformabhängig konfigurieren A.S.
            Device.OnPlatform(iOS: () => SetUIiOS());

            //Datenbank initialisieren
            dbcv = new DBStorageCV();

            LoadData();
            CheckLang();
            BindingContext = this;

        }

        //Textfarbe und Hintergrundfarbe für Eingabefelder setzen A.S.
        void SetUIiOS()
        {
            SetUIEntryiOS(Kinder);
            SetUIEntryiOS(Staatsangehörigkeit);

            FStand.BackgroundColor = EntryBackColor;
            FStand.TextColor = EntryTextColor;           

            Hobbies.BackgroundColor = EntryBackColor;
            Hobbies.TextColor = EntryTextColor;
           

        }

        void SetUIEntryiOS(Entry ent)
        {
            ent.BackgroundColor = EntryBackColor;
            ent.TextColor = EntryTextColor;
        }


        //Daten speichern
        void LoadData()
        {
            pdRow = dbcv.dbCon.Get<PersonendatenTableItem>(dbcv.CurrentCVID);
            switch (pdRow.Familienstand)
            {
                case "verheiratet": FStand.SelectedIndex = 1; break;
                case "geschieden": FStand.SelectedIndex = 2; break;
                case "verwitwet": FStand.SelectedIndex = 3; break;
                default: FStand.SelectedIndex = 0; break;
            }
            Kinder.Text = pdRow.Kinder;
            Staatsangehörigkeit.Text = pdRow.Nationalität;
            Hobbies.Text = pdRow.Hobbies;
        }

        //Daten der Seite speichern
        void SaveData()
        {
            pdRow = dbcv.dbCon.Get<PersonendatenTableItem>(dbcv.CurrentCVID);
            pdRow.Kinder = Kinder.Text;
            pdRow.Nationalität = Staatsangehörigkeit.Text;
            switch (FStand.SelectedIndex)
            {
                case 0: pdRow.Familienstand = "ledig"; break;
                case 1: pdRow.Familienstand = "verheiratet"; break;
                case 2: pdRow.Familienstand = "geschieden"; break;
                case 3: pdRow.Familienstand = "verwitwet"; break;
            }
            pdRow.Hobbies = Hobbies.Text;

            dbcv.dbCon.Update(pdRow);
        }


        void Next_Click(object sender, EventArgs e)
        {
            // new MainPage().Detail.Navigation.PushModalAsync(new PersonalInfo());
            // ((MasterDetailPage)Parent).IsPresented = false;
            //this.Navigation.PushAsync(new PersonalInfo());

            SaveData();
            dbcv.dbCon.Close();

            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new ContactInfo());
            App.Current.MainPage = mv;

        }
        void Prev_Click(object sender, EventArgs e)
        {
            // new MainPage().Detail.Navigation.PushModalAsync(new PersonalInfo());
            // ((MasterDetailPage)Parent).IsPresented = false;
            //this.Navigation.PushAsync(new PersonalInfo());

            SaveData();
            dbcv.dbCon.Close();

            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new PersonalInfo());
            App.Current.MainPage = mv;

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "Persönlichen Daten";
                    Label1 = "Familienstand:";
                    Label2 = "Staatsangehörigkeit:";
                    Label3 = "Kinder/Alter:";
                    Label4 = "Hobbies:";
                    break;
                case 2:
                    MainLabel = "Personal Data";
                    Label1 = "Marital status:";
                    Label2 = "Nationality:";
                    Label3 = "Children/Age:";
                    Label4 = "Hobbys:";
                    break;
                case 4:
                    MainLabel = "Données personnelles";
                    Label1 = "état civil:";
                    Label2 = "Nationalité:";
                    Label3 = "enfants / âge:";
                    Label4 = "Hobbys:";
                    break;
                case 3:
                    MainLabel = "المعلومات الشخصية";
                    Label1 = "الحالة الإجتماعية:";
                    Label2 = "الجنسية:";
                    Label3 = "الأولاد/العمر:";
                    Label4 = "الهوايات:";
                    break;


            }

        }
    }
}
