using boost.Core.App;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace boost.Screens
{
	public sealed partial class NewUserExplenations : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public NewUserExplenations()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			textBlock1.Text =
			"Let's get started!\n" +
			"In boost there are 2 kinds of users: Players and Coaches.\n" +
			"\n" +
			"Coaches assign players' daily and weekly goals, for each goal a coach select if the player accomplishes it he will receive a reward in the form of crystals!\n" +
			"\n" +
			"Players need to finish their daily and weekly goals to get healthier and gain crystals to spend on games in the Game Hub!\n" +
			"\n" +
			"What are crystals?\n" +
			"-Crystals are boost's currency, they are what keeps the players motivated and the coaches up to date on their players' activities.\n" +
			"\n" +
			"-Each 1 ILS is worth 100 crystals so be cautious on how you spend them!\n" +
			"\n" +
			"-Coaches can reward their players with crystals for both accomplishing their daily or weekly goals and for performing extraordinary activities (by the coach's judgment)\n" +
			"\n" +
			"You will now choose if you want to be a player or a coach!\n" +
			"Friendly reminder:\n" +
			"-A coach needs to enter both a credit card and goals for his players.\n" +
			"\n" +
			"-A player needs to have a coach, once a coach creates a new player he will get a code to give to his player, if you do not have a coach you will not have a code and you couldn’t register to boost.\n" +
			"\n" +
			"Good Luck and have fun!\n";

		}
		private void button_Click_I_Understand(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Coach_Or_Player));
		}
	}
}
