﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class Berufs : ContentPage
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

        BeruflicheTätigkeitTableItem dbRow;
        DBStorageCV dbcv;
        //Farben festlegen A.S.
        Color EntryBackColor = Color.FromRgb(80, 20, 20);
        Color EntryTextColor = Color.White;



        public Berufs(int MyCount, int MyPages)
        {
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);
            
            //UI Plattformabhängig konfigurieren A.S.
            Device.OnPlatform(iOS: () => SetUIiOS());

            dbcv = new DBStorageCV();
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<BeruflicheTätigkeitTableItem>().Where(v => v.PersonendatenID == curCVID);
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
            SetUIEntryiOS(Tätigkeit);
            SetUIEntryiOS(Arbeitgeber);
            SetUIEntryiOS(Ort);

            BVonDate.BackgroundColor = EntryBackColor;
            BVonDate.TextColor = EntryTextColor;

            BBisDate.BackgroundColor = EntryBackColor;
            BBisDate.TextColor = EntryTextColor;

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
            var query = dbcv.dbCon.Table<BeruflicheTätigkeitTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            if (query.Count() > 0)
            {
                dbRow = query.First();
                Tätigkeit.Text = dbRow.Tätigkeit;
                BVonDate.Date = dbRow.Von;
                BBisDate.Date = dbRow.Bis;
                Arbeitgeber.Text = dbRow.Arbeitgeber;
                Ort.Text = dbRow.Ort;
            }
            else
            {
                BVonDate.Date = new DateTime(2000, 01, 01);
                BBisDate.Date = new DateTime(2000, 01, 01);

            }
        }

        //Daten der Seite speichern
        void SaveData()
        {
            int curCVID = dbcv.CurrentCVID;
            var query = dbcv.dbCon.Table<BeruflicheTätigkeitTableItem>().Where(v => v.PersonendatenID == curCVID & v.Index == MyCounter);
            if (query.Count() > 0)
            { dbRow = query.First(); }
            else
            {
                BeruflicheTätigkeitTableItem newDBRow = new BeruflicheTätigkeitTableItem();
                newDBRow.PersonendatenID = curCVID;
                newDBRow.Index = MyCounter;
                dbcv.dbCon.Insert(newDBRow);
                dbRow = newDBRow;
                SumPages += 1;
            };
            dbRow.Tätigkeit = Tätigkeit.Text;
            dbRow.Von = BVonDate.Date;
            dbRow.Bis = BBisDate.Date;
            dbRow.Arbeitgeber = Arbeitgeber.Text;
            dbRow.Ort = Ort.Text;
            dbcv.dbCon.Update(dbRow);
        }

        void Insert_Beruf(object sender, EventArgs e)
        {
            SaveData();
            MyCounter = SumPages + 1;
            SumPages += 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Berufs(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Del_Beruf(object sender, EventArgs e)
        {
            if (MyCounter == 1)
            {
                return;
            }
            SaveData();
            int NewCounter = MyCounter;
            var query = dbcv.dbCon.Table<BeruflicheTätigkeitTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == MyCounter);
            BeruflicheTätigkeitTableItem DelRow;
            DelRow = query.First();
            dbcv.dbCon.Delete<BeruflicheTätigkeitTableItem>(DelRow.ID);
            if (SumPages > MyCounter)
            {
                for (int i = 0; i < (SumPages - MyCounter); i++)
                {
                    NewCounter += 1;
                    var queryLoop = dbcv.dbCon.Table<BeruflicheTätigkeitTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == NewCounter);
                    DelRow = queryLoop.First();
                    DelRow.Index = NewCounter - 1;
                    dbcv.dbCon.Update(DelRow);
                }
            }
            MyCounter -= 1;
            SumPages -= 1;
            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new Berufs(MyCounter, SumPages));
            App.Current.MainPage = mv;
        }

        void Next_Click(object sender, EventArgs e)
        {
            SaveData();
            dbcv.dbCon.Close();

            MainPage mv = new MainPage();
            if (MyCounter == SumPages)
            {
                mv.Detail = new NavigationPage(new Qualifikation(1, 1));
            }
            else
            {
                MyCounter += 1;
                mv.Detail = new NavigationPage(new Berufs(MyCounter, SumPages));
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
                mv.Detail = new NavigationPage(new Ausbildung(1, 1));
            }
            else
            {
                MyCounter -= 1;
                mv.Detail = new NavigationPage(new Berufs(MyCounter, SumPages));
            };
            App.Current.MainPage = mv;

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "Berufliche Tätigkeit";
                    Label1 = "Tätigkeit:";
                    Label2 = "Von:";
                    Label3 = "Bis:";
                    Label4 = "Bei Firma:";
                    Label5 = "Ort/Land:";
                    break;
                case 2:
                    MainLabel = "Professional activities";
                    Label1 = "Activity:";
                    Label2 = "From:";
                    Label3 = "To:";
                    Label4 = "Company:";
                    Label5 = "Place/Country:";
                    break;
                case 4:
                    MainLabel = "Activité professionelle";
                    Label1 = "Activité:";
                    Label2 = "de:";
                    Label3 = "à:";
                    Label4 = "compagnie:";
                    Label5 = "Lieu/Pays:";
                    break;
                case 3:
                    MainLabel = "النشاط المهني";
                    Label1 = "المركز الوظيفي:";
                    Label2 = "من:";
                    Label3 = "إلى:";
                    Label4 = "الشركة:";
                    Label5 = "المدينة/البلد:";
                    break;


            }

        }
    }
}
