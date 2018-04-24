using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Coach_Change_Credit_Card : Page
    {
        string credit;
        public Coach_Change_Credit_Card()
        {
            this.InitializeComponent();
            credit = "";
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            credit = box.Text;
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
            rootFrame.Navigate(typeof(Coach_Lobby));


        }
    }
}
