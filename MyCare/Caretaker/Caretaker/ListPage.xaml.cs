using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Caretaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListPage : Page
    {
        public ListPage()
        {
            InitializeComponent();
            var patients = ((string)localSettings.Values["cpatients"]).Trim().Split(' ');
            foreach (string patient in patients)
            {
                if (patient.Equals(""))
                {
                    continue;
                }
                Items.Add(patient);
            }
        }

        private ObservableCollection<string> _items = new ObservableCollection<string>();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public ObservableCollection<string> Items
        {
            get { return this._items; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddPatientPage));
        }

        private void GoToInfo(object sender, ItemClickEventArgs e)
        {
            String name = (String)e.ClickedItem;
            Frame.Navigate(typeof(InfoPage), name);
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["cemail"] = null;
            localSettings.Values["cpatients"] = null;
            Frame.Navigate(typeof(MainPage));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
