using boost.Cloud.AzureDatabase;
using boost.Core.App;
using boost.Core.Entities.Exceptions;
using boost.Core.Entities.Users;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
	public sealed partial class New_Player : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public New_Player()
		{
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			this.InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (_signInFlow.NewPlayerFlow(code.Text) == null)
				{
					ShowMessage("ERROR: Invalid code entered!");
					return;
				}
				this.Frame.Navigate(typeof(NewUnassignedPlayerExplenations));
			}
			catch(Exception ex)
			{
				ShowMessage(ex.Message);
				_signInFlow.SignOut();
				this.Frame.Navigate(typeof(MainPage));
			}
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
