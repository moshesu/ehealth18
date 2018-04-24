using boost.Core.App;
using boost.Core.Entities.Users;
using boost.Core.UserComparison;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace boost.Screens
{
	public sealed partial class CoachViewPlayerComparison : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();
		public SolidColorBrush ProgressCompareOthersColorBrush = MainPage.getProgressCompareOthersColorBrush();
		public SolidColorBrush ProgressComparePlayerColorBrush = MainPage.getProgressComparePlayerColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICompareUser _CompareUser;
		public readonly ICoachPlayersPage _ICoachPlayersPage;

		Player player;
		public CoachViewPlayerComparison()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_CompareUser = ProgramContainer.container.GetInstance<ICompareUser>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			player = (_ICoachPlayersPage.GetPlayers())[Coach_View_Players.Tag];
			Init();
		}
		private void Init()
		{
			var comp = _CompareUser.GetComparison(player.UserId);
			if (comp.ActiveHoursComparison.PercentileAmounts.Length == 0 ||
			    comp.SleepComparison.PercentileAmounts.Length == 0 ||
			    comp.StepsComparison.PercentileAmounts.Length == 0)
			{
				ShowMessage("Looks like we need more data from other users to compare your progress.");
				return;
			}
			AddProgressToGrid(comp.StepsComparison, 0);
			AddProgressToGrid(comp.SleepComparison, 1);
			AddProgressToGrid(comp.ActiveHoursComparison, 2);
		}
		private void AddProgressToGrid(ComparisonData comparison, int index)
		{
			string[] keys = new string[10];
			int length = keys.Length;
			int location = comparison.UserPercentile;
			int[] amounts = comparison.PercentileAmounts;
			double[] values = new double[length];
			int sumAmounts = amounts.Sum();
			string[] midKeys = new string[11];
			for (int i = 0; i < length + 1; i++)
			{
				midKeys[i] = ((int)(comparison.Min + (comparison.Max - comparison.Min) * 0.1 * i)).ToString();
			}
			for (int i = 0; i < length; i++)
			{
				keys[i] = midKeys[i] + "-" + midKeys[i + 1];
				values[i] = Math.Floor((double)amounts[i] / sumAmounts * 10 * 100) / 10;
			}
			double maxValue = values.Max();

			inner.RowDefinitions.Add(new RowDefinition());
			Grid grid = new Grid();
			InitGrid(grid, length * 1, 4);
			grid.SetValue(Grid.RowProperty, index);
			inner.Children.Add(grid);
			AddTitle(grid, index);
			for (int i = 0; i < length; i++)
			{
				int row = 1 * i + 1;
				TextBlock line = new TextBlock
				{
					Text = "\n",
				};
				line.SetValue(Grid.RowProperty, row + 1);
				grid.Children.Add(line);
				TextBlock textBlock = new TextBlock
				{
					Text = keys[i],
					FontSize = 15,
					TextWrapping = TextWrapping.WrapWholeWords,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,

				};
				textBlock.SetValue(Grid.RowProperty, row);
				textBlock.SetValue(Grid.ColumnProperty, 0);
				grid.Children.Add(textBlock);
				Rectangle rect = new Rectangle
				{
					Height = 30,
					Fill = (i == location) ? ProgressComparePlayerColorBrush : ProgressCompareOthersColorBrush,
					Width = (double)values[i] / maxValue * CoachViewPlayerStart.width / 2,
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Center,
					Stroke = TextOnBackgroundColorBrush,
					StrokeThickness = 1,
				};
				rect.SetValue(Grid.RowProperty, row);
				rect.SetValue(Grid.ColumnProperty, 1);
				rect.SetValue(Grid.ColumnSpanProperty, 2);
				grid.Children.Add(rect);
				TextBlock value = new TextBlock
				{
					Text = values[i].ToString() + "%",
					FontSize = 15,
					TextWrapping = TextWrapping.WrapWholeWords,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
				};
				value.SetValue(Grid.RowProperty, row);
				value.SetValue(Grid.ColumnProperty, 3);
				grid.Children.Add(value);
			}
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerStart));
		}
		public void InitGrid(Grid grid, int rows, int columns)
		{
			for (int i = 0; i < rows; i++)
			{
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}
			for (int j = 0; j < columns; j++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}
		}
		private void AddTitle(Grid grid, int index)
		{
			grid.RowDefinitions.Add(new RowDefinition());
			string msg = "Comparing " + player.FirstName + "'s ";
			switch (index)
			{
				case 0: msg += "Daily Steps"; break;
				case 1: msg += "Daily Sleep Minutes"; break;
				case 2: msg += "Weekly Active Minutes"; break;
			}
			TextBlock text = new TextBlock()
			{
				Text = msg + "\n",
				Foreground = TextOnBackgroundColorBrush,
				FontSize = 30,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			text.SetValue(Grid.RowProperty, 0);
			text.SetValue(Grid.ColumnSpanProperty, 4);
			grid.Children.Add(text);
		}
		public async void ShowMessage(string msg)
		{
			var messageDialog = new MessageDialog(msg);
			messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
			await messageDialog.ShowAsync();
			return;
		}
	}
}
