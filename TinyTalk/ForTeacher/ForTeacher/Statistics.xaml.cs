using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ForTeacher
{
    public sealed partial class Statistics : Page
    {
        static user current_child = null;

        public Statistics()
        {
            this.InitializeComponent();
            ComBox.Visibility = Visibility.Collapsed;
            string[] symbols = InitializeArray();
            ComBox.ItemsSource = symbols;
            InitializeComboox();
        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }

        public async Task<string> create_graphAsync(string combox)
        {
            symbol userSymbol = new symbol();
            userSymbol.email = current_child.email;
            userSymbol.symbolName = combox;
            symbol[] userSymbolUsage = await Common.GetUserCounterAsync(combox.ToString(), userSymbol.email);
            string graph_values = "";
            int[] month_values = new int[12];
            if (userSymbolUsage != null)
            {
                for (int i = 0; i < userSymbolUsage.Length; i++)
                {
                    month_values[userSymbolUsage[i].date.Month - 1] += 1;
                }
            }
            for (int i = 0; i < 11; i++)
            {
                graph_values += month_values[i].ToString() + ",";
            }
            graph_values += month_values[11].ToString();
            return "http://chart.googleapis.com/chart?cht=bvg&chs=400x350&chxs=0,6699ff,12,0,lt|1,6699ff,10,1,lt&chxt=x,y&chxl=0:|Jan|Feb|Mar|Apr|May|June|July|Aug|Sep|Oct|Nov|Dec&chd=t:" + graph_values + "&chf=bg,s,&chco=3399ff";
        }

        private async void ComBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string completeUri = "";
            if (ComBox.SelectedItem.ToString().Equals("allsymbols"))
            {
                completeUri = await create_graphAsync(ComBox.SelectedItem.ToString());
            }
            else
            {
                completeUri = await create_graphAsync("i" + ComBox.SelectedItem.ToString());
            }
            Uri requestUri = new Uri(completeUri);
            image.Source = new BitmapImage(requestUri);
        }

        private async void child_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectDB db = new ConnectDB();
            current_child = await db.GetUserByMailAsync(ComBox_child.SelectedItem.ToString());
            ComBox.Visibility = Visibility.Visible;
        }

        private string[] InitializeArray()
        {
            string[] symbols = {
                "no",
                "agree",
                "towalk",
                "car",
                "grandmother",
                "dad",
                "bro",
                "dinner",
                "for",
                "anger",
                "tosleep",
                "pants",
                "grandfather",
                "sis",
                "mum",
                "watermallon",
                "pain",
                "want",
                "love",
                "shirt",
                "shoes",
                "im",
                "juice",
                "breakfast",
                "allsymbols"
            };
            return symbols;
        }

        private void InitializeComboox()
        {
            user[] kids = Common.gardenChildren;
            string[] childrenInGarden = new string[Common.counter_child];
            int child_count = 0;
            if (kids != null)
            {
                for (int i = 0; i < Common.gardenChildren.Length; i++)
                {
                    if (Common.gardenChildren[i].type != "Teacher" && Common.gardenChildren[i].type != "Parent")
                    {
                        childrenInGarden[child_count] = Common.gardenChildren[i].email;
                        child_count++;
                    }
                }
            }
            ComBox_child.ItemsSource = childrenInGarden;
    }
    }
}
