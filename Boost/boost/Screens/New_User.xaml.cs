using boost.Core.App;
using boost.Screens;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace boost
{
	public sealed partial class New_User : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public New_User()
        {
            this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			textBlock1.Text =	"Hi, welcome to Boost!\n"+
								"Your first step for a healthier lifestyle!";
        }
        private void button_Click_I_Understand(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewUserExplenations));
        }
    }
}
