using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class Ausbildung : ContentPage
    {
        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        public string Label2 { get; private set; }
        public string Label3 { get; private set; }
        public string Label4 { get; private set; }
        public string Label5 { get; private set; }
        public string Label6 { get; private set; }
        public int MyCounter { get; private set; }
        public int SumPages { get; private set; }
        public string imagePath { get; private set; }

        AusbildungTableItem dbRow;
        DBStorageCV dbcv;
        //Farben festlegen A.S.
        Color EntryBackColor = Color.FromRgb(80, 20, 20);
        Color EntryTextColor = Color.White;

        public Ausbildung(int MyCount, int MyPages)
        {
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);

            //UI Plattformabhängig konfigurieren A.S.
            Device.OnPlatform(iOS: () => SetUIiOS());

            dbcv = new DBStorageCV();
            int curCVID = dbcv.CurrentCVID;
            MyCounter = MyCount;
            var query = dbcv.dbCon.Table<AusbildungTableItem>().Where(v => v.PersonendatenID == curCVID);
            SumPages = query.Count();
            if (MyCount > 1)
            {
                imagePath = "Del2.png";
            }
            else
            {
                imagePath = "del1.png";
            }
            CheckLang();
            BindingContext = this;
            LoadData();
        }

        //Textfarbe und Hintergrundfarbe für Eingabefelder setzen A.S.
        void SetUIiOS()
        {
            SetUIEntryiOS(AusbildungName);
            SetUIEntryiOS(Abschluss);
            SetUIEntryiOS(CompanyN);
            SetUIEntryiOS(Ort);
            
            AVonDate.BackgroundColor = EntryBackColor;
            AVonDate.TextColor = EntryTextColor;

            ABisDate.BackgroundColor = EntryBackColor;
            ABisDate.TextColor = EntryTextColor;
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
            var query = dbcv.dbCon.Table<AusbildungTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            if (query.Count() > 0)
            {
                dbRow = query.First();
                AusbildungName.Text = dbRow.Ausbildung;
                AVonDate.Date = dbRow.Von;
                ABisDate.Date = dbRow.Bis;
                CompanyN.Text = dbRow.AtCompany;
                Abschluss.Text = dbRow.Abschluss;
                Ort.Text = dbRow.Ort;
            }
            else
            {
                AVonDate.Date = new DateTime(2000, 01, 01);
                ABisDate.Date = new DateTime(2000, 01, 01);

            }
        }

        //Daten der Seite speichern
        void SaveData()
        {
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<AusbildungTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            if (query.Count() > 0)
            { dbRow = query.First(); }
            else
            {
                AusbildungTableItem newDBRow = new AusbildungTableItem();
                newDBRow.PersonendatenID = curCVID;
                newDBRow.Index = MyCounter;
                dbcv.dbCon.Insert(newDBRow);
                dbRow = newDBRow;
                SumPages += 1;
            };
            dbRow.Ausbildung = AusbildungName.Text;
            dbRow.Von = AVonDate.Date;
            dbRow.Bis = ABisDate.Date;
            dbRow.AtCompany = CompanyN.Text;
            dbRow.Abschluss = Abschluss.Text;
            dbRow.Ort = Ort.Text;

            dbcv.dbCon.Update(dbRow);
        }

        void Insert_Ausbildung(object sender, EventArgs e)
        {
            SaveData();
            MyCounter = SumPages + 1;
            SumPages += 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Ausbildung(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Del_Ausbildung(object sender, EventArgs e)
        {
            if (MyCounter==1)
            {
                return;
            }
            SaveData();
            int NewCounter = MyCounter;
            var query = dbcv.dbCon.Table<AusbildungTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == MyCounter);
            AusbildungTableItem DelRow;
            DelRow = query.First();
            dbcv.dbCon.Delete<AusbildungTableItem>(DelRow.ID);
            if (SumPages > MyCounter)
            {
                for (int i = 0; i < (SumPages - MyCounter); i++)
                {
                    NewCounter += 1;
                    var queryLoop = dbcv.dbCon.Table<AusbildungTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == NewCounter);
                    DelRow = queryLoop.First();
                    DelRow.Index = NewCounter - 1;
                    dbcv.dbCon.Update(DelRow);
                }
            }
            MyCounter -= 1;
            SumPages -= 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Ausbildung(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Next_Click(object sender, EventArgs e)
        {
            SaveData();
            dbcv.dbCon.Close();

            MainPage mv = new MainPage();
            if (MyCounter == SumPages)
            {
                mv.Detail = new NavigationPage(new Berufs(1, 1));
            }
            else
            {

                MyCounter += 1;

                mv.Detail = new NavigationPage(new Ausbildung(MyCounter, SumPages));
            };

            App.Current.MainPage = mv;
        }

        void Prev_Click(object sender, EventArgs e)
        {
            SaveData();
            dbcv.dbCon.Close();

            MainPage mv = new MainPage();
            if (MyCounter == 1)
            {
                mv.Detail = new NavigationPage(new Schools(1, 1));
            }
            else
            {
                MyCounter -= 1;
                mv.Detail = new NavigationPage(new Ausbildung(MyCounter, SumPages));
            };
            App.Current.MainPage = mv;

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "Ausbildung/Studium";
                    Label1 = "Ausbildung/Hochschule:";
                    Label2 = "Von:";
                    Label3 = "Bis:";
                    Label4 = "Abschluss:";
                    Label5 = "Ort/Land:";
                    Label6 = "Bei Firma:";
                    break;
                case 2:
                    MainLabel = "Education/Study";
                    Label1 = "Institute/University:";
                    Label2 = "From:";
                    Label3 = "To:";
                    Label4 = "Certificate:";
                    Label5 = "Place/Country:";
                    Label6 = "Company:";
                    break;
                case 4:
                    MainLabel = "Formation/études";
                    Label1 = "Institut / Université:";
                    Label2 = "de:";
                    Label3 = "à:";
                    Label4 = "Diplôme :";
                    Label5 = "Lieu/Pays:";
                    Label6 = "compagnie:";
                    break;
                case 3:
                    MainLabel = "الدراسات العليا";
                    Label1 = "المعهد/الجامعة:";
                    Label2 = "من:";
                    Label3 = "إلى:";
                    Label4 = "الاختصاص:";
                    Label5 = "المدينة/البلد:";
                    Label6 = "الشركة:";
                    break;


            }

        }
    }
}

