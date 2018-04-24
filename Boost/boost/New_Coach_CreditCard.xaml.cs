using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class New_Coach_CreditCard : Page
    {
        string credit;
        private Visibility _show;
       // public string Show { get; set; }
        public Visibility Show
        {
            get { return _show; }
            set
            {
                if (_show == value)
                    return;
                _show = value;
                OnPropertyChanged();
            }
        }
        //Text="ERROR: your credit card number must be 16 digits long"
        //Visibility = "{ Bind Show , Mode=OneWay}"
        public New_Coach_CreditCard()
        {
            this.InitializeComponent();
            credit = "";
            _show = Visibility.Collapsed;
            DataContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            credit = box.Text;
        }
        void waiting(int time)
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(time);
                return 42;
            });
            t.Wait();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*if(credit.Length != 16)
            {
                confirmation_textblock.Visibility  = Visibility.Visible;
                confirmation_textblock.Text = "ERROR: your credit card number must be 16 digits long";
                return;
            }
            else
            {*/
                confirmation_textblock.Visibility = Visibility.Visible;
                /*confirmation_textblock.Text = "Validating";
                waiting(300);
                confirmation_textblock.Text = "Validating.";
                waiting(300);
                confirmation_textblock.Text = "Validating..";
                waiting(300);
                confirmation_textblock.Text = "Validating...";
                waiting(300);
                confirmation_textblock.Text = "Success!";
                waiting(1000);*/

            //}
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            Window.Current.Activate();
            rootFrame.Navigate(typeof(New_Coach_Add_New_Player));


        }

    }
}
