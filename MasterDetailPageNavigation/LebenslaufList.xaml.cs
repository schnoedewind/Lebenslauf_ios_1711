
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Lebenslauf
{
    public partial class LebenslaufList : ContentPage
    {
        private ObservableCollection<LebenslaufItems> LebenslaufItem { get; set; }
        DBStorageCV dbcv;
        Random r = new Random();
        public LebenslaufList()
        {
            LebenslaufItem = new ObservableCollection<LebenslaufItems>();
            InitializeComponent();
            GlobalClass.AppText(this.GetType().Name);

            dbcv = new DBStorageCV();
            LoadMyList();
            BindingContext = this;
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
          
            if (e.SelectedItem == null)
            {
                return;

            }
            var vSelUser = (LebenslaufItems)e.SelectedItem;

            dbcv.CurrentCVID = vSelUser.MainID;
            Navigation.PushAsync(new PersonalInfo());
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mSelCV = ((MenuItem)sender);
            String Mystr;
            Mystr = mSelCV.CommandParameter.ToString();
            bool accepted = await DisplayAlert("Bestätigen", "Sind Sie Sicher ? ", "Ja", "Nein");
            if (accepted)
            {

                dbcv.DeleteFromList(Mystr);
                
            }
            LebenslaufItem.Clear();
            InitializeComponent();
            LoadMyList();
            BindingContext = this;

            await Navigation.PushAsync(new LebenslaufList());

        }

       public void LoadMyList()
        {
            lstData.ItemsSource = LebenslaufItem;
            var query = dbcv.dbCon.Query<PersonendatenTableItem>("SELECT * FROM Personendaten");
            foreach (var s in query)
            {
                if (!(s.NameCV == null))
                {
                    byte[] bytes = dbcv.LoadImageData(s.ID);
                    //if (bytes != null)
                    //{                   
                    LebenslaufItem.Add(new LebenslaufItems() { MainID = s.ID, CVName = s.NameCV, Personimage = bytes });
                    //}
                }
                else
                {
                    dbcv.DeleteFromListWithID(s.ID);
                }
            }
        }


    }
}