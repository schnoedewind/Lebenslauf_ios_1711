using System.Collections.Generic;
using Xamarin.Forms;

namespace Lebenslauf
{
	public partial class MasterPage : ContentPage
	{
      
        public ListView ListView { get { return listView; } }
        public string lstLabel1,lstLabel2,lstLabel3,lstLabel4,lstLabel5,lstLabel6,lstLabel7,lstLabel8;
		public MasterPage ()
		{
			InitializeComponent ();
            SetLabels();
            var masterPageItems = new List<MasterPageItem>();
            

            masterPageItems.Add(new MasterPageItem
            {

                Title = lstLabel1,
               
                TargetType = typeof(FirstPage)
            });
           
            masterPageItems.Add(new MasterPageItem
            {
                Title = lstLabel2,
               
                TargetType = typeof(LanguagePage)
            });

    //      deaktiviert für iOS
    //        masterPageItems.Add (new MasterPageItem {
				//Title = lstLabel3,
                
    //            TargetType = typeof(ShareApp)

    //        });

			masterPageItems.Add (new MasterPageItem {
				Title = lstLabel4,
				
				TargetType = typeof(PressPage)
			});
            masterPageItems.Add(new MasterPageItem
            {
                Title = lstLabel5,
               
                TargetType = typeof(PrivacyPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = lstLabel6,
               
                
                TargetType = typeof(HelpPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = lstLabel7,
                
                TargetType = typeof(AppFeedback)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = lstLabel8,
               
                TargetType = typeof(CreateDocFile)
            });

            listView.ItemsSource = masterPageItems;
		}
        public void SetLabels()
        {
            switch (GlobalClass.LanguagePrg)
            {
                case 1:
                    lstLabel1 = "Erste Seite";
                    lstLabel2 = "Sprache";
                    lstLabel3 = "App teilen";
                    lstLabel4 = "Impressum";
                    lstLabel5 = "Datenschutz";
                    lstLabel6 = "Hilfe";
                    lstLabel7 = "Feedback";
                    lstLabel8 = "Word Datei Erstellen";
                    break;
                case 2:
                    lstLabel1 = "First Page";
                    lstLabel2 = "Language";
                    lstLabel3 = "Share App";
                    lstLabel4 = "Imprint";
                    lstLabel5 = "Data Protection";
                    lstLabel6 = "Help";
                    lstLabel7 = "Feedback";
                    lstLabel8 = "Create Word File";
                    break;
                case 4:
                    lstLabel1 = "Première page";
                    lstLabel2 = "La langue";
                    lstLabel3 = "Partager l'application";
                    lstLabel4 = "Imprimer";
                    lstLabel5 = "Protection des données";
                    lstLabel6 = "Aidez-moi";
                    lstLabel7 = "Retour d'information";
                    lstLabel8 = "créer fichier Word";
                    break;
                case 3:
                    lstLabel1 = "صفحة البداية";
                    lstLabel2 = "اللغة";
                    lstLabel3 = "مشاركة التطبيق";
                    lstLabel4 = "المحررون";
                    lstLabel5 = "حماية البيانات";
                    lstLabel6 = "المساعدة";
                    lstLabel7 = "الإنطباع";
                    lstLabel8 = "انشاء ملف وورد";
                    break;
                
            }
        }
    }
    
}
