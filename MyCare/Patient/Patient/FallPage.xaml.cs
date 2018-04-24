using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Storage.Streams;









// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Patient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FallPage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public FallPage()
        {
            InitializeComponent();
            DispatcherTimerSetup();
        }

        DispatcherTimer DT;
        DateTime stopTime;
        int timesTicked = 1;
        int timesToTick = 15;

        public void DispatcherTimerSetup()
        {
            DT = new DispatcherTimer();
            DT.Tick += dispatcherTimer_Tick;
            DT.Interval = new TimeSpan(0, 0, 1);
            stopTime = DateTime.Now.AddSeconds(15);
            DT.Start();
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            TimerDisplay.Text = "Auto click YES in:  " + (stopTime - DateTime.Now).ToString(@"\ hh\:mm\:ss");
            timesTicked++;
            if (timesTicked > timesToTick)
            {
                Yes_Click(null, null);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DT.Stop();
        }

        private async void Yes_Click(object sender, RoutedEventArgs e)
        {
            ///////////////////////////////
           
            errormessage.Text = "";
            string apiKey = "";

            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();

            Uri requestUri = new Uri("https://funcappformail.azurewebsites.net/api/MailGetKey?code=NuZ8kGzSajBeD1tvQfLg5xmzAoz1N5nV/Z2hWamc0WcPsdHAAvaesw==");

            HttpStringContent stringContent = new HttpStringContent(
                "{ \"key\": \"key\"}",
                UnicodeEncoding.Utf8, "application/json");

            try
            {
                HttpResponseMessage getResponse = await httpClient.PostAsync(requestUri, stringContent);

                if ((int)getResponse.StatusCode != 200)
                {
                    errormessage.Text = getResponse.StatusCode.ToString();
                }
                else
                {
                    apiKey = await getResponse.Content.ReadAsStringAsync();
                    apiKey = apiKey.Remove(0, 1);
                    apiKey = apiKey.Remove(apiKey.Length - 1, 1);
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult.ToString("X") == "80072EE7")
                {
                    errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: check your internet connection";
                }
                else
                {
                    errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                }
            }

          
            /////////////////////////////////////////////


            //mail using SendGrid
            //////////////////////////////////////////////////////////////////////
          
           // var apiKey = Environment.GetEnvironmentVariable("MyCareKey2");
            var client = new SendGridClient(apiKey);
            //////////////////////////////////////////////////////////////////////
            var msg = new SendGridMessage(); 
            msg.SetFrom(new EmailAddress("mycare.ehealth@gmail.com", "MyCare eHealth"));
            var recipients = new List<EmailAddress>();
            var caretakerMails = ((string) localSettings.Values["pcaretakers"]).Trim().Split(' ');
            foreach(var mail in caretakerMails)
            {
                recipients.Add(new EmailAddress(mail," "));
            }

            msg.AddTos(recipients);
            msg.SetSubject("One of your patients fell!");
            msg.AddContent(MimeType.Text, String.Format(
                "Falling detected in " + DateTime.Now.ToString() + "\n\n" +
                "Your patient [" + localSettings.Values["pname"].ToString() + " " + 
                localSettings.Values["plast"].ToString() + "] has fallen! \n\n" +
                "Please send help ASAP"));

            var response = await client.SendEmailAsync(msg);
            Frame.Navigate(typeof(ProfilePage));
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MonitorPage));
        }
    }
}
