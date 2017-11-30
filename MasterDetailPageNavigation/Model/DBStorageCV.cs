using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SQLite;
using Xamarin.Forms;
using Plugin.Media.Abstractions;

namespace Lebenslauf
{
    //Tabellendefinitionen 

    [Table("Personendaten")]
    public class PersonendatenTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public string Anrede { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime GebDat { get; set; }
        public string GebLand { get; set; }
        public string Familienstand { get; set; }
        public string Nationalitaet { get; set; }
        public string Kinder { get; set; }
        public string Hobbies { get; set; }
        public string Fuehrerschein { get; set; }
        public string NameCV { get; set; }
        public string GebOrt { get; set; }
    };

    [Table("Images")]
    public class ImagesTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten            
        public string ImageDataStringBase64 { get; set; }
    }

    [Table("Kontaktdaten")]
    public class KontaktdatenTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten
        public string PLZ { get; set; }
        public string Ort { get; set; }
        public string Strasse { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
    };

    [Table("Schulbildung")]
    public class SchulbildungTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten
        public int Index { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public string Schulname { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Abschluss { get; set; }
    };

    //Ausbildung/Studium
    [Table("Ausbildung")]
    public class AusbildungTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten
        public int Index { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public string Ausbildung { get; set; }
        public string AtCompany { get; set; }
        public string Abschluss { get; set; }
        public string Ort { get; set; }
    };

    [Table("BeruflicheTaetigkeit")]
    public class BeruflicheTaetigkeitTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten
        public int Index { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public string Arbeitgeber { get; set; }
        public string Taetigkeit { get; set; }
        public string Ort { get; set; }
    };

    [Table("Weiterbildung")]
    public class WeiterbildungTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten
        public int Index { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public string WeiterbildungName { get; set; }
        public string WeiterbildungStelle { get; set; }
        public string Ort { get; set; }
    };

    [Table("Sprachkenntnisse")]
    public class SprachkenntnisseTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten        
        public int Index { get; set; }
        public string Sprache { get; set; }
        public int Sprachlevel { get; set; }
    };

   

    [Table("ComputerKenntnisse")]
    public class ComputerKenntnisseTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten        
        public int Index { get; set; }
        //public string ComputerKenntnissBezeichnung { get; set; }
        public int ComputerKenntnissBezeichnungID { get; set; }
        public string BetriebssystemProgramm { get; set; }
        //public string Qualifikation { get; set; }
    };

    [Table("DrivingLicense")]
    public class DrivingLicenceTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten   
        public int Index { get; set; }
        public int LicenseYes { get; set; }
        public string LicenseText { get; set; }
        public int LicenseTextID { get; set; }

    };

    [Table("Freitext")]
    public class FreitextTableItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public int PersonendatenID { get; set; } //ForeignKey Personendaten        
        public int Index { get; set; }
        public string Freitext { get; set; }
    };

    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }

    public class DBStorageCV
    {
        //public string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatabaseCV.db3");
        //// public SQLiteConnection MyDB = new SQLiteConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatabaseCV.db3"));
        //public SQLiteConnection MyDB = new SQLiteConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatabaseCV.db3"));

        private static int currentCVID;
        public SQLiteConnection dbCon;

        //Constructor   
        public DBStorageCV()
        {
            dbCon = DependencyService.Get<ISQLite>().GetConnection();
        }

        //Destructor
        ~DBStorageCV()
        {
            //dbCon.Close();
            dbCon.Dispose();
        }

        public int CurrentCVID
        {
            get
            {
                return currentCVID;
            }

            set
            {
                currentCVID = value;
            }
        }


        public void PrepareDB()
        {
            dbCon.CreateTable<PersonendatenTableItem>();
            dbCon.CreateTable<KontaktdatenTableItem>();
            dbCon.CreateTable<SchulbildungTableItem>();
            dbCon.CreateTable<ImagesTableItem>();
            dbCon.CreateTable<AusbildungTableItem>();
            dbCon.CreateTable<BeruflicheTaetigkeitTableItem>();
            dbCon.CreateTable<WeiterbildungTableItem>();
            dbCon.CreateTable<SprachkenntnisseTableItem>();
            dbCon.CreateTable<ComputerKenntnisseTableItem>();
            dbCon.CreateTable<DrivingLicenceTableItem>();
            dbCon.CreateTable<FreitextTableItem>();
        }

        public void InitDB()
        {

            if (dbCon.Table<PersonendatenTableItem>().Count() == 0)
            {
                AddNewCV();
            }
        }

        public void AddNewCV()
        {
            //Personendaten
            var pditem = new PersonendatenTableItem();
            dbCon.Insert(pditem);
            CurrentCVID = pditem.ID;
            //Kontaktdaten                     
            var kditem = new KontaktdatenTableItem();
            kditem.PersonendatenID = CurrentCVID;
            dbCon.Insert(kditem);

            //Schulbildung    
            //var sbitem = new SchulbildungTableItem();
            //sbitem.PersonendatenID = CurrentCVID;
            //sbitem.Index = 1;
            //dbCon.Insert(sbitem);
            ////Ausbildung
            //var abitem = new AusbildungTableItem();
            //abitem.PersonendatenID = CurrentCVID;
            //abitem.Index = 1;
            //Bilder
            var imgitem = new ImagesTableItem();
            imgitem.PersonendatenID = CurrentCVID;
            dbCon.Insert(imgitem);
        }


        public List<PersonendatenTableItem> GetAllCV()
        {
            return dbCon.Query<PersonendatenTableItem>("Select * From [Personendaten]");
        }

        public void DeleteFromList(String LocalStr)
        {
            // return dbCon.Delete(aCV);
            dbCon.Query<PersonendatenTableItem>("Delete From [Personendaten] WHERE NameCV= '" + LocalStr + "'");
        }

        public void DeleteFromListWithID(int LocalStr)
        {
            // return dbCon.Delete(aCV);
            var query = dbCon.Delete<PersonendatenTableItem>(LocalStr);

            
        }


        public void SaveImageFileToDB(MediaFile file)
        {
            //Save to DB
            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                file.Dispose();
                SaveImageDataToDB(memoryStream.ToArray());
            }
        }


        private Image LoadImageFromDB()
        {
            Image image = new Image();
            var byteArray = LoadImageDataFromDB();
            // Stream stream = new MemoryStream(byteArray);
            image.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
            return image;
        }

        private void SaveImageDataToDB(byte[] img)
        {
            ImagesTableItem it = new ImagesTableItem();
            it.ImageDataStringBase64 = Convert.ToBase64String(img);
            it.PersonendatenID = CurrentCVID;
            // dbCon.Update(it);
            SQLiteCommand cmd = dbCon.CreateCommand("Update [Images] set ImageDataStringBase64 = ? where PersonendatenID = " + Convert.ToString(CurrentCVID), it.ImageDataStringBase64);
            cmd.ExecuteNonQuery();

        }

        public byte[] LoadImageDataFromDB()
        {
            ImagesTableItem it = new ImagesTableItem();
            SQLiteCommand cmd = dbCon.CreateCommand("Select * from [Images] where PersonendatenID = " + Convert.ToString(CurrentCVID), it.ImageDataStringBase64);
            it = cmd.ExecuteQuery<ImagesTableItem>().FirstOrDefault();
            if (it.ImageDataStringBase64 == null) { return null; }
            else
            { return Convert.FromBase64String(it.ImageDataStringBase64); }
        }

        public byte[] LoadImageData(int CurID)
        {
            ImagesTableItem it = new ImagesTableItem();

            SQLiteCommand cmd = dbCon.CreateCommand("Select * from [Images] where PersonendatenID = " + Convert.ToString(CurID), it.ImageDataStringBase64);
            
            it = cmd.ExecuteQuery<ImagesTableItem>().FirstOrDefault();

            if (it.ImageDataStringBase64 == null) { return null; }
            else
            { return  Convert.FromBase64String(it.ImageDataStringBase64); }
        }

    };
}