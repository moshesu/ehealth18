using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ForParent
{
    public sealed partial class Analysis : Page
    {
        public Analysis()
        {
            this.InitializeComponent();
            string[] symbols = InitializeArray();
            // Specify the ComboBox item source
            ComBox.ItemsSource = symbols;
        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }

        public async Task<string> create_graphAsync(string combox)
        {
            symbol userSymbol = new symbol();
            userSymbol.email = Common.myChild;
            userSymbol.symbolName = combox;
            symbol[] userSymbolUsage = await Common.GetUserCounterAsync(combox.ToString());
            string graph_values = "";
            int[] month_values = new int[12];
            if (userSymbolUsage != null)
            {
                for (int i = 0; i < userSymbolUsage.Length; i++)
                {
                    month_values[userSymbolUsage[i].date.Month - 1] += 1;
                }
            }
            for (int i = 0; i < 11; i++) {
                graph_values += month_values[i].ToString() + ",";
            }
            graph_values += month_values[11].ToString();        
            return "http://chart.googleapis.com/chart?cht=bvg&chs=400x350&chxs=0,6699ff,12,0,lt|1,6699ff,10,1,lt&chxt=x,y&chxl=0:|Jan|Feb|Mar|Apr|May|June|July|Aug|Sep|Oct|Nov|Dec&chd=t:"+ graph_values +"&chf=bg,s,&chco=3399ff";
        }
        
        private async void ComBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string completeUri = "";
            if (ComBox.SelectedItem.ToString().Equals("allsymbols"))
            {
                completeUri = await create_graphAsync(ComBox.SelectedItem.ToString());
            }
            else {
                completeUri = await create_graphAsync("i" + ComBox.SelectedItem.ToString());
            }
            Uri requestUri = new Uri(completeUri);
            image.Source = new BitmapImage(requestUri);
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
    }
}
