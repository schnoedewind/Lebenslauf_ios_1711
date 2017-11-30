using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class Schools : ContentPage
    {
        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        public string Label2 { get; private set; }
        public string Label3 { get; private set; }
        public string Label4 { get; private set; }
        public string Label5 { get; private set; }
        public int MyCounter { get; private set; }
        public int SumPages { get; private set; }
        public string imagePath { get; private set; }
        SchulbildungTableItem dbRow;
        DBStorageCV dbcv;
        //Farben festlegen A.S.
        Color EntryBackColor = Color.FromRgb(80, 20, 20);
        Color EntryTextColor = Color.White;

        public Schools(int MyCount, int MyPages)
        {
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);
            
            //UI Plattformabhängig konfigurieren A.S.
            Device.OnPlatform(iOS: () => SetUIiOS());

            dbcv = new DBStorageCV();
            int curCVID = dbcv.CurrentCVID;
            MyCounter = MyCount;
            var query = dbcv.dbCon.Table<SchulbildungTableItem>().Where(v => v.PersonendatenID == curCVID);
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
            SetUIEntryiOS(Schulname);
            SetUIEntryiOS(Ort);
            SetUIEntryiOS(Land);

            SVonDate.BackgroundColor = EntryBackColor;
            SVonDate.TextColor = EntryTextColor;

            SBisDate.BackgroundColor = EntryBackColor;
            SBisDate.TextColor = EntryTextColor;
        }

        void SetUIEntryiOS(Entry ent)
        {
            ent.BackgroundColor = EntryBackColor;
            ent.TextColor = EntryTextColor;
        }


        //Daten speichern
        void LoadData()
        {
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<SchulbildungTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            if (query.Count() > 0)
            {
                dbRow = query.First();
                Schulname.Text = dbRow.Schulname;
                SVonDate.Date = dbRow.Von;
                SBisDate.Date = dbRow.Bis;
                Ort.Text = dbRow.Ort;
                Land.Text = dbRow.Land;
                

            }
            else
            {
                SVonDate.Date = new DateTime(2000, 01, 01);
                SBisDate.Date = new DateTime(2000, 01, 01);
               
            }
        }



        //Daten der Seite speichern
        void SaveData()
        {
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<SchulbildungTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            
            if (query.Count() > 0)
            { dbRow = query.First(); }
            else
            {
                SchulbildungTableItem newDBRow = new SchulbildungTableItem();
                newDBRow.PersonendatenID = curCVID;
                newDBRow.Index = MyCounter;
                dbcv.dbCon.Insert(newDBRow);
                dbRow = newDBRow;
                SumPages += 1;
            };

            dbRow.Schulname = Schulname.Text;
            dbRow.Von = SVonDate.Date;
            dbRow.Bis = SBisDate.Date;
            dbRow.Ort = Ort.Text;
            dbRow.Land = Land.Text;
            dbcv.dbCon.Update(dbRow);
        }

        void Insert_Schools(object sender, EventArgs e)
        {
            SaveData();

            MyCounter = SumPages + 1;
            SumPages += 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Schools(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Del_Schools(object sender, EventArgs e)
        {
            if (MyCounter == 1)
            {
                return;
            }
            SaveData();
            int NewCounter = MyCounter;
            var query = dbcv.dbCon.Table<SchulbildungTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == MyCounter);
            SchulbildungTableItem DelRow;
            DelRow = query.First();
            dbcv.dbCon.Delete<SchulbildungTableItem>(DelRow.ID);
            if (SumPages > MyCounter)
            {
                for (int i = 0; i < (SumPages-MyCounter); i++)
                {
                    NewCounter += 1;
                    var queryLoop = dbcv.dbCon.Table<SchulbildungTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == NewCounter);
                    DelRow = queryLoop.First();
                    DelRow.Index = NewCounter - 1;
                    dbcv.dbCon.Update(DelRow);
                }
            }           
            MyCounter -= 1;
            SumPages -= 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Schools(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Next_Click(object sender, EventArgs e)
        {
            // new MainPage().Detail.Navigation.PushModalAsync(new PersonalInfo());
            // ((MasterDetailPage)Parent).IsPresented = false;
            //this.Navigation.PushAsync(new PersonalInfo());

            SaveData();
            dbcv.dbCon.Close();


            MainPage mv = new MainPage();
            if (MyCounter == SumPages)
            {
                mv.Detail = new NavigationPage(new Ausbildung(1, 1));
            }
            else
            {

                MyCounter += 1;

                mv.Detail = new NavigationPage(new Schools(MyCounter, SumPages));
            };

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
            if (MyCounter == 1)
            {
                mv.Detail = new NavigationPage(new ContactInfo());
            }
            else
            {
                MyCounter -= 1;
                mv.Detail = new NavigationPage(new Schools(MyCounter, SumPages));
            };

            App.Current.MainPage = mv;

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "Schulbildung";
                    Label1 = "Schulname:";
                    Label2 = "Von:";
                    Label3 = "Bis:";
                    Label4 = "Ort:";
                    Label5 = "Land:";
                    break;
                case 2:
                    MainLabel = "Education";
                    Label1 = "Name of school:";
                    Label2 = "From:";
                    Label3 = "To:";
                    Label4 = "Place:";
                    Label5 = "Country:";
                    break;
                case 4:
                    MainLabel = "Éducation";
                    Label1 = "Nom de l’école:";
                    Label2 = "de:";
                    Label3 = "à:";
                    Label4 = "Lieu:";
                    Label5 = "Pays:";
                    break;
                case 3:
                    MainLabel = "الدراسة";
                    Label1 = "اسم المدرسة:";
                    Label2 = "من:";
                    Label3 = "إلى:";
                    Label4 = "المدينة:";
                    Label5 = "البلد:";
                    break;


            }

        }
    }
}
