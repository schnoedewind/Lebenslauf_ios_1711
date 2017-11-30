using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class Computer : ContentPage
    {
        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        public string Label2 { get; private set; }
        public int MyCounter { get; private set; }
        public int SumPages { get; private set; }
        public string imagePath { get; private set; }

        ComputerKenntnisseTableItem dbRow;
        DBStorageCV dbcv;
        //Farben festlegen A.S.
        Color EntryBackColor = Color.FromRgb(80, 20, 20);
        Color EntryTextColor = Color.White;

        public Computer(int MyCount, int MyPages)
        {
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);

            //UI Plattformabhängig konfigurieren A.S.
            Device.OnPlatform(iOS: () => SetUIiOS());

            dbcv = new DBStorageCV();
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == curCVID);
            SumPages = query.Count();
            MyCounter = MyCount;
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
            SetUIEntryiOS(Betriebssystem);

            Level.BackgroundColor = EntryBackColor;
            Level.TextColor = EntryTextColor;

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
            var query = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            if (query.Count() > 0)
            {
                dbRow = query.First();
                Betriebssystem.Text = dbRow.BetriebssystemProgramm;
                Level.SelectedIndex = dbRow.ComputerKenntnissBezeichnungID;
            }
        }

        //Daten der Seite speichern
        void SaveData()
        {
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            if (query.Count() > 0)
            { dbRow = query.First(); }
            else
            {
                ComputerKenntnisseTableItem newDBRow = new ComputerKenntnisseTableItem();
                newDBRow.PersonendatenID = curCVID;
                newDBRow.Index = MyCounter;
                dbcv.dbCon.Insert(newDBRow);
                dbRow = newDBRow;
                SumPages += 1;
            };
            dbRow.BetriebssystemProgramm = Betriebssystem.Text;
            dbRow.ComputerKenntnissBezeichnungID = Level.SelectedIndex;

            dbcv.dbCon.Update(dbRow);
        }



        void Insert_IT(object sender, EventArgs e)
        {
            SaveData();
            MyCounter = SumPages + 1;
            SumPages += 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Computer(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Del_IT(object sender, EventArgs e)
        {
            if (MyCounter == 1)
            {
                return;
            }
            SaveData();
            int NewCounter = MyCounter;
            var query = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == MyCounter);
            ComputerKenntnisseTableItem DelRow;
            DelRow = query.First();
            dbcv.dbCon.Delete<ComputerKenntnisseTableItem>(DelRow.ID);
            if (SumPages > MyCounter)
            {
                for (int i = 0; i < (SumPages - MyCounter); i++)
                {
                    NewCounter += 1;
                    var queryLoop = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == NewCounter);
                    DelRow = queryLoop.First();
                    DelRow.Index = NewCounter - 1;
                    dbcv.dbCon.Update(DelRow);
                }
            }
            MyCounter -= 1;
            SumPages -= 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Computer(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Next_Click(object sender, EventArgs e)
        {
            SaveData();
            dbcv.dbCon.Close();
            MainPage mv = new MainPage();
            if (MyCounter == SumPages)
            {
                mv.Detail = new NavigationPage(new DriveLicsense(1,1));
            }
            else
            {
                MyCounter += 1;
                mv.Detail = new NavigationPage(new Computer(MyCounter, SumPages));
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
                mv.Detail = new NavigationPage(new Language(1, 1));
            }
            else
            {
                MyCounter -= 1;
                mv.Detail = new NavigationPage(new Computer(MyCounter, SumPages));
            };
            App.Current.MainPage = mv;

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "EDV";
                    Label1 = "Betriebssystem/Programm:";
                    Label2 = "Kenntnisse:";
                    break;
                case 2:
                    MainLabel = "Computing";
                    Label1 = "Operating system/Program:";
                    Label2 = "Knowledge:";
                    break;
                case 4:
                    MainLabel = "Informatique";
                    Label1 = "Système d’exploitation/programme:";
                    Label2 = "Connaissances:";
                    break;
                case 3:
                    MainLabel = "مهارات الكمبيوتر";
                    Label1 = "نظام التشغيل/البرنامج:";
                    Label2 = "المستوى:";
                    break;


            }

        }
    }
}