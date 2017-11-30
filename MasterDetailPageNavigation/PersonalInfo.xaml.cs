using Plugin.Media;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace Lebenslauf
{
    public partial class PersonalInfo : ContentPage
    {
        public string MainLabel { get; private set; }
        public string Label1 { get; private set; }
        public string Label2 { get; private set; }
        public string Label3 { get; private set; }
        public string Label4 { get; private set; }
        public string Label5 { get; private set; }
        public string Label6 { get; private set; }

        PersonendatenTableItem pdRow;
        DBStorageCV dbcv;
        //Farben festlegen A.S.
        Color EntryBackColor = Color.FromRgb(80, 20, 20);
        Color EntryTextColor = Color.White;

        public PersonalInfo()
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
            SetUIEntryiOS(Vorname);
            SetUIEntryiOS(Nachname);
            SetUIEntryiOS(Geburtsort);
            SetUIEntryiOS(Geburtsland);

            Anrede.BackgroundColor = EntryBackColor;
            Anrede.TextColor = EntryTextColor;

            BirthDate.BackgroundColor = EntryBackColor;
            BirthDate.TextColor = EntryTextColor;
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
                if (pdRow.Anrede == "Frau") { Anrede.SelectedIndex = 0; } else { Anrede.SelectedIndex = 1; };
                Vorname.Text = pdRow.Vorname;
                Nachname.Text = pdRow.Nachname;
                //DateTime gebdat;
                //if (DateTime.TryParse(pdRow.GebDat, out gebdat)) { BirthDate.Date = gebdat; }
                BirthDate.Date = pdRow.GebDat;
                Geburtsort.Text = pdRow.GebOrt;
                Geburtsland.Text = pdRow.GebLand;
                LoadMyImageFromDB();
         
           
        }

        public void LoadMyImageFromDB()
        {
            byte[] bytes = dbcv.LoadImageDataFromDB();
            if (bytes != null)
            {
                Stream stream = new MemoryStream(bytes);
                image.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
        }

        //Daten der Seite speichern
        void SaveData()
        {
            pdRow = dbcv.dbCon.Get<PersonendatenTableItem>(dbcv.CurrentCVID);
            if (Anrede.SelectedIndex == 0) { pdRow.Anrede = "Frau"; } else { pdRow.Anrede = "Herr"; };
            pdRow.Vorname = Vorname.Text;
            pdRow.Nachname = Nachname.Text;
            pdRow.GebDat = BirthDate.Date;
            pdRow.GebOrt = Geburtsort.Text;
            pdRow.GebLand = Geburtsland.Text;
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
            mv.Detail = new NavigationPage(new PersonalInfo2());
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
            mv.Detail = new NavigationPage(new NameOfCV());
            App.Current.MainPage = mv;

        }
        void OnButtonClicked(object sender, EventArgs e)
        {
            SaveData();
            
        }

      


        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            var imageSender = (Image)sender;
            
            var action = await DisplayActionSheet("Woher möchten Sie ein Foto wählen?", "Cancel", null, "Kamera", "Galerie");
            //await DisplayAlert("Information", "Action: " + action, "OK");
            if (action == "Galerie")
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Fotos werden nicht unterstützt", ":(Die Erlaubnis nicht erteilt für Fotos.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom
                });

                if (file == null)
                    return;
                
                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    dbcv.SaveImageFileToDB(file); //Speichern der Bilddaten aus der Datei in der DB
                    file.Dispose();
                    return stream;
                });
            }
            else if (action == "Kamera")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Keine Kamera", ":( Die Kamera ist nicht erreichtbar", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,                    
                    Directory = "Lebenslauf",
                    Name = "PersonPhoto.jpg"
                   
                });

                if (file == null)
                    return;

                // await DisplayAlert("Der Pfad der Datei", file.Path, "OK");

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    dbcv.SaveImageFileToDB(file);  //Speichern der Bilddaten aus der Datei in der DB                  
                    file.Dispose();
                    return stream;
                });
            }

        }

        void CheckLang()
        {
            int Lang = GlobalClass.LanguagePrg;
            switch (Lang)
            {
                case 1:
                    MainLabel = "Persönlichen Daten";
                    Label1 = "Anrede:";
                    Label2 = "Vorname:";
                    Label3 = "Nachname:";
                    Label4 = "Geburtsdatum:";
                    Label5 = "Geburtsort:";
                    Label6 = "Geburtsland:";
                    break;
                case 2:
                    MainLabel = "Personal Data";
                    Label1 = "Title:";
                    Label2 = "First Name:";
                    Label3 = "Surname:";
                    Label4 = "Birth date:";
                    Label5 = "Place of birth:";
                    Label6 = "Country of birth:";
                    break;
                case 4:
                    MainLabel = "Données personnelles";
                    Label1 = "Titre:";
                    Label2 = "Prénom:";
                    Label3 = "Nom de famille:";
                    Label4 = "Date de naissance:";
                    Label5 = "Lieu de naissance:";
                    Label6 = "Pays de naissance:";
                    break;
                case 3:
                    MainLabel = "المعلومات الشخصية";
                    Label1 = "اللقب:";
                    Label2 = "الاسم الأول:";
                    Label3 = "الكنية:";
                    Label4 = "تاريخ الولادة:";
                    Label5 = "مكان الولادة:";
                    Label6 = "بلد الولادة:";
                    break;


            }

        }

    }
}




