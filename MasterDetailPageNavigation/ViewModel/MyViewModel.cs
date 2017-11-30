using Lebenslauf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

public class ChangeCounter : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;



    int _mycounter;
    int _sumpages;
    DateTime _mydate;
    String manilabel,label1, label2, label3, label4, label5, label6;
    public int MyCounter
    {
        get { return _mycounter; }
        set
        {
            _mycounter = value;
            OnPropertyChanged();
        }
    }

    public int SumPages
    {
        get { return _sumpages; }
        set
        {
            _sumpages = value;
            
        }
    }

    public DateTime MyDate
    {
        get { return _mydate; }
        set
        {
            _mydate = value;
            OnPropertyChanged();
        }
    }
    private string imagePath;
    public string ImagePath
    {
        get { return imagePath; }
        set
        {
            imagePath = value;
            PropertyChanged(this, new PropertyChangedEventArgs("ImagePath"));
        }
    }

    private string helptext;
    public string HelpText
    {
        get { return helptext; }
        set
        {
            helptext = value;
            PropertyChanged(this, new PropertyChangedEventArgs("HelpText"));
        }
    }

    

    public string Label1
    {
        get { return label1; }
        set
        {
            label1 = value;
            PropertyChanged(this, new PropertyChangedEventArgs("Label1"));
        }
    }

    public string Label2
    {
        get { return label2; }
        set
        {
            label2 = value;
            PropertyChanged(this, new PropertyChangedEventArgs("Label2"));
        }
    }

    public string Label3
    {
        get { return label3; }
        set
        {
            label3 = value;
            PropertyChanged(this, new PropertyChangedEventArgs("Label3"));
        }
    }

    public string Label4
    {
        get { return label4; }
        set
        {
            label4 = value;
            PropertyChanged(this, new PropertyChangedEventArgs("Label4"));
        }
    }

    public string Label5
    {
        get { return label5; }
        set
        {
            label5 = value;
            PropertyChanged(this, new PropertyChangedEventArgs("Label5"));
        }
    }

    public string Label6
    {
        get { return label6; }
        set
        {
            label6 = value;
            PropertyChanged(this, new PropertyChangedEventArgs("Label6"));
        }
    }

    public string MailLabel
    {
        get { return MailLabel; }
        set
        {
            MailLabel = value;
            PropertyChanged(this, new PropertyChangedEventArgs("MailLabel"));
        }
    }

    List<string> countries = new List<string>
    {
        "Afghanistan",
        "Albania",
        "Algeria",
        "Andorra",
        "Angola",
        
    };
    public List<string> Countries => countries;




    void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        var handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}