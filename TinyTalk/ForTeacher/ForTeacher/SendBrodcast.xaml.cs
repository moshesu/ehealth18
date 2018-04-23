using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;

namespace ForTeacher
{
    public sealed partial class SendBrodcast : Page
    {
        static int symbolsForSend_curr = 0;
        static Image[] symbolsForSend = new Image[5];

        public SendBrodcast()
        {
            this.InitializeComponent();
            symbolsForSend[0] = forSend11;
            symbolsForSend[1] = forSend12;
            symbolsForSend[2] = forSend13;
            symbolsForSend[3] = forSend14;
            symbolsForSend[4] = forSend15;

        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < symbolsForSend.Length; i++)
            {
                symbolsForSend[i].Source = null;
                symbolsForSend[i].Tag = "";
            }
            symbolsForSend_curr = 0;
            send.Visibility = Visibility.Visible;
            delete_all.Visibility = Visibility.Collapsed;
            isClickable(true);
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }

        private void isClickable(bool enable)
        {
            ino.IsEnabled = enable;
            iagree.IsEnabled = enable;
            itowalk.IsEnabled = enable;
            icar.IsEnabled = enable;
            igrandmother.IsEnabled = enable;
            idad.IsEnabled = enable;
            ibro.IsEnabled = enable;
            idinner.IsEnabled = enable;
            ifor.IsEnabled = enable;
            ianger.IsEnabled = enable;
            itosleep.IsEnabled = enable;
            ipants.IsEnabled = enable;
            igrandfather.IsEnabled = enable;
            isis.IsEnabled = enable;
            imum.IsEnabled = enable;
            iwatermallon.IsEnabled = enable;
            ipain.IsEnabled = enable;
            iwant.IsEnabled = enable;
            ilove.IsEnabled = enable;
            ishirt.IsEnabled = enable;
            ishoes.IsEnabled = enable;
            iim.IsEnabled = enable;
            ijuice.IsEnabled = enable;
            ibreakfast.IsEnabled = enable;

        }

        private async void SendClick(object sender, RoutedEventArgs e)
        {
            isClickable(false);
            // Create final message
            string sendMsg = GetMsgToSend();
            delete_all.Visibility = Visibility.Visible;
            send.Visibility = Visibility.Collapsed;
            if (sendMsg != "")
            {
                String final_msg = sendMsg + "$" + Common.who_am_i + "$";
                // Get Garden children
                user[] kids = Common.gardenChildren;
                if (kids != null)
                {
                    for (int i = 0; i < kids.Length; i++)
                    {
                        //Create an HTTP client object
                        HttpClient httpClient = new HttpClient();
                        Uri requestUri = new Uri("https://function-queue-connect.azurewebsites.net/api/HttpTriggerCSharp1-send?code=c4TP96qDiVU6X5Zd6HNmAOCOIp35R52MB0MZnL6GRjY8ldfF2GqZ3A==&&name=" + final_msg + kids[i].email);
                            
                        HttpResponseMessage httpResponse = new HttpResponseMessage();
                        try
                        {
                            //Send the GET request
                            httpResponse = await httpClient.GetAsync(requestUri);
                            httpResponse.EnsureSuccessStatusCode();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    Frame toHome = Window.Current.Content as Frame;
                    toHome.Navigate(typeof(MainPage));
                }
            }
        }

        private string GetMsgToSend()
        {
            string sentence = "";
            if (symbolsForSend_curr != 0)
            {
                symbolsForSend_curr = 0;
                for (int i = 0; i < symbolsForSend.Length; i++)
                {
                    sentence = sentence + symbolsForSend[i].Tag.ToString() + "-";
                }
            }
            return sentence;
        }

        private void Symbol_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string button_name = button.Name;
            if (symbolsForSend_curr < 5)
            {
                AddToForSendGrid(button_name);
            }
        }

        private void AddToForSendGrid(string button_name)
        {
            Uri requestUri = new Uri(base.BaseUri, "/symbols/" + button_name + ".png");

            switch (symbolsForSend_curr)
            {
                case (0):
                    symbolsForSend[0].Source = new BitmapImage(requestUri);
                    symbolsForSend[0].Tag = button_name;
                    symbolsForSend_curr++;
                    break;
                case (1):
                    symbolsForSend[1].Source = new BitmapImage(requestUri);
                    symbolsForSend[1].Tag = button_name;
                    symbolsForSend_curr++;
                    break;
                case (2):
                    symbolsForSend[2].Source = new BitmapImage(requestUri);
                    symbolsForSend[2].Tag = button_name;
                    symbolsForSend_curr++;
                    break;
                case (3):
                    symbolsForSend[3].Source = new BitmapImage(requestUri);
                    symbolsForSend[3].Tag = button_name;
                    symbolsForSend_curr++;
                    break;
                case (4):
                    symbolsForSend[4].Source = new BitmapImage(requestUri);
                    symbolsForSend[4].Tag = button_name;
                    symbolsForSend_curr++;
                    break;
            }
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < symbolsForSend.Length; i++)
            {
                symbolsForSend[i].Source = null;
                symbolsForSend[i].Tag = "";
            }
            symbolsForSend_curr = 0;
            send.Visibility = Visibility.Visible;
            delete_all.Visibility = Visibility.Collapsed;
            isClickable(true);
        }

    }
}