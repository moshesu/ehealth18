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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Coach_Or_Player : Page
    {
        bool coach;
        bool player;
        public Coach_Or_Player()
        {
            this.InitializeComponent();
            coach = false;
            player = false;
        }

        private void radioButton_Checked_Coach(object sender, RoutedEventArgs e)
        {
            coach = true;
            player = false;
        }
        private void radioButton_Checked_Player(object sender, RoutedEventArgs e)
        {
            player = true;
            coach = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (coach == false && player == false)
            {
                return;
            }
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }
            if(coach == true)
            {
                Window.Current.Activate();
                rootFrame.Navigate(typeof(New_Coach));
            }
            if(player == true)
            {
                Window.Current.Activate();
                rootFrame.Navigate(typeof(New_Player));
            }
            
            
        }
    }
}
