using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using boost.Cloud.HealthCloud.DataFetcher;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace boost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
			//What a branch!
            this.InitializeComponent();
        }
        private void log_in_button_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            Window.Current.Activate();
            rootFrame.Navigate(typeof(Game_Hub));
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            Window.Current.Activate();
            rootFrame.Navigate(typeof(New_User));
        }
    }
}
