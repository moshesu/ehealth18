using boost.Screens;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Coach_Or_Player : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

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
            if(coach == true)
            {
                this.Frame.Navigate(typeof(New_Coach_Explenations));
            }
            if(player == true)
            {
                this.Frame.Navigate(typeof(New_Player));
            }
        }
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewUserExplenations));
        }
    }
}
