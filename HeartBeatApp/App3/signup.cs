using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using System;
using static HBApp.Resource;
using Java.Lang;
using Java.Util;
using System.Text;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;



namespace HBApp
{
    [Activity(Name = "user.signup")]

    public class SignUpActivity : Activity
    {
        MobileServiceClient mClient;
        IMobileServiceTable<Users> mTable;
        public static string current_username = "";
        private List<Users> mtable_list;

        // Define the connection-string with your values.
        public static string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=ehealth7;AccountKey=CE+Szx92IRmdGo9CWZGku6ReTL1LZMNxxby0Qhb+9sdElHGQsdHZ6Krs3o2QXf10jL3u6KlKNVLJqXzaoH1PqQ==;EndpointSuffix=core.windows.net";
        public static string gsrConnectionString = "DefaultEndpointsProtocol=https;AccountName=ehealthgsr;AccountKey=IiazKTibVtoijGYqkYPpzMikUv1TLNdP7CEQ26ttlnsyOUKkl74ImKy1/jrmYRu41yB8BZYi4AQQVaRF/nyarg==;EndpointSuffix=core.windows.net";
        public static string accelConnectionString = "DefaultEndpointsProtocol=https;AccountName=ehealthaccel;AccountKey=DEagGBxhWhAzAaPiN6S4YyjRnAumnAjO9PzhCGEchfg9qVeTPbChGMOF6Uto4nEUZs2Y9ZDp1pBi7W2eZXmsRw==;EndpointSuffix=core.windows.net";
        //toast popup variables:
        int duration = 2006; // Toast.LENGTH_LONG * 4;
        bool validInput = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup);

            // This MobileServiceClient has been configured to communicate with the Azure Mobile App and
            // Azure Gateway using the application url. You're all set to start working with your Mobile App!
            //Microsoft.WindowsAzure.MobileServices.MobileServiceClient eHealthHeartRateClient = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient("https://ehealthheartrate.azurewebsites.net");

            // Set our view from the "main" layout resource

            EditText etFirstname = (EditText)FindViewById(Id.etFirstName);
            EditText etLasttname = (EditText)FindViewById(Id.etLastName);
            EditText etPassword = (EditText)FindViewById(Id.etPassword);
            EditText etHeigth = (EditText)FindViewById(Id.etHeight);
            EditText etWeigth = (EditText)FindViewById(Id.etWeight);
            EditText etDob = (EditText)FindViewById(Id.etDob);
            EditText etUsername = (EditText)FindViewById(Id.etUserName);
            EditText etDoctorName = (EditText)FindViewById(Id.etDoctorName);
            Spinner ganderSpinner = (Spinner)FindViewById(Id.spinnerGnder);
            Spinner healthConditionSpinner = (Spinner)FindViewById(Id.spinnerHealthCondiotion);

            // TODO: check how I can do this about Spinners
            string[] data = { "Female", "Male" };
            ArrayAdapter ganderAdapter = new ArrayAdapter(this,
                Android.Resource.Layout.SimpleListItem1, data);
            ganderSpinner.Adapter = ganderAdapter;

            string[] data1 = { "Healthy", "UnHealthy" };
            ArrayAdapter healthCondAdapter = new ArrayAdapter(this,
                Android.Resource.Layout.SimpleListItem1, data1);
            healthConditionSpinner.Adapter = healthCondAdapter;

            Button bsignup = (Button)FindViewById(Id.Bsingnup);

            mClient = AzureServiceAdapter.getInstance().getClient();

            //IMobileServiceTable<Users> mTable = mClient.GetTable("Users", Users.class);  <-- as it was in java
            mTable = mClient.GetTable<Users>();
            Toast.MakeText(this, "Welcome", ToastLength.Long).Show();

