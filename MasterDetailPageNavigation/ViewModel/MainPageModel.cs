using FreshMvvm;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lebenslauf
{
    [ImplementPropertyChanged]
    public class MainPageModel : FreshBasePageModel
    {
        public MainPageModel()
        {
           var mydb = new DBStorageCV();
            mydb.PrepareDB();
           // mydb.Testdb();


            Button1Text = "Lebenslauf erstellen";
            Vorname = "Peter";
        }

        protected override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            //CoreMethods.DisplayAlert("Page is appearing", "", "Ok");
            Button1Text = "Lebenslauf erstellen";
            Vorname = "Peter";

            base.ViewIsAppearing(sender, e);
        }

        private string button1Text;
        private string vorname;

        public string Button1Text
        {
            get
            {
                return button1Text;
            }

            set
            {
                button1Text = value;
            }
        }

        public string Vorname
        {
            get
            {
                return vorname;
            }

            set
            {
                vorname = value;
            }
        }
    }
}
