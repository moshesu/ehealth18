using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Webkit;
using Microsoft.WindowsAzure.MobileServices;
using System;
using static HBApp.Resource;
using Java.Lang;
using Java.Util;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBApp
{
    [Activity(MainLauncher = true, Name = "main.login")]

    public class MainActivity : Activity
    {
        MobileServiceClient mClient=null;
        int duration = 2600;
        public static string current_username = "";
        private List<Users> mtable_list;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            // This MobileServiceClient has been configured to communicate with the Azure Mobile App and
            // Azure Gateway using the application url. You're all set to start working with your Mobile App!
            //Microsoft.WindowsAzure.MobileServices.MobileServiceClient eHealthHeartRateClient = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient("https://ehealthheartrate.azurewebsites.net");

            // Set our view from the "main" layout resource


            EditText etPassword = (EditText)FindViewById(Id.Password);
            EditText etUsername = (EditText)FindViewById(Id.Username);

            // TO DO: check how I can do this about Spinners

            Button blogin = (Button)FindViewById(Id.Blogin);
            Button bsignup = (Button)FindViewById(Id.BSignUp);

            if (mClient == null)
            { 
                AzureServiceAdapter.Initialize(this);
                mClient = AzureServiceAdapter.getInstance().getClient();
            }

            //IMobileServiceTable<Users> mTable = mClient.GetTable("Users", Users.class);  <-- as it was in java
            IMobileServiceTable<Users> mTable = mClient.GetTable<Users>();
            Toast.MakeText(this, "Welcome", ToastLength.Long).Show();


            // actions per button
            blogin.Click += async (sender, e) =>
            {
                Toast.MakeText(this, "login attempt as " + etUsername.Text.ToString(), ToastLength.Long).Show();
                try
                {
                    Users signinItem = await mTable.LookupAsync(UUID.NameUUIDFromBytes(Encoding.ASCII.GetBytes(etUsername.Text.ToString())).ToString());

                    System.Diagnostics.Debug.WriteLine("#########################################");
                    System.Diagnostics.Debug.WriteLine(signinItem);
                    System.Diagnostics.Debug.WriteLine("#########################################");
                    //Users item = ( Users )test_item;
                    //Debug.
                    if (signinItem != null)
                    {
                        if (signinItem.getpassword().Equals(etPassword.Text.ToString()))
                        {
                            current_username = signinItem.getusername();

                            Toast.MakeText(this, "Login Successfull", ToastLength.Long).Show();

                            Bundle itemBundle = new Bundle();
                            itemBundle.PutString("username", signinItem.getusername());
                            itemBundle.PutInt("age", Convert.ToInt32(signinItem.getdob()));
                            itemBundle.PutInt("height", signinItem.getheight());
                            itemBundle.PutInt("weight", signinItem.getweight());

                            var activity2 = new Intent(this, typeof(user_page));
                            activity2.PutExtra("currentUserBundle", itemBundle);
                            StartActivity(activity2);
                        }
                        else
                        {
                            current_username = "";
                            Toast.MakeText(this, "Login Failed, Invalid username or password", ToastLength.Long).Show();
                        }
                    }

                } catch (MobileServiceInvalidOperationException exp)
                {
                    if (exp.Message.Contains("The item does not exist"))
                    {
                        Toast.MakeText(this, "Username or Password are incorrect. Please try again.", ToastLength.Long).Show();
                        return;
                    } else
                    { 
                        Toast.MakeText(this, "Something went wrong :/ please try again!", ToastLength.Long).Show();
                    }
                }

                

            };

            // actions per button
            bsignup.Click += async (sender, e) =>
            {
                StartActivity(new Intent(this, typeof(SignUpActivity)));
            };
            
        }
    }
}





 

