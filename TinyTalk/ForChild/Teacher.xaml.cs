using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;


namespace ForChild
{
    public sealed partial class TeacherPage : Page
    {
        static bool flag = true;
        
        static Image[] symbolsSentFromOther1 = new Image[5];
        
        static int symbolsSentFromOther_full1 = 0;
        
        public TeacherPage()
        {
            this.InitializeComponent();
            IntializeArrays();
            flag = true;
            if(symbolsSentFromOther_full1 == 0)
                delete_all.Visibility = Visibility.Collapsed;
            GetMsgFromTeacher();
        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            flag = false;
            symbolsSentFromOther_full1 = 0;
            for (int i = 0; i < symbolsSentFromOther1.Length; i++)
            {
                symbolsSentFromOther1[i].Source = null;
            }
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            delete_all.Visibility = Visibility.Collapsed;
            symbolsSentFromOther_full1 = 0;
            for (int i = 0; i < symbolsSentFromOther1.Length; i++)
            {
                symbolsSentFromOther1[i].Source = null;
            }
            flag = true;
            GetMsgFromTeacher();
        }

        private void GetMessageImg(Image[] symbolsSentFromOther)
        {
            if (symbolsSentFromOther_full1 == 0)
            {
                symbolsSentFromOther_full1 = 1;

                for (int i = 0; i < symbolsSentFromOther1.Length; i++)
                {
                    symbolsSentFromOther1[i].Source = symbolsSentFromOther[i].Source;
                }
                delete_all.Visibility = Visibility.Visible;
            }
        }

        private async Task GetMessageAsync(OutTable[] message)
        {
            Image[] images = new Image[5];

            int numofmsg = 0; 
            
            if (message != null && message.Length > 0)
            {
                for (int x = 0; x < message.Length; x++)
                {
                    int i = 0;
                    numofmsg++;
                    string msg = message[x].Message;
                    string[] tmp = msg.Split('-');
                    foreach (string source in tmp)
                    {
                        if (i >= 5)
                            break;
                        Uri requestUri = new Uri(base.BaseUri, "/symbols/" + source + ".png");
                        images[i] = new Image();
                        images[i].Source = new BitmapImage(requestUri);
                        i++;
                    }
                    GetMessageImg(images);
                    if (numofmsg == 3)
                    {
                        return;
                    }//if
                }
            }
        }

        private async void GetMsgFromTeacher()
        {
            while (flag && symbolsSentFromOther_full1 != 1)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                OutTable[] table = await Common.GetMsgAsync(Common.myTeacher);
                await GetMessageAsync(table);
            }
        }

        private void IntializeArrays()
        {
            symbolsSentFromOther1[0] = afterSend11;
            symbolsSentFromOther1[1] = afterSend12;
            symbolsSentFromOther1[2] = afterSend13;
            symbolsSentFromOther1[3] = afterSend14;
            symbolsSentFromOther1[4] = afterSend15;
        }
    }
}

