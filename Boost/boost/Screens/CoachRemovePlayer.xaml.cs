using boost.Core.App;
using boost.Core.Entities.Users;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System;
using boost.Repositories;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost.Screens
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CoachRemovePlayer : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public readonly IPlayerRepository _IPlayerRepository;
		public CoachRemovePlayer()
		{
			InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			_IPlayerRepository = ProgramContainer.container.GetInstance<IPlayerRepository>();
			Player[] players = _ICoachPlayersPage.GetPlayers();
			for (int i = 0; i < players.Length; i++)
			{
				if (players[i].FirstName == "")
					continue;
				inner.RowDefinitions.Add(new RowDefinition());
				Button butt = new Button()
				{
					Content = players[i].FirstName + " " + players[i].LastName,
					Background = LogOutButtonColorBrush,
					Tag = i, //Player number in coach's list.
				};
				butt.SetValue(Grid.RowProperty, i);
				butt.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
				butt.Click += new RoutedEventHandler(RemovePlayerClick);
				inner.Children.Add(butt);
			}
		}
		private async void RemovePlayerClick(object sender, RoutedEventArgs e)
		{
			Player player = _ICoachPlayersPage.GetPlayers()[(int)((Button)sender).Tag];
			string msg = "Are you sure you want to remove " + player.FirstName + " " + player.LastName + " from your players list?";
			var messageDialog = new MessageDialog(msg);
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("No")  { Id = 1 });
			var result = await messageDialog.ShowAsync();
			if ((int)result.Id == 1)
				return;
			_IPlayerRepository.RemovePlayer(player.UserId);

			this.Frame.Navigate(typeof(Coach_View_Players));
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Coach_View_Players));
		}
	}
}
