using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class ContactInfo : ContentPage
    {
        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        public string Label2 { get; private set; }
        public string Label3 { get; private set; }
        public string Label4 { get; private set; }
        public string Label5 { get; private set; }
        KontaktdatenTableItem dbRow;
        DBStorageCV dbcv;
        //Farben festlegen A.S.
        Color EntryBackColor = Color.FromRgb(80, 20, 20);
        Color EntryTextColor = Color.White;

        public ContactInfo()
        {
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);
            
            //UI Plattformabhängig konfigurieren A.S.
            Device.OnPlatform(iOS: () => SetUIiOS());
            
            //Datenbank initialisieren
            dbcv = new DBStorageCV();
            CheckLang();
            BindingContext = this;
            LoadData();
        }

        //Textfarbe und Hintergrundfarbe für Eingabefelder setzen A.S.
        void SetUIiOS()
        {
            SetUIEntryiOS(Wohnort);
            SetUIEntryiOS(PLZ);
            SetUIEntryiOS(Straße);
            SetUIEntryiOS(Telefon);
            SetUIEntryiOS(EMail);
        }

        void SetUIEntryiOS(Entry ent)
        {
            ent.BackgroundColor = EntryBackColor;
            ent.TextColor = EntryTextColor;
        }

        //Daten laden
        void LoadData()
        {
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<KontaktdatenTableItem>().Where(v => v.PersonendatenID == curCVID);
            if (query.Count() > 0)
            {
                dbRow = query.First();
                Wohnort.Text = dbRow.Ort;
                PLZ.Text = dbRow.PLZ;
                Straße.Text = dbRow.Strasse;
                Telefon.Text = dbRow.Telefon;
                EMail.Text = dbRow.Email;
            }
        }

        //Daten der Seite speichern
        void SaveData()
        {
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<KontaktdatenTableItem>().Where(v => v.PersonendatenID == curCVID );
            if (query.Count() > 0)
            {

                dbRow = query.First();
                dbRow.Ort = Wohnort.Text;
                dbRow.PLZ = PLZ.Text;
                dbRow.Strasse = Straße.Text;
                dbRow.Telefon = Telefon.Text;
                dbRow.Email = EMail.Text;
                dbRow.PersonendatenID = curCVID;
               
                dbcv.dbCon.Update(dbRow);
            }
        }


        void Next_Click(object sender, EventArgs e)
        {

            SaveData();
            dbcv.dbCon.Close();
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Schools(1, 1));
            App.Current.MainPage = mv;

        }
        void Prev_Click(object sender, EventArgs e)
        {
            SaveData();
            dbcv.dbCon.Close();
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new PersonalInfo2());
            App.Current.MainPage = mv;

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "Kontaktinformation";
                    Label1 = "Wohnort:";
                    Label2 = "PLZ:";
                    Label3 = "Straße:";
                    Label4 = "Telefonnr:";
                    Label5 = "E-Mail:";
                    break;
                case 2:
                    MainLabel = "Contact";
                    Label1 = "Place of residence:";
                    Label2 = "Post code:";
                    Label3 = "Street:";
                    Label4 = "Telephone:";
                    Label5 = "E-Mail:";
                    break;
                case 4:
                    MainLabel = "Contact";
                    Label1 = "Domicile:";
                    Label2 = "Code postal:";
                    Label3 = "Rue:";
                    Label4 = "numéro de telephone:";
                    Label5 = "émail:";
                    break;
                case 3:
                    MainLabel = "معلومات الإتصال";
                    Label1 = "مكان الإقامة:";
                    Label2 = "الرمز البريدي:";
                    Label3 = "الشارع:";
                    Label4 = "الهاتف:";
                    Label5 = "البريد الإلكتروني:";
                    break;

                    
            }

        }
    }
}
