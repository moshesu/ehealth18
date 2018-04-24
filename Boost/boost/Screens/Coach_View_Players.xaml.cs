using boost.Core.App;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using boost.Core.Entities.Users;
using boost.Screens;
using Windows.UI.Popups;
using System;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Coach_View_Players : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public static int Tag;
		public static double width;
		public Coach_View_Players()
		{
			InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			Player[] players = _ICoachPlayersPage.GetPlayers();
			for (int i = 0; i < players.Length; i++)
			{
				
					
				inner.RowDefinitions.Add(new RowDefinition());
				Button butt = new Button()
				{
					
					Content =(players[i].FirstName != "") ? players[i].FirstName + " " + players[i].LastName : "Code: "+players[i].UserId,
					Background = ButtonColorBrush,
					Foreground = ButtonTextColorBrush,
					Tag = i, //Player number in coach's list.
				};
				butt.SetValue(Grid.RowProperty, i);
				butt.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
				if(players[i].FirstName != "")
					butt.Click += new RoutedEventHandler(button_view_player_goals);
				else
					butt.Click += new RoutedEventHandler(UnassignedPlayerClick);
				inner.Children.Add(butt);
			}
		}
		private void button_view_player_goals(object sender, RoutedEventArgs e)
		{
			width = outer.ActualWidth;
			Tag = (int)((Button)sender).Tag;
			this.Frame.Navigate(typeof(CoachViewPlayerStart));
		}
		private async void UnassignedPlayerClick(object sender, RoutedEventArgs e)
		{
			string msg = "User is not initialized yet. Please give him the code: " + ((string)((Button)sender).Content).Substring(6);
			var messageDialog3 = new MessageDialog(msg);
			messageDialog3.Commands.Add(new UICommand("OK") { Id = 0 });
			var result3 = await messageDialog3.ShowAsync();
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Coach_Lobby));
		}

		private void AddNewPlayerClick(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachExplenationsBeforeAddingNewPlayerGoals));
		}
	}
}
