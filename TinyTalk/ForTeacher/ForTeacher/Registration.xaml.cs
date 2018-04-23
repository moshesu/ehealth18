using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ForTeacher
{
    public sealed partial class Registration : Page
    {
        public Registration()
        {
            this.InitializeComponent();
            loading.Visibility = Visibility.Collapsed;
        }

        private async void RegisterAsync(object sender, RoutedEventArgs e)
        {
            //get data from textbox
            loading.Visibility = Visibility.Visible;
            string Email = email.Text;
            string firstName = firstname.Text;
            string lastName = lastname.Text;
            string childYear = "0";
            string gardenName = gardenname.Text;
            string Password = password.Password.ToString();
            string Teacher = "Teacher";

            string msg = "";

            ConnectDB db = new ConnectDB();
            msg = await db.ValidateUserAsync(Email, firstName, lastName,
                childYear, gardenName, Password);
            if (!msg.Equals("success"))
            {
                loading.Visibility = Visibility.Collapsed;
                result.Text = "faild: " + msg;
            }
            
            else
            {
                user newUser = new user
                {
                    email = Email,
                    firstname = firstName,
                    lastname = lastName,
                    childyear = 0,
                    type = Teacher,
                    gardenname = gardenName,
                    password = Password,
                };

                bool isPass = await db.CreateUserAsync(newUser);

                if (isPass == false)
                {
                    loading.Visibility = Visibility.Collapsed;
                    msg = "Can't connect database";
                    result.Text = "faild: " + msg;
                }

                if (isPass == true)
                {
                    loading.Visibility = Visibility.Collapsed;
                    result.Text = "success";
                }
            }
        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }
    }
}
