using boost.Cloud.AzureDatabase;
using boost.Core.App;
using boost.Core.Entities.Users;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace boost
{
	public sealed partial class NewUnassignedPlayerExplenations : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly UserTypeRepository _userTypeRepository;
		Player player;
		public NewUnassignedPlayerExplenations()
        {
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			this.InitializeComponent();
			_userTypeRepository = ProgramContainer.container.GetInstance<UserTypeRepository>();
			player = _signInFlow.GetPlayer();
			if (Coach_Lobby.CoachAndPlayer)
			{
				_userTypeRepository.SaveUserType(player.UserId, UserType.CoachAndPlayer);
				((SignInFlow)_signInFlow).SaveUserToLocalStorage(UserType.Player);
			}
			welcome.Text = "Hi " + player.FirstName + ".\n" +
			"You will soon see the goals your coach assigned you, but first... some explanations about boost\n" +
			"\n" +
			"Your coach assigned you daily (and possibly) weekly goals, once you complete a goal you will recive a certian amount of crystals (chosen by your coach). But you can also get crystals if your coach is impressed by a certian activity you had.\n" +
			"\n" +
			"You can spend Your crystals in the Game Hub. There you will be able to play cool and exciting games such as Tic Tac Toe !\n" +
			"\n" +
			"Are you ready?\n";

		}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewUnassignedPlayerSeeGoals));
        }

    }
}
