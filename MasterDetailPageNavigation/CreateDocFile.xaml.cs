using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace Lebenslauf
{
    public partial class CreateDocFile : ContentPage
    {
        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        PersonendatenTableItem pdRow;
        KontaktdatenTableItem pdRowContact;
        ImagesTableItem pdRowImage;
        DBStorageCV dbcv;
        SprachkenntnisseTableItem pdRowLanguage;
        AusbildungTableItem pdRowAusbildung;
        BeruflicheTaetigkeitTableItem pdRowBeruf;
        SchulbildungTableItem pdRowSchule;
        WeiterbildungTableItem pdRowWeiter;
        ComputerKenntnisseTableItem pdRowComputer;
        FreitextTableItem pdRowFreiText;
        DrivingLicenceTableItem pdRowLicense;

        const string DATEFORMAT = "dd.MM.yyyy";

        public CreateDocFile()
        {

            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);

            dbcv = new DBStorageCV();
            CheckLang();
            BindingContext = this;

        }


        void Next_Click(object sender, EventArgs e)
        {


            dbcv.dbCon.Close();


            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new FirstPage());
            App.Current.MainPage = mv;
        }

        void Prev_Click(object sender, EventArgs e)
        {


            dbcv.dbCon.Close();

            MainPage mv = new MainPage();
            mv.Detail = new NavigationPage(new MoreInfo(1, 1));
            App.Current.MainPage = mv;

        }
        //AS: Umgebaut für IOS 
        void OnButtonClicked(object sender, EventArgs e)
        {

            if (Device.OS == TargetPlatform.iOS)
            {
                FillDocIos(); //iOS Version
            }
            else
            {
                //Hier wird das Dokument in der Android-Version gefüllt

                //string MyFile = "";
                //pdRow = dbcv.dbCon.Get<PersonendatenTableItem>(dbcv.CurrentCVID);
                //MyFile = pdRow.NameCV + ".docx";
                //var query = dbcv.dbCon.Table<KontaktdatenTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
                //if (query.Count() > 0)
                //{
                //    pdRowContact = query.First();
                //}

                //var query1 = dbcv.dbCon.Table<ImagesTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
                //if (query1.Count() > 0)
                //{
                //    pdRowImage = query1.First();
                //}

                //Assembly assembly = typeof(App).GetTypeInfo().Assembly;
                //// Creating a new document.
                //WordDocument document = new WordDocument();
                //Stream inputStream = assembly.GetManifestResourceStream("Lebenslauf.Templates.Lebenslauf02.docx");
                ////Open Template document
                //document.Open(inputStream, FormatType.Word2013);
                //inputStream.Dispose();

                //MailMergeDataSet dataSet = new MailMergeDataSet();

                //MailMergeDataTable dataTable = new MailMergeDataTable("Persons", GetPerson());
                //dataSet.Add(dataTable);
                //document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(Person_Photo);
                //dataTable = new MailMergeDataTable("Works", GetWork());
                //dataSet.Add(dataTable);
                //dataTable = new MailMergeDataTable("Ausbildungs", GetAusbildung());
                //dataSet.Add(dataTable);
                //dataTable = new MailMergeDataTable("Schools", GetSchule());
                //dataSet.Add(dataTable);
                //dataTable = new MailMergeDataTable("Advances", GetAdvance());
                //dataSet.Add(dataTable);
                //dataTable = new MailMergeDataTable("Languages", GetLanguage());
                //dataSet.Add(dataTable);
                //dataTable = new MailMergeDataTable("Computers", GetComputer());
                //dataSet.Add(dataTable);
                //dataTable = new MailMergeDataTable("FreiTexts", GetFreiText());
                //dataSet.Add(dataTable);
                //dataTable = new MailMergeDataTable("DriveLicenses", GetDriveLicense());
                //dataSet.Add(dataTable);

                //List<DictionaryEntry> commands = new List<DictionaryEntry>();
                //DictionaryEntry entry = new DictionaryEntry("Persons", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("Works", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("Ausbildungs", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("Schools", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("Advances", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("Languages", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("Computers", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("FreiTexts", string.Empty);
                //commands.Add(entry);
                //entry = new DictionaryEntry("DriveLicenses", string.Empty);
                //commands.Add(entry);
                //document.MailMerge.ExecuteNestedGroup(dataSet, commands);

                //MemoryStream stream = new MemoryStream();
                //document.Save(stream, FormatType.Word2013);
                //document.Close();
                ////dbcv.dbCon.Close();
                //if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                //    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save(MyFile, "application/msword", stream);
                //else
                //    Xamarin.Forms.DependencyService.Get<ISave>().Save(MyFile, "application/msword", stream);
            }
        }

        //Berufliche Tätigkeiten
        public class Works
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Company { get; set; }
            public string Work { get; set; }
            public string Place { get; set; }
            public Works(DateTime from, DateTime to, string company, string work, string place)
            {
                From = from.ToString(DATEFORMAT);
                To = to.ToString(DATEFORMAT);
                Company = company;
                Work = work;
                Place = place;
            }
        }

        public List<Works> GetWorksList()
        {
            List<Works> Works = new List<Works>();
            var query = dbcv.dbCon.Query<BeruflicheTaetigkeitTableItem>("SELECT * FROM BeruflicheTätigkeit WHERE PersonendatenID = " + dbcv.CurrentCVID + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                if (s.Von != s.Bis )
                {
                    Works.Add(new Works(s.Von, s.Bis, s.Arbeitgeber, s.Taetigkeit, s.Ort));
                }
                
            }
            return Works;
        }

        //Ausbildung
        public class Ausbildung
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Institute { get; set; }
            public string Graduation { get; set; }
            public string Place { get; set; }

            public Ausbildung(DateTime from, DateTime to, string institute, string graduation, string place)
            {
                From = from.ToString(DATEFORMAT);
                To = to.ToString(DATEFORMAT);
                Institute = institute;
                Graduation = graduation;
                Place = place;
            }
        }


        public List<Ausbildung> GetAusbildungList()
        {
            List<Ausbildung> Ausbildungs = new List<Ausbildung>();
            string AbschlussString = "";
            string AusbildungString = "";

            var query = dbcv.dbCon.Query<AusbildungTableItem>("SELECT * FROM Ausbildung WHERE PersonendatenID = " + dbcv.CurrentCVID + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                if (String.IsNullOrEmpty(s.Abschluss))
                { }
                else
                { AbschlussString = "Abschluß: " + s.Abschluss; }
                if (String.IsNullOrEmpty(s.AtCompany))
                { AusbildungString = s.Ausbildung; }
                else
                { AusbildungString = s.Ausbildung + " bei " + s.AtCompany; }
                if (s.Von != s.Bis)
                {
                    Ausbildungs.Add(new Ausbildung(s.Von, s.Bis, AusbildungString, AbschlussString, s.Ort));
                }
            }
            return Ausbildungs;
        }

        //Schule
        public class Schule
        {
            public string From { get; set; }
            public string To { get; set; }
            public string School { get; set; }
            public string Place { get; set; }
            public string Country { get; set; }
            public string Graduation { get; set; }

            public Schule(DateTime from, DateTime to, string school, string place, string country, string graduation)
            {
                From = from.ToString(DATEFORMAT);
                To = to.ToString(DATEFORMAT);
                School = school;
                Place = place;
                Country = country;
                Graduation = graduation;
            }
        }

        public List<Schule> GetSchuleList()
        {
            List<Schule> Schools = new List<Schule>();
            string AbschlussString = "";
            var query = dbcv.dbCon.Query<SchulbildungTableItem>("SELECT * FROM Schulbildung WHERE PersonendatenID = " + dbcv.CurrentCVID + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                if (String.IsNullOrEmpty(s.Abschluss))
                { }
                else
                { AbschlussString = "Abschluß: " + s.Abschluss; }
                if (s.Von != s.Bis)
                {
                    Schools.Add(new Schule(s.Von, s.Bis, s.Schulname, s.Ort, s.Land, AbschlussString));
                }
            }
            return Schools;
        }

        //Advance
        public class Advance
        {
            public string From { get; set; }
            public string To { get; set; }
            public string AdvanceName { get; set; }
            public string AdvancePlace { get; set; }
            public string Place { get; set; }

            public Advance(DateTime from, DateTime to, string advanceName, string advancePlace, string place)
            {
                From = from.ToString(DATEFORMAT);
                To = to.ToString(DATEFORMAT);
                AdvanceName = advanceName;
                AdvancePlace = advancePlace;
                Place = place;
            }
        }

        List<Advance> GetAdvanceList()
        {
            List<Advance> Advances = new List<Advance>();
            var query = dbcv.dbCon.Query<WeiterbildungTableItem>("SELECT * FROM Weiterbildung WHERE PersonendatenID = " + dbcv.CurrentCVID + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                if (s.Von != s.Bis)
                {
                    Advances.Add(new Advance(s.Von, s.Bis, s.WeiterbildungName, s.WeiterbildungStelle, s.Ort));
                }
            }
            return Advances;
        }
 

        //Sprachen
        public class Sprache
        {
            public string Language { get; set; }
            public string Level { get; set; }

           public Sprache(string language, string level)
            {
                Language = language;
                Level = level;
            }
        }
        
        List<Sprache> GetLanguagesList()
        {

            List<Sprache> Languages = new List<Sprache>();
            var query = dbcv.dbCon.Table<SprachkenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
            string LevelString = "";

            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count() + 1; i++)
                {
                    var query1 = dbcv.dbCon.Table<SprachkenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowLanguage = query1.First();


                    switch (pdRowLanguage.Sprachlevel)
                    {
                        case 0:
                            LevelString = "Grundkenntnisse";
                            break;
                        case 1:
                            LevelString = "gute Grundkenntnisse";
                            break;
                        case 2:
                            LevelString = "konversationssicher";
                            break;
                        case 3:
                            LevelString = "fließend";
                            break;
                        case 4:
                            LevelString = "fließend in Wort und Schrift";
                            break;
                        case 5:
                            LevelString = "verhandlungssicher in Wort und Schrift";
                            break;
                        case 6:
                            LevelString = "Muttersprache";
                            break;
                    }

                    if (pdRowLanguage.Sprache != null)
                    {
                        Languages.Add(new Sprache(pdRowLanguage.Sprache, LevelString));
                    }                    
                }

            }

            return Languages;
        }


        //Führerschein
        public class Licence

        {
            public string DriveType { get; set; }

            public Licence(string drivetype)
            {
                DriveType = drivetype;
            }
        }

        List<Licence> GetLicenceList()
        {
            List<Licence> DriveLicenses = new List<Licence>();
            string DriveType = "";
            string DriveTypeAll = "";
            var query = dbcv.dbCon.Table<DrivingLicenceTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count() + 1; i++)
                {
                    var query1 = dbcv.dbCon.Table<DrivingLicenceTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowLicense = query1.First();

                    if (pdRowLicense.LicenseYes == 0)
                    {

                        switch (pdRowLicense.LicenseTextID)
                        {
                            case 0:
                                DriveType = "Klasse AM";
                                break;
                            case 1:
                                DriveType = "Klasse A1";
                                break;
                            case 2:
                                DriveType = "Klasse A2";
                                break;
                            case 3:
                                DriveType = "Klasse A";
                                break;
                            case 4:
                                DriveType = "Klasse B";
                                break;
                            case 5:
                                DriveType = "Klasse B96";
                                break;
                            case 6:
                                DriveType = "Klasse BE";
                                break;
                            case 7:
                                DriveType = "Klasse C1";
                                break;
                            case 8:
                                DriveType = "Klasse C1E";
                                break;
                            case 9:
                                DriveType = "Klasse C";
                                break;
                            case 10:
                                DriveType = "Klasse CE";
                                break;
                            case 11:
                                DriveType = "Klasse D1";
                                break;
                            case 12:
                                DriveType = "Klasse D1E";
                                break;
                            case 13:
                                DriveType = "Klasse D";
                                break;
                            case 14:
                                DriveType = "Klasse DE";
                                break;
                            case 15:
                                DriveType = "Klasse T";
                                break;
                            case 16:
                                DriveType = "Klasse L";
                                break;
                        }
                        if (i == 1)
                        {
                            DriveTypeAll = DriveType;
                        }
                        else
                        {
                            DriveTypeAll = DriveTypeAll + ", " + DriveType;
                        }

                    }
                }
                DriveLicenses.Add(new Licence(DriveTypeAll));
            }
            return DriveLicenses;
        }

        //Computerkentnisse
        public class ComputerKentnisse
        {            
            public string Comp { get; set; }
            public string Level { get; set; }
            public ComputerKentnisse(string comp, string level)
            {
                Comp = comp;
                Level = level;
            }
        }


        private List<ComputerKentnisse> GetComputerList()
        {
            List<ComputerKentnisse> ComputerL = new List<ComputerKentnisse>();
            var query = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
            string LevelString = "";
            string CompK = "";
            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count() + 1; i++)
                {
                    var query1 = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowComputer = query1.First();
                    CompK = pdRowComputer.BetriebssystemProgramm;

                    switch (pdRowComputer.ComputerKenntnissBezeichnungID)
                    {
                        case 0:
                            LevelString = "Grundkenntnisse";
                            break;
                        case 1:
                            LevelString = "gute kenntnisse";
                            break;
                        case 2:
                            LevelString = "Sehr gute Kenntnisse";
                            break;
                    }

                    if (CompK != null)
                    {
                        ComputerL.Add(new ComputerKentnisse(CompK, LevelString));
                    }
                    
                }
            }
            return ComputerL;

        }

        //Sonstiges 
        public class Sonstiges
        {
            public string Freitext { get; set; }            
            public Sonstiges(string freitext)
            {
                Freitext = freitext;                
            }
        }


        private List<Sonstiges> GetSonstigesList()
        {
            List<Sonstiges> FreiTexts = new List<Sonstiges>();
            var query = dbcv.dbCon.Table<FreitextTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);

            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count() + 1; i++)
                {
                    var query1 = dbcv.dbCon.Table<FreitextTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowFreiText = query1.First();
                    FreiTexts.Add(new Sonstiges(pdRowFreiText.Freitext));
                }
            }
            return FreiTexts;
        }



        public void FillDocIos ()
        {
            //string MyFile = "";
            //try
            //{
            //    pdRow = dbcv.dbCon.Get<PersonendatenTableItem>(dbcv.CurrentCVID);
            //}
            //catch (Exception)
            //{
            //    return;
            //    //throw;
            //}
            
            //MyFile = pdRow.NameCV + ".docx";
            //var query = dbcv.dbCon.Table<KontaktdatenTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
            //if (query.Count() > 0)
            //{
            //    pdRowContact = query.First();
            //}
       
            //var query1 = dbcv.dbCon.Table<ImagesTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
            //if (query1.Count() > 0)
            //{
            //    pdRowImage = query1.First();
            //}

            //Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            
            //// Creating a new document.
            ////TEST
            ////string resname;           
            ////foreach (string resourceName in assembly.GetManifestResourceNames())
            ////{
            ////    resname = resourceName;
            ////}

            //WordDocument document = new WordDocument();
            //Stream inputStream = assembly.GetManifestResourceStream("Lebenslauf.Templates.Lebenslauf02ios.docx");
            ////Open Template document
            //document.Open(inputStream, FormatType.Word2013);
            //inputStream.Dispose();

            ////Handler für das Foto (Android)
            ////document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(Person_Photo);

            ////FOTO ios
            //Stream imageStream;
            //byte[] bytes = dbcv.LoadImageDataFromDB();
            //if (bytes != null)
            //{
            //    var resizer = DependencyService.Get<IMediaService>();
            //    var resizedBytes = resizer.ResizeImage(bytes, 137, 140); //Ein Fehler in ResizeImage vertauscht die Höhe und Breite des Bildes, dies wird beim Einfügen in das Word Dokument ausgeglichen
            //    imageStream = new MemoryStream(resizedBytes);              
            //}
            //else
            //{
            //    //Assembly assembly = GetType().Assembly();
            //    imageStream = assembly.GetManifestResourceStream("Lebenslauf.Foto.png");
            //}
            
            //IWParagraph paragraph = document.LastParagraph;
            
            //WPicture picture = paragraph.AppendPicture(imageStream) as WPicture;                        

            //picture.TextWrappingStyle = TextWrappingStyle.InFrontOfText;

            //picture.VerticalOrigin = VerticalOrigin.Margin;          
            //picture.HorizontalOrigin = 0;
            
            //picture.VerticalPosition = 20;
            //picture.HorizontalPosition = 350;

            //// Korrektur eines Fehlers in der ResizeImage-Funktion. Bei dem Bild sind höhe und Breite vertauscht
            //float w = picture.Width;
            //float h = picture.Height;
            //picture.Width = h;
            //picture.Height = w;
            

            ////Skalieren
            ////float scale = (100 / h);
            ////picture.Width = w * scale;
            ////picture.Height = h * scale;


            //// w = picture.Width;
            //// h = picture.Height;


            ////picture.VerticalOrigin = VerticalOrigin.Margin;
            ////picture.HorizontalOrigin = 0;
            ////picture.VerticalPosition = 90;
            ////picture.HorizontalPosition = 350;
            ////picture.Width = 137;
            ////picture.Height = 140;

            ////picture.WidthScale = 10;
            ////picture.HeightScale = 20;

            ////picture.Width = picture.Height;
            ////picture.Height = picture.Width;


            ////Arbeit
            //List<Works> workslist = GetWorksList();
            //MailMergeDataTable dtw = new MailMergeDataTable("Works", workslist);
            //document.MailMerge.ExecuteNestedGroup(dtw);

            //////Ausbildung
            //List<Ausbildung> ausbildungslist = GetAusbildungList();
            //MailMergeDataTable dta = new MailMergeDataTable("Ausbildungs", ausbildungslist);
            //document.MailMerge.ExecuteNestedGroup(dta);

            //////Schule
            //List<Schule> schulelist = GetSchuleList();
            //MailMergeDataTable dts = new MailMergeDataTable("Schools", schulelist);
            //document.MailMerge.ExecuteNestedGroup(dts);

            //////Advances
            //List<Advance> advancelist = GetAdvanceList();
            //MailMergeDataTable dtad = new MailMergeDataTable("Advances", advancelist);
            //document.MailMerge.ExecuteNestedGroup(dtad);

            //////Führerschein
            //List<Licence> licencelist = GetLicenceList();
            //MailMergeDataTable dtl = new MailMergeDataTable("DriveLicenses", licencelist);
            //document.MailMerge.ExecuteNestedGroup(dtl);

            //////Sprachen
            //List<Sprache> sprachelist = GetLanguagesList();
            //MailMergeDataTable dtsp = new MailMergeDataTable("Languages", sprachelist);
            //document.MailMerge.ExecuteNestedGroup(dtsp);

            ////Sonstiges
            //List<Sonstiges> sonstigeslist = GetSonstigesList();
            //MailMergeDataTable dftxt = new MailMergeDataTable("FreiTexts", sonstigeslist);
            //document.MailMerge.ExecuteNestedGroup(dftxt);

            ////Computer
            //List<ComputerKentnisse> computerlist = GetComputerList();
            //MailMergeDataTable dcmp = new MailMergeDataTable("Computers", computerlist);
            //document.MailMerge.ExecuteNestedGroup(dcmp);



            ////Personendaten
            //string gebdat;
            //gebdat = pdRow.GebDat.ToString(DATEFORMAT);
            //string[] fieldNames = new string[] { "FirstName", "Lastname", "Nation", "BirDate", "BirCountry", "BirCity", "Society", "Child", "Street", "ZipCode", "City", "Tel", "Email", "Hobby" };
            //string[] fieldValues = new string[] { pdRow.Vorname, pdRow.Nachname, pdRow.Nationalitaet, gebdat, pdRow.GebLand, pdRow.GebOrt, pdRow.Familienstand, pdRow.Kinder, NullToString(pdRowContact.Strasse), NullToString(pdRowContact.PLZ), NullToString(pdRowContact.Ort), NullToString(pdRowContact.Telefon), NullToString(pdRowContact.Email), pdRow.Hobbies };
            //document.MailMerge.Execute(fieldNames, fieldValues);


            //MemoryStream stream = new MemoryStream();
            //document.Save(stream, FormatType.Word2013);
            //document.Close();
            ////dbcv.dbCon.Close();
            //if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            //    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save(MyFile, "application/msword", stream);
            //else
            //    Xamarin.Forms.DependencyService.Get<ISave>().Save(MyFile, "application/msword", stream);
        }


        //private void Person_Photo(object sender, MergeImageFieldEventArgs args)

        //{

        //    if (args.FieldName == "Foto")
        //    {
        //        byte[] bytes = dbcv.LoadImageDataFromDB();
        //        if (bytes != null)
        //        {

        //            var resizer = DependencyService.Get<IMediaService>();
        //            var resizedBytes = resizer.ResizeImage(bytes, 137, 140);
        //            Stream stream = new MemoryStream(resizedBytes);
        //            args.ImageStream = stream;
        //        }
        //        else
        //        {
        //            Assembly assembly = GetType().Assembly();
        //            Stream imageStream = assembly.GetManifestResourceStream("Lebenslauf.Foto.png");

        //            args.ImageStream = imageStream;
        //        }

        //    }
        //}

        private List<ExpandoObject> GetPerson()

        {

            List<ExpandoObject> Persons = new List<ExpandoObject>();
            Persons.Add(GetDynamicPerson("Foto.png",pdRow.Vorname, pdRow.Nachname,pdRow.Nationalitaet,pdRow.GebDat,pdRow.GebLand,pdRow.GebOrt,pdRow.Familienstand,pdRow.Kinder, NullToString(pdRowContact.Strasse), NullToString(pdRowContact.PLZ), NullToString(pdRowContact.Ort), NullToString(pdRowContact.Telefon), NullToString(pdRowContact.Email),pdRow.Hobbies));
            return Persons;
            
        }

       
        private List<ExpandoObject> GetLanguage()

        {

            List<ExpandoObject> Languages = new List<ExpandoObject>();
            var query = dbcv.dbCon.Table<SprachkenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID );
            string LevelString = "";

            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count()+1; i++)
                    {
                    var query1 = dbcv.dbCon.Table<SprachkenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowLanguage = query1.First();
                   
                   
                    switch (pdRowLanguage.Sprachlevel)
                    {
                        case 0:
                            LevelString = "Grundkenntnisse";
                            break;
                        case 1:
                            LevelString = "gute Grundkenntnisse";
                            break;
                        case 2:
                            LevelString = "konversationssicher";
                            break;
                        case 3:
                            LevelString = "fließend";
                            break;
                        case 4:
                            LevelString = "fließend in Wort und Schrift";
                            break;
                        case 5:
                            LevelString = "verhandlungssicher in Wort und Schrift";
                            break;
                        case 6:
                            LevelString = "Muttersprache";
                            break;
                    }
                    Languages.Add(GetDynamicLanguage(pdRowLanguage.Sprache, LevelString));
                    }
               
            }
            

            return Languages;

        }

        private List<ExpandoObject> GetDriveLicense()

        {
            List<ExpandoObject> DriveLicenses = new List<ExpandoObject>();
            string DriveType = "";
            string DriveTypeAll = "";
            var query = dbcv.dbCon.Table<DrivingLicenceTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count() + 1; i++)
                {
                    var query1 = dbcv.dbCon.Table<DrivingLicenceTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowLicense = query1.First();

                    if (pdRowLicense.LicenseYes == 0)
                    {

                        switch (pdRowLicense.LicenseTextID)
                        {
                            case 0:
                                DriveType = "Klasse AM";
                                break;
                            case 1:
                                DriveType = "Klasse A1";
                                break;
                            case 2:
                                DriveType = "Klasse A2";
                                break;
                            case 3:
                                DriveType = "Klasse A";
                                break;
                            case 4:
                                DriveType = "Klasse B";
                                break;
                            case 5:
                                DriveType = "Klasse B96";
                                break;
                            case 6:
                                DriveType = "Klasse BE";
                                break;
                            case 7:
                                DriveType = "Klasse C1";
                                break;
                            case 8:
                                DriveType = "Klasse C1E";
                                break;
                            case 9:
                                DriveType = "Klasse C";
                                break;
                            case 10:
                                DriveType = "Klasse CE";
                                break;
                            case 11:
                                DriveType = "Klasse D1";
                                break;
                            case 12:
                                DriveType = "Klasse D1E";
                                break;
                            case 13:
                                DriveType = "Klasse D";
                                break;
                            case 14:
                                DriveType = "Klasse DE";
                                break;
                            case 15:
                                DriveType = "Klasse T";
                                break;
                            case 16:
                                DriveType = "Klasse L";
                                break;
                        }
                        if (i==1)
                        {
                            DriveTypeAll = DriveType;
                        }
                        else
                        {
                            DriveTypeAll = DriveTypeAll + ", " + DriveType;
                        }
                       
                    }
                }
                DriveLicenses.Add(GetDynamicDrive(DriveTypeAll));
            }
           

            return DriveLicenses;
        }

        private List<ExpandoObject> GetComputer()

        {

            List<ExpandoObject> Computers = new List<ExpandoObject>();
            var query = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);
            string LevelString = "";

            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count() + 1; i++)
                {
                    var query1 = dbcv.dbCon.Table<ComputerKenntnisseTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowComputer = query1.First();
                    switch (pdRowComputer.ComputerKenntnissBezeichnungID)
                    {
                        case 0:
                            LevelString = "Grundkenntnisse";
                            break;
                        case 1:
                            LevelString = "gute kenntnisse";
                            break;
                        case 2:
                            LevelString = "Sehr gute Kenntnisse";
                            break;
                    }
                    Computers.Add(GetDynamicComputer(pdRowComputer.BetriebssystemProgramm, LevelString));
                }

            }


            return Computers;

        }

        private List<ExpandoObject> GetFreiText()

        {

            List<ExpandoObject> FreiTexts = new List<ExpandoObject>();
            var query = dbcv.dbCon.Table<FreitextTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID);

            if (query.Count() > 0)
            {
                for (int i = 1; i < query.Count() + 1; i++)
                {
                    var query1 = dbcv.dbCon.Table<FreitextTableItem>().Where(v => v.PersonendatenID == dbcv.CurrentCVID & v.Index == i);

                    pdRowFreiText = query1.First();
                    FreiTexts.Add(GetDynamicFreiText(pdRowFreiText.Freitext));
                }

            }


            return FreiTexts;

        }

        private List<ExpandoObject> GetWork()

        {

            List<ExpandoObject> Works = new List<ExpandoObject>();

            var query = dbcv.dbCon.Query<BeruflicheTaetigkeitTableItem>("SELECT * FROM BeruflicheTätigkeit WHERE PersonendatenID = " + dbcv.CurrentCVID  + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                Works.Add(GetDynamicWork(s.Von, s.Bis, s.Arbeitgeber, s.Taetigkeit, s.Ort));
            }

            return Works;

        }

        private List<ExpandoObject> GetAusbildung()

        {

            List<ExpandoObject> Ausbildungs = new List<ExpandoObject>();
            string AbschlussString = "";
            string AusbildungString = "";

            var query = dbcv.dbCon.Query<AusbildungTableItem>("SELECT * FROM Ausbildung WHERE PersonendatenID = " + dbcv.CurrentCVID + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                if (String.IsNullOrEmpty(s.Abschluss))
                { }
                else
                { AbschlussString = "Abschluß: " + s.Abschluss; }
                if (String.IsNullOrEmpty(s.AtCompany))
                { AusbildungString = s.Ausbildung; }
                else
                { AusbildungString = s.Ausbildung + " bei " + s.AtCompany; }

                Ausbildungs.Add(GetDynamicAusbildung(s.Von, s.Bis, AusbildungString, AbschlussString, s.Ort));
            }

            return Ausbildungs;
        }


        private List<ExpandoObject> GetSchule()

        {
            List<ExpandoObject> Schools = new List<ExpandoObject>();
            string AbschlussString = "";
            var query = dbcv.dbCon.Query<SchulbildungTableItem>("SELECT * FROM Schulbildung WHERE PersonendatenID = " + dbcv.CurrentCVID + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                if (String.IsNullOrEmpty(s.Abschluss))
                { }
                else
                { AbschlussString = "Abschluß: " + s.Abschluss; }
                Schools.Add(GetDynamicSchule(s.Von, s.Bis, s.Schulname, s.Ort, s.Land, AbschlussString));
            }
            return Schools;
        }

        private List<ExpandoObject> GetAdvance()

        {

            List<ExpandoObject> Advances = new List<ExpandoObject>();
            var query = dbcv.dbCon.Query<WeiterbildungTableItem>("SELECT * FROM Weiterbildung WHERE PersonendatenID = " + dbcv.CurrentCVID + " Order By [Bis] DESC");
            foreach (var s in query)
            {
                Advances.Add(GetDynamicAdvance(s.Von, s.Bis, s.WeiterbildungName, s.WeiterbildungStelle, s.Ort));
            }

            return Advances;

        }

        private dynamic GetDynamicPerson(string PersonPhoto, string FirstName, string Lastname,string Nation,DateTime BirDate,string BirCountry,string BirCity,string Society,string Child,string Street,string ZipCode,string City,string Tel,string Email,string Hobby )

        {

            dynamic dynamicPerson = new ExpandoObject(); //TODO A.S.: "dynamic" wird von iOS nicht unterstützt

            //A.S.: iOS-Version stürzt an dieser Stelle ab, bei der Zuweisung eines Wertes an einen Parameter von dynamicPerson
            //Fehlermeldung: "System.ArgumentNullException: Value cannot be null. Parameter name: method"
            //Ursache gefunden, iOS akzeptiert keine dynamsische Code-Erzeugung, siehe:
            //https://forums.xamarin.com/discussion/53941/expandoobject-crashing-ios
            //https://developer.xamarin.com/guides/ios/advanced_topics/limitations/
            //Das ExpandoObject wird unter iOS nicht funktionieren!

            dynamicPerson.Foto = PersonPhoto; 
            dynamicPerson.FirstName = FirstName;
            dynamicPerson.Lastname = Lastname;
            dynamicPerson.Nation = Nation;
            dynamicPerson.BirDate = BirDate.ToString(DATEFORMAT, CultureInfo.InvariantCulture);
            dynamicPerson.BirCountry = BirCountry;
            dynamicPerson.BirCity = BirCity;
            dynamicPerson.Society = Society;
            dynamicPerson.Child = Child;
            dynamicPerson.Street = Street;
            dynamicPerson.ZipCode = ZipCode;
            dynamicPerson.City = City;
            dynamicPerson.Tel = Tel;
            dynamicPerson.Email = Email;
            dynamicPerson.Hobby = Hobby;



            return dynamicPerson;

        }




        private dynamic GetDynamicAusbildung(DateTime From,  DateTime To,string Institute,string Graduation,string Place)

        {

            dynamic dynamicAusbildung = new ExpandoObject();
            dynamicAusbildung.From = From.ToString(DATEFORMAT, CultureInfo.InvariantCulture);
            dynamicAusbildung.To = To.ToString(DATEFORMAT, CultureInfo.InvariantCulture);
            dynamicAusbildung.Institute = Institute;
            dynamicAusbildung.Graduation = Graduation;
            dynamicAusbildung.Place = Place;
            return dynamicAusbildung;

        }




        private dynamic GetDynamicSchule(DateTime From, DateTime To, string School, string Place, string Country, string Graduation)

        {

            dynamic dynamicSchule = new ExpandoObject();

            dynamicSchule.From = From.ToString(DATEFORMAT, CultureInfo.InvariantCulture);

            dynamicSchule.To = To.ToString(DATEFORMAT, CultureInfo.InvariantCulture);

            dynamicSchule.School = School;
            dynamicSchule.Place = Place;
            dynamicSchule.Country = Country;
            dynamicSchule.Graduation = Graduation;



            return dynamicSchule;

        }


        private dynamic GetDynamicAdvance(DateTime From, DateTime To, string AdvanceName, string AdvancePlace, string Place)

        {

            dynamic dynamicAdvance = new ExpandoObject();

            dynamicAdvance.From = From.ToString(DATEFORMAT, CultureInfo.InvariantCulture);

            dynamicAdvance.To = To.ToString(DATEFORMAT, CultureInfo.InvariantCulture);

            dynamicAdvance.AdvanceName = AdvanceName;
            dynamicAdvance.AdvancePlace = AdvancePlace;
            dynamicAdvance.Place = Place;



            return dynamicAdvance;

        }



        private dynamic GetDynamicLanguage(string Lang, string Level)

        {

            dynamic dynamicLanguage = new ExpandoObject();
            
            dynamicLanguage.Language = Lang;

            dynamicLanguage.Level = Level;

            return dynamicLanguage;

        }


        private dynamic GetDynamicDrive( string DriveType)

        {

            dynamic dynamicDrive = new ExpandoObject();


            dynamicDrive.DriveType = DriveType;

            return dynamicDrive;

        }

        private dynamic GetDynamicFreiText(string FreiText)

        {

            dynamic dynamicFreiText = new ExpandoObject();


            dynamicFreiText.FreiText = FreiText;

            return dynamicFreiText;

        }

        private dynamic GetDynamicComputer(string Computer, String Level)

        {

            dynamic dynamicComputer = new ExpandoObject();


            dynamicComputer.Comp = Computer;

            dynamicComputer.Level = Level;

            return dynamicComputer;

        }


        private dynamic GetDynamicWork(DateTime From, DateTime To,string Company,string Work,string Place)

        {

            dynamic dynamicWork = new ExpandoObject();


            dynamicWork.From = From.ToString(DATEFORMAT, CultureInfo.InvariantCulture);

            dynamicWork.To = To.ToString(DATEFORMAT, CultureInfo.InvariantCulture);

            dynamicWork.Company = Company;
            dynamicWork.Work = Work;
            dynamicWork.Place = Place;

            return dynamicWork;

        }


static string NullToString(object Value)
        {
          
            return Value == null ? "" : Value.ToString();

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "Word Datei erstellen";
                    Label1 = "Word Datei erstellen";
                    break;
                case 2:
                    MainLabel = "Create Word file";
                    Label1 = "Create Word file";
                    break;
                case 4:
                    MainLabel = "créer fichier Word";
                    Label1 = "créer fichier Word";
                    break;
                case 3:
                    MainLabel = "انشاء ملف وورد";
                    Label1 = "انشاء ملف وورد";
                    break;


            }

        }

    }
}