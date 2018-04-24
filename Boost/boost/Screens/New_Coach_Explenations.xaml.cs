using boost.Cloud.AzureDatabase;
using boost.Core.App;
using boost.Core.Entities.Users;
using boost.Repositories;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class New_Coach_Explenations : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly IPlayerRepository _IPlayerRepository;
		public readonly UserTypeRepository _userTypeRepository;

		public New_Coach_Explenations()
        {
            this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IPlayerRepository = ProgramContainer.container.GetInstance<IPlayerRepository>();
			_userTypeRepository = ProgramContainer.container.GetInstance<UserTypeRepository>();

			Coach coach = _signInFlow.NewCoachFlow();
			if (Game_Hub.CoachAndPlayer)
			{
				_userTypeRepository.SaveUserType(coach.UserId, UserType.CoachAndPlayer);
				((SignInFlow)_signInFlow).SaveUserToLocalStorage(UserType.Coach);
			}
			if (coach.FirstName == null)
			{
				this.Frame.Navigate(typeof(New_Coach));
				return;
			}
			welcome.Text = "Hi " + coach.FirstName + ".\n" +
			"\n" +
			"You will soon add your first player, but before that you will need to enter your credit card number!\n" +
			"\n" +
			"As you recall in boost we use Crystals for games' cost.\n" +
			"\n" +
			"1 ILS is worth 100 Crystals so when you reward your player with crystals tha appropriate amount of money will be deducted from your bank account\n" +
			"\n" +
			"After that you will add your first player!\n";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(New_Coach_CreditCard));
        }

    }
}
