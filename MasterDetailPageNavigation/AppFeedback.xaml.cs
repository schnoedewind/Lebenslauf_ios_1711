using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lebenslauf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppFeedback : ContentPage
    {
        public AppFeedback()
        {
            InitializeComponent();
        }
        void OnButtonClicked(object sender, EventArgs e)
        {
            var uri = "mailto:info@visibelle.de?subject=CV%20App%20Feedback&body=" + EmailBody.Text;
            Device.OpenUri(new Uri(uri));

        }
        }
}