            // actions per button
            bsignup.Click += async (sender, e) =>
            {
                if (!isInputValidate(etFirstname,
                    etLasttname,
                    etPassword,
                    etHeigth,
                    etWeigth,
                    etDob,
                    etUsername,
                    etDoctorName))
                {
                    return;
                }
                //Newtonsoft.Json.Linq.JToken useritem;
                Users useritem;

                try
                {
                    // Checking out if username already exist in DB
                    useritem = await mTable.LookupAsync(UUID.NameUUIDFromBytes(Encoding.ASCII.GetBytes(etUsername.Text.ToString())).ToString());

                    // Getting to this line iff LookupAsync successfully finished
                    System.Diagnostics.Debug.WriteLine("Lookup requests return with success.");
                    etUsername.Error = "Username is already exists\n";
                    validInput = false;
                    System.Diagnostics.Debug.WriteLine("User is already exists\n{0}\n", etUsername.Text.ToString());
                }
                // In case the user is valid (not exists in DB) we will create a new one
                catch (MobileServiceInvalidOperationException exp)
                {
                    if (exp.Message.Contains("The item does not exist"))
                    {
                        System.Diagnostics.Debug.WriteLine("Lookup requests return with failure.");
                        System.Diagnostics.Debug.WriteLine("The username is free, lts' send signup request!");

                        // SignUp Request
                        sendSignupReq(etFirstname,
                                    etLasttname,
                                    etPassword,
                                    etHeigth,
                                    etWeigth,
                                    etDob,
                                    etUsername,
                                    etDoctorName,
                                    ganderSpinner,
                                    healthConditionSpinner);

                        // Finaly Create a user Item
                        //Users item = json2UserItem(useritem);

                        // Enter prev_n, avg, deviation = 0 when creating the tables
                    }
                    else
                    {
                        Toast.MakeText(this, "Something went wrong :/ please try again!", ToastLength.Long).Show();
                    }
                }

            };
        }

        bool isInputValidate(EditText etFirstname,
                       EditText etLastname,
                       EditText etPassword,
                       EditText etHeigth,
                       EditText etWeigth,
                       EditText etAge,
                       EditText etUsername,
                       EditText etDoctorName)
        {

            validInput = true;

            // TODO: Refactor to macros
            if (etFirstname.Text.ToString().Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Username cannot be empty");
                etFirstname.Error = "Username cannot be empty";
                validInput = false;
            }
            if (etLastname.Text.ToString().Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Lastname cannot be empty");
                etLastname.Error = "Lastname cannot be empty";
                validInput = false;
            }
            if (etDoctorName.Text.ToString().Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Doctor name cannot be empty");
                etDoctorName.Error = "Doctor name cannot be empty";
                validInput = false;
            }
            if (etHeigth.Text.ToString().Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Height cannot be empty");
                etHeigth.Error = "Height cannot be empty";
                validInput = false;
            }
            if (etWeigth.Text.ToString().Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Weight cannot be empty");
                etWeigth.Error = "Weight cannot be empty";
                validInput = false;
            }
            if (etAge.Text.ToString().Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Age cannot be empty");
                etAge.Error = "Age cannot be empty";
                validInput = false;
            }
            if ((etAge.Text.ToString().Length != 0) && (Int32.Parse(etAge.Text.ToString()) < 1))
            {
                System.Diagnostics.Debug.WriteLine("Age must be bigger than 0");
                etAge.Error = "Age must be bigger than 0";
                validInput = false;
            }
            if ((etWeigth.Text.ToString().Length != 0) && (Int32.Parse(etWeigth.Text.ToString()) < 1))
            {
                System.Diagnostics.Debug.WriteLine("Age must be bigger than 0");
                etWeigth.Error = "Weigth must be bigger than 0";
                validInput = false;
            }
            if ((etHeigth.Text.ToString().Length != 0) && (Int32.Parse(etHeigth.Text.ToString()) < 1))
            {
                System.Diagnostics.Debug.WriteLine("Age must be bigger than 0");
                etHeigth.Error = "Heigth must be bigger than 0";
                validInput = false;
            }

            //TODO: Password must be bigger than 8
            if (etPassword.Text.ToString().Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Password cannot be empty");
                etPassword.Error = "Password cannot be empty";
                validInput = false;
            }
            //TODO: Username must be bigger than 8
            if (etUsername.Text.ToString().Length < 3)
            {
                System.Diagnostics.Debug.WriteLine("Username must contain 3 or more letters");
                etUsername.Error = "Username must contain 3 or more letters";
                validInput = false;
            }

            return validInput;

        }

        async void sendSignupReq(EditText etFirstname,
                      EditText etLastname,
                      EditText etPassword,
                      EditText etHeigth,
                      EditText etWeigth,
                      EditText etDob,
                      EditText etUsername,
                      EditText etDoctorname,
                      Spinner ganderSpinner,
                      Spinner healthConditionSpinner)
        {

            try
            {

                // Creating Item and Inserting into 'Users' SQL Table
                Users item = new Users(etFirstname.Text.ToString(),
                                       etLastname.Text.ToString(),
                                        ganderSpinner.SelectedItem.ToString(),
                                        Int32.Parse(etDob.Text.ToString()),
                                        Int32.Parse(etHeigth.Text.ToString()),
                                        Int32.Parse(etWeigth.Text.ToString()),
                                        healthConditionSpinner.SelectedItem.ToString(),
                                        0,
                                        0,
                                        etPassword.Text.ToString(),
                                        etUsername.Text.ToString());

                await mTable.InsertAsync(item);


                //Try to execute ASYNC task to create a new Band Table
                try
                {
                    new CreateBandTable().Execute(item.getusername(), storageConnectionString);
                }
                catch (Java.Lang.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    Toast.MakeText(this, "Something went wrong :/ please try again!", ToastLength.Long).Show();
                    // TODO: Deleting user from SQL table
                }

                /* This Section is for colecting more data and extracting corelation from it
                new CreateBandTable().execute(item.getUsername(), gsrConnectionString);
                new CreateBandTable().execute(item.getUsername(), accelConnectionString);
                */
                System.Diagnostics.Debug.WriteLine("Tables created!");
                Toast.MakeText(this, "Sign up successfully!", ToastLength.Long).Show();

                // navigate to login page
                //StartActivity(new Intent(this, typeof(user_page)));



            }
            catch (Java.Lang.Exception exp)
            {
                exp.PrintStackTrace();
                Toast.MakeText(this, "Something went wrong :/ please try again!", ToastLength.Long).Show();
            }
            catch (System.Exception exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.ToString());
                Toast.MakeText(this, "Something went wrong :/ please try again!", ToastLength.Long).Show();
            }
        }


        // Fixme: need to refactor all code
        private class CreateBandTable : AsyncTask<string, Java.Lang.Void, Java.Lang.Void>
        {
            protected override Java.Lang.Void RunInBackground(params string[] @params)
            {
                string Average = "Average", Variance = "Variance", Total_count = "Total Count";

                try
                {
                    int count = @params.Length;
                    if (count == 2)
                    {
                        //Add a band readings Table for the new user:
                        // Retrieve storage account from connection-string.
                        string key = @params[1];
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(key);

                        // Create the table client.
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                        // Create the table if it doesn't exist.
                        string tableName = @params[0];
                        CloudTable cloudTable = tableClient.GetTableReference(tableName);
                        //sleep(8200);
                        cloudTable.CreateIfNotExistsAsync();

                        // Inititating Viraiance, Average and Total Count to 0 
                        new AddBandReading().Execute(tableName, 0.ToString(), Variance, Variance, storageConnectionString).Get(); //send data to cloud table
                        new AddBandReading().Execute(tableName, 0.ToString(), Average, Average, storageConnectionString).Get(); //send data to cloud table
                        new AddBandReading().Execute(tableName, 0.ToString(), Total_count, Total_count, storageConnectionString).Get(); //send data to cloud table

                    } else
                    {
                        System.Diagnostics.Debug.WriteLine("CreateBandTable Usage:       'new CreateBandTable().Execute(item.getusername(), storageConnectionString)'");
                    }
                } catch (Java.Lang.Exception e)
                {
                    e.PrintStackTrace();
                }
                    return null;
            }
        }

        private class AddBandReading : AsyncTask<string, Java.Lang.Void, Java.Lang.Void>
        {
            protected override Java.Lang.Void RunInBackground(params string[] @params)
            {
                try
                {
                    int count = @params.Length;
                    if (count == 5)
                    {
                        //Add a band readings Table for the new user:
                        // Retrieve storage account from connection-string.
                        CloudStorageAccount storageAccount =
                                CloudStorageAccount.Parse(@params[4]);
                        // Create the table client.
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                        // Create a cloud table object for the table.
                        CloudTable cloudTable = tableClient.GetTableReference(@params[0]);

                        // Create an operation to retrieve

                        AzureTableEntity tableEntry = new AzureTableEntity(@params[1], @params[2], @params[3]);
                        tableEntry.Value = @params[1];
                        TableOperation insertEntry = TableOperation.InsertOrReplace(tableEntry);
                        cloudTable.ExecuteAsync(insertEntry);
                    }
                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                return null;
            }
        }

    }

}