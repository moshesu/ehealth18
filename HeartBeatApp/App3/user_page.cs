using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Java.Lang.Ref;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Microsoft.Band;
using Com.Microsoft.Band.Sensors;
using Microsoft.WindowsAzure.Storage;

using Java.Lang;
using Java.Lang.Ref;
using static HBApp.Resource;
using System.Collections;
using Microsoft.WindowsAzure.Storage.Table;
using Microcharts;

namespace HBApp
{
    [Activity(Label = "user.page")]
    public class user_page : Activity
    {   //constants
        static int NumberOfSamplesPerEntry = 60;
        static int EmptyReading = 0;
        static int none = -1;
        static string dateFormat_day = "yyyy:MM:dd";    //OLD "dd:MM:yyyy";
        static string dateFormat_hr = "HH:mm:ss";
        static string Average = "Average", Variance = "Variance", Total_count = "Total Count";
        public static string storageConnectionString ="DefaultEndpointsProtocol=https;AccountName=ehealth7;AccountKey=CE+Szx92IRmdGo9CWZGku6ReTL1LZMNxxby0Qhb+9sdElHGQsdHZ6Krs3o2QXf10jL3u6KlKNVLJqXzaoH1PqQ==;EndpointSuffix=core.windows.net";

        private IBandClient client = null;
        
        //UI elements
        private Button btnStart, btnConsent, btnStop, dateBtn, modeBtn;
        private TextView txtStatus, txtStep, currentHB, statistics;
        private DatePicker fromDatePckr,toDatePckr;//, toDatePckr;
        private Microcharts.Droid.ChartView chartView, chartViewPie, historyChartView;
        private ViewStates vbtnConsent, vbtnStop, vdateBtn, vmodeBtn,
            vtxtStatus, vtxtStep, vcurrentHB, vmaxHR, vminHR, vfromDatePckr, vtoDatePckr,
            vchartView, vchartViewPie, vhistoryChartView, vstatistics;

        WeakReference<Activity> reference;
        HeartRateEventListener mHeartRateEventListener;

        List<int> ArrayHeartRate = new List<int>(); //keeps up to NumberOfSamplesPerEntry last HR band readings
        List<int> ChartQueue = new List<int>();
        private bool liveUpdate = true;
        private bool cloud_stats_recieved = false;
        private static string current_user;
        private static int age, weight, height;

        public double avg=none, variance=none;
        public int total_n=none;

        static double cloud_avg=none, cloud_variance=none, BMI;
        static int cloud_n=none;
        static AzureTableEntity return_var;
        NotificationHandler notifyHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Bundle itemBundle = this.Intent.GetBundleExtra("currentUserBundle");
            current_user = itemBundle.GetString("username") ?? "pos";
            age = itemBundle.GetInt("age", 40);
            weight = itemBundle.GetInt("weight", 90);
            height = itemBundle.GetInt("height", 200);
            BMI = weight / (height * weight);

            // Creating instance of NotificationJandler
            NotificationHandler.Initialize(60, 30);
            notifyHandler = NotificationHandler.getInstance();


            //Layout related
            SetContentView(Resource.Layout.activity_user_page);
            txtStatus = (TextView)FindViewById(Id.txtStatus);
            txtStep = (TextView)FindViewById(Id.txtStep);
            currentHB = (TextView)FindViewById(Id.bpm);
            btnStop = (Button)FindViewById(Id.btnStop);
            btnConsent = (Button)FindViewById(Id.btnConsent);
            modeBtn = (Button)FindViewById(Id.modeBtn);
            dateBtn = (Button)FindViewById(Id.dateBtn);
            fromDatePckr = (DatePicker)FindViewById(Id.fromDatePicker);
            toDatePckr = (DatePicker)FindViewById(Id.toDatePicker);
            statistics = (TextView)FindViewById(Id.statistics);
            DatePickerInit(fromDatePckr);
            DatePickerInit(toDatePckr);
            chartView = FindViewById<Microcharts.Droid.ChartView>(Resource.Id.chartView);
            chartViewPie = FindViewById<Microcharts.Droid.ChartView>(Resource.Id.chartViewPie);
            historyChartView = FindViewById<Microcharts.Droid.ChartView>(Resource.Id.HistorychartView);
            
            //set UI elements Default Visiblity
            txtStep.Visibility = ViewStates.Visible;
            btnConsent.Visibility = ViewStates.Visible;
            vfromDatePckr = ViewStates.Visible;
            vtoDatePckr = ViewStates.Visible;
            vdateBtn = ViewStates.Visible;
            vhistoryChartView = ViewStates.Gone;
            vstatistics = ViewStates.Gone;
            vchartViewPie = ViewStates.Visible;

            reference = new WeakReference<Activity>(this);  //used to declared as "final" in java
            mHeartRateEventListener = new HeartRateEventListener(this);
            new HeartRateConsentTask(this).Execute(reference);

            btnStop.Click += async (sender, e) => {
                try
                {
                    client.SensorManager.UnregisterHeartRateEventListener(mHeartRateEventListener);
                }
                catch (BandIOException be)
                {
                    appendToUI(be.Message);
                }
            };

            btnConsent.Click += async (sender, e) => {
                try
                {
                    new HeartRateConsentTask(this).Execute(reference);
                }
                catch (BandIOException be)
                {
                    appendToUI(be.Message);
                }
            };

            new RetrieveSingleTableEntry(this).Execute(current_user, Variance, Variance, storageConnectionString);
            new RetrieveSingleTableEntry(this).Execute(current_user, Average, Average, storageConnectionString);
            new RetrieveSingleTableEntry(this).Execute(current_user, Total_count, Total_count, storageConnectionString);


            


            dateBtn.Click += async (sender, e) => {
                try
                {
                    if (dateBtn.Text.Equals(GetString(Resource.String.render))) {
                        new DisplayRangeOfEntries(this).Execute(current_user,
                            "PartitionKey", Convert.ToDateTime(fromDatePckr.DateTime.ToString()).ToString(dateFormat_day),
                            "PartitionKey", Convert.ToDateTime(toDatePckr.DateTime.ToString()).ToString(dateFormat_day),
                            storageConnectionString);
                        //if retrieval is succesful, the graph visibility will change to visible and dateBtn mode to clean                               
                    }
                    else
                    {
                        historyChartView.Chart = null;
                        historyChartView.Visibility = ViewStates.Gone;
                        fromDatePckr.Visibility = ViewStates.Visible;
                        toDatePckr.Visibility = ViewStates.Visible;
                        dateBtn.Text = GetString(Resource.String.render);
                    }
                }
                catch (System.Exception se)
                {
                    appendToUI(se.Message);
                }
            };

            modeBtn.Click += async (sender, e) => {
                try
                {
                    if (modeBtn.Text.Equals(GetString(Resource.String.history_view)))
                    {
                        //save current live view UI element's visiblity mode
                        vtxtStep = txtStep.Visibility;
                        vtxtStatus = txtStatus.Visibility;
                        vbtnConsent = btnConsent.Visibility;
                        vchartView = chartView.Visibility;
                        vchartViewPie = chartViewPie.Visibility;
                        vcurrentHB = currentHB.Visibility;
                        //hide live view UI elements
                        txtStep.Visibility = ViewStates.Gone;
                        txtStatus.Visibility = ViewStates.Gone;
                        btnConsent.Visibility = ViewStates.Gone;
                        chartView.Visibility = ViewStates.Gone;
                        chartViewPie.Visibility = ViewStates.Gone;
                        currentHB.Visibility = ViewStates.Gone;
                        liveUpdate = false;
                        //make history view UI elements visible (if they were before)
                        historyChartView.Visibility = vhistoryChartView;
                        fromDatePckr.Visibility = vfromDatePckr;
                        toDatePckr.Visibility = vtoDatePckr;
                        dateBtn.Visibility = vdateBtn;
                        statistics.Visibility = ViewStates.Visible;
                        modeBtn.Text = GetString(Resource.String.live_view);
                    }
                    else   //go back to live view
                    {
                        //save current history view UI element's visiblity mode
                        vfromDatePckr = fromDatePckr.Visibility;
                        vtoDatePckr = toDatePckr.Visibility;
                        vdateBtn = dateBtn.Visibility;
                        vhistoryChartView = historyChartView.Visibility;
                        //hide history view UI elements
                        fromDatePckr.Visibility = ViewStates.Gone;
                        toDatePckr.Visibility = ViewStates.Gone;
                        dateBtn.Visibility = ViewStates.Gone;
                        historyChartView.Visibility = ViewStates.Gone;
                        //make live view UI elements visible
                        txtStep.Visibility = vtxtStep;
                        txtStatus.Visibility = vtxtStatus;
                        currentHB.Visibility = vcurrentHB;
                        btnConsent.Visibility = vbtnConsent;
                        chartView.Visibility = vchartView;
                        chartViewPie.Visibility = vchartView;
                        statistics.Visibility = ViewStates.Gone;
                        liveUpdate = true;
                        modeBtn.Text = GetString(Resource.String.history_view);
                    }
                }
                catch (System.Exception se)
                {
                    appendToUI(se.Message);
                }
            };

        }

        class DateChangedListener : Java.Lang.Object, DatePicker.IOnDateChangedListener
        {
            public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                System.Diagnostics.Debug.WriteLine(Convert.ToDateTime(view.DateTime.ToString()).ToString(dateFormat_day));
            }
        }

        /*in Java we could simply do: client.getSensorManager().requestHeartRateConsent(params[0].get(), new HeartRateConsentListener() {
         * but because HeartRateConsentListener is an interface c# wont allow using it in a smilar manner
         * so I was forced to implement the interface first before passing it to RequestHeartRateConsent */
        class ConsentListener : Java.Lang.Object, IHeartRateConsentListener
        {
            
            //public IntPtr Handle => IntPtr.Zero;
            user_page o_;
            public ConsentListener() { }
            public ConsentListener(user_page o) { o_ = o; }
            
            /*
            public void Dispose()
            {
                //throw new NotImplementedException();
            }
            */
            public void UserAccepted(bool p0)
            {
               new HeartRateSubscriptionTask(o_).Execute();
            }
        }

        class HeartRateEventListener : Java.Lang.Object, IBandHeartRateEventListener
        {
            user_page o_;
            public HeartRateEventListener() { }
            public HeartRateEventListener(user_page o) { o_ = o; }
            

            public void OnBandHeartRateChanged(IBandHeartRateEvent p0)
            {
                try {                                   
                    System.Diagnostics.Debug.WriteLine("#########################################");
                    System.Diagnostics.Debug.WriteLine(p0.HeartRate.ToString() + " Band quality Status: " + p0.Quality);
                    int x = p0.HeartRate;
                    if (o_.cloud_stats_recieved && o_.variance != none && o_.avg != none && o_.total_n != none)
                    {
                        if (o_.ArrayHeartRate.Count == NumberOfSamplesPerEntry) //we got enough samples to send to cloud:
                        {   
                            string date_hr = DateTime.Now.ToString(dateFormat_hr); //get date for RowKey
                            string date_day = DateTime.Now.ToString(dateFormat_day); //get date for PartitionKey 
                            new AddBandReading().Execute(current_user, string.Join(",",  o_.ArrayHeartRate.ToArray()), date_day, date_hr, storageConnectionString); //send data to cloud table
                            new AddBandReading().Execute(current_user, o_.variance.ToString(), Variance, Variance, storageConnectionString); //send data to cloud table
                            new AddBandReading().Execute(current_user, o_.avg.ToString(), Average, Average, storageConnectionString); //send data to cloud table
                            new AddBandReading().Execute(current_user, o_.total_n.ToString(), Total_count, Total_count, storageConnectionString); //send data to cloud table
                            o_.ArrayHeartRate.Clear();
                        }

                        if (p0.Quality == HeartRateQuality.Locked)
                        {   //we let the application learn about the user for 1000 "good" band observations before notifying about abnormal bpm
                            if (o_.total_n > 1000)  
                                NotificationHandler.CheckAndCreateNotification(o_.BaseContext, o_, x);
                            o_.ArrayHeartRate.Add(x);
                            o_.variance = o_.Welford_Variance(x, o_.avg, o_.total_n, o_.variance);
                            o_.avg = o_.Welford_Avg(x, o_.avg, o_.total_n);
                            o_.total_n += 1;
                            System.Diagnostics.Debug.WriteLine("variance = " + o_.variance.ToString() + " avg= "+ o_.avg);
                            o_.UpdateChart(x);
                        }
                        else
                        {
                            o_.appendToUI("Band not locked on Heart Rate, try to stay still");    
                        }
                    }
                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("HeartRateEventListener ->  OnBandHeartRateChanged exception:/n" + e.Message);
                }
            }
        }

        double Welford_Avg(int x, double prev_avg, double prev_n)
        {
            return ((prev_n * prev_avg + x) / (prev_n + 1));
        }

        //citation: https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance#Online_algorithm
        double Welford_Variance(int x, double prev_avg, int prev_n, double prev_variance)
        {
            return prev_n == 0 ? 0 : ((prev_variance*(prev_n - 1)) / (prev_n)  + (System.Math.Pow(((double)x - prev_avg), 2)) / (prev_n + 1));
        }

        class HeartRateConsentTask : AsyncTask<WeakReference<Activity>, Java.Lang.Void, Java.Lang.Void> {
            /*
             *passsing the outerclass to the inner because unlike Java, c# cannot access outter class methods w/o them being static   
             */
            user_page o_;
            public HeartRateConsentTask(user_page o) { o_ = o; }

            protected override Java.Lang.Void RunInBackground(params WeakReference<Activity>[] @params)
            {
                
                Activity param1;
                @params[0].TryGetTarget(out param1); //to replace params[0].get() as it was in Java
                ConsentListener param2 = new ConsentListener(o_);

                try
                {
                    if (o_.getConnectedBandClient()) {
                        if (param1 != null) {
                            o_.client.SensorManager.RequestHeartRateConsent(param1, param2); 
                        }
                    } else {
                        o_.appendToUI("Band isn't connected. Please make sure bluetooth is on and the band is in range.\n");
                        o_.setVisibility(true);
                    }
                } catch (BandException e) {
                    string exceptionMessage = "Band Exception: Either Microsoft Health BandService is not available or \n" +
                        "Microsoft Health BandService doesn't support your SDK Version. Please update to latest SDK\n"
                        + e.Message.ToString();
                    o_.appendToUI(exceptionMessage);
                
                    }
                    catch (System.Exception e) {
                        o_.appendToUI(e.Message.ToString());
                    }
                    return null;
            } //end of RunInBackground

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                RunInBackground(o_.reference);
                return 0;
            }
        } //end of HeartRateConsentTask

        private class HeartRateSubscriptionTask : AsyncTask<Java.Lang.Void, Java.Lang.Void, Java.Lang.Void> { 
            user_page o_;
            public HeartRateSubscriptionTask() { }
            public HeartRateSubscriptionTask(user_page o) { o_ = o; }
            protected override Java.Lang.Void RunInBackground(params Java.Lang.Void[] @params)
            {
                try
                {
                    if (o_.getConnectedBandClient())
                    {
                        if (o_.client.SensorManager.CurrentHeartRateConsent == UserConsent.Granted)
                        {
                            o_.client.SensorManager.RegisterHeartRateEventListener(o_.mHeartRateEventListener);
                            o_.setVisibility(false); 
                        }
                    }
                } catch (System.Exception e) {
                    o_.appendToUI(e.Message.ToString());
                }
                    return null;
            }
            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                RunInBackground();
                return 0;
            }
        }
        
        private bool getConnectedBandClient() {
            if (client == null) {
                   IBandInfo[] devices = BandClientManager.Instance.GetPairedBands();
                   if (devices.Length == 0) {
                        appendToUI("Band isn't paired with your phone.\n");
                        setVisibility(true);
                        return false;
                    }
                    client = BandClientManager.Instance.Create(BaseContext, devices[0]);
            } else if (ConnectionState.Connected == client.ConnectionState) {
                return true;
            }

            appendToUI("Band is connecting...\n");
            return ConnectionState.Connected == client.Connect().Await();
        }
        
        //changes text of txtStatus UI element
        private void appendToUI(string str)
        {
            this.RunOnUiThread(() => {
                txtStatus.Text = str;
            });
        }

        //changes helper UI element's (help/instructions and user consent button) visiblity
        private void setVisibility(bool v)
        {
            if (!v)
            {
                RunOnUiThread(() => {
                    txtStep.Visibility = ViewStates.Gone;
                    btnConsent.Visibility = ViewStates.Gone;
                });
            }
            else
            {
                RunOnUiThread(() => {
                    txtStep.Visibility = ViewStates.Visible;
                    btnConsent.Visibility = ViewStates.Visible;
                });
            }
        }
        
        //updates LIVE view chart (as new band readings are recieved)
        private void UpdateChart(int point)
        {
            try
            {

                if (ChartQueue.Count != NumberOfSamplesPerEntry)
                {
                    ChartQueue.Add(point);
                }
                else
                { 
                    ChartQueue.RemoveAt(0);
                    ChartQueue.Add(point);
                }

               
                List<int> local_queue = new List<int>();
                local_queue = ChartQueue.ToList<int>();
                PointChart chart = new PointChart()
                {
                    Entries = local_queue.Select(v => new Entry(v)
                    {
                        Color = colorGraph(v),
                        ValueLabel = v.ToString().Equals(EmptyReading.ToString()) ? " " : v.ToString()
                    }),
                };
                chart.BackgroundColor = SkiaSharp.SKColors.Transparent;


                List<Entry> entries_radial = new List<Entry>
                {
                    new Entry((float)avg)
                    {
                         Color = SkiaSharp.SKColors.Transparent
                    },
                    new Entry(point)
                    {
                        Color = colorGraph(point)
                    }
                };

                RadialGaugeChart chart_radial = new RadialGaugeChart()
                {
                    Entries = entries_radial
                };
                chart_radial.MaxValue = (int)(avg + System.Math.Sqrt(variance)*2);
                chart_radial.MinValue = 0;
                chart_radial.LineAreaAlpha = 0;
                chart_radial.Margin = 10;

                chart_radial.BackgroundColor = SkiaSharp.SKColors.Transparent;
                if (liveUpdate) { 
                    RunOnUiThread(() =>
                    {
                        currentHB.Text = point.ToString();
                        txtStatus.Text = "";
                        chartView.Chart = chart;
                        chartViewPie.Chart = chart_radial;   
                    });
                }
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("UpdateChart() exception: "+e.Message);
            }
        }
        
        //dynamically colors the graphs according to the distance from avg+std*1.6
        private SkiaSharp.SKColor colorGraph(double v)
        {
            if ((int)v == EmptyReading)
            {
                return SkiaSharp.SKColors.Transparent;
            }
            if (cloud_stats_recieved)
            { 
                try
                {

                    double std_d = System.Math.Sqrt(variance);

                    if (v < avg - 1.6 * std_d || v > avg + 1.6 * std_d)
                    { 
                        if (v < avg - 2 * std_d || v > avg + 2 * std_d)
                        {
                        
                            return SkiaSharp.SKColors.DarkRed;   //red
                        }
                        return SkiaSharp.SKColors.Red;   //orange
                    }
                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("ColorGraph() "+e.Message);
                }
            }
            return SkiaSharp.SKColor.Parse("#fd4a4c");
        }
        //usage: new AddBandReading().execute(userName(string), Value(string), PartitionKey(string), RowKey(string), storageConnectionString)
        //Adds a single row to Azure table storag - storageConnectionString, in table name - userName, with PartitionKey RowKey and Value.
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

        //retrieves a single table entity/entry from:
        //params[3] - table connection string
        //params[0] - table name
        //params[1], params[2] - table partition and row keys
        //the retrieved entity is assigned to user_page.return_var
        private class RetrieveSingleTableEntry : AsyncTask<string, Java.Lang.Void, Java.Lang.Void>
        {
            AzureTableEntity local_return_var;
            string PKey;    //partition key, for use in OnPostExecute
            user_page o_;

            public RetrieveSingleTableEntry() { }
            public RetrieveSingleTableEntry(user_page o) { o_ = o; }

            protected override Java.Lang.Void RunInBackground(params string[] @params)
            {
                try
                {
                    int count = @params.Length;
                    if (count == 4)
                    {
                        PKey = @params[1];
                        // Retrieve storage account from connection-string.
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(@params[3]);
                        // Create the table client.
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                        // Create a cloud table object for the table.
                        CloudTable cloudTable = tableClient.GetTableReference(@params[0]);
                        // Create an operation to retrieve
                        TableOperation retrieveEntity = TableOperation.Retrieve<AzureTableEntity>(@params[1], @params[2]);
                        TableResult retrievedResult = cloudTable.ExecuteAsync(retrieveEntity).Result;
                        local_return_var = (AzureTableEntity)retrievedResult.Result;
                    }
                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("RetrieveSingleTableEntry" + e.Message);
                }

                return null;
            }
            protected override void OnPostExecute(Java.Lang.Void results)
            {
                if (local_return_var != null)
                {  
                    if (PKey.Equals(Average))
                        cloud_avg = Convert.ToDouble(local_return_var.Value);
                    if (PKey.Equals(Variance))
                        cloud_variance = Convert.ToDouble(local_return_var.Value);
                    if (PKey.Equals(Total_count))
                        cloud_n = Convert.ToInt32(local_return_var.Value);
                    if (cloud_n != none && cloud_variance != none && cloud_avg != none)
                    {
                        o_.avg = cloud_avg;
                        o_.variance = cloud_variance;
                        o_.total_n = cloud_n;
                        o_.cloud_stats_recieved = true;
                    }
                    return_var = local_return_var;
                }
            }
        }


        //retrieves entries in range between:
        //params[5] - table connection string
        //params[0] - table name
        //params[1], params[2] - 1st key type ("PartitionKey"/"RowKey") and value 
        //params[3], params[4] - 2nd key type ("PartitionKey"/"RowKey") and value
        //the retrieved entities are sent to UpdateHistoryChart to be displayed in a graph view
        private class DisplayRangeOfEntries : AsyncTask<string, Java.Lang.Void, Java.Lang.Void>
        {
            List<AzureTableEntity> items = new List<AzureTableEntity>();
            TableContinuationToken token = null;
            user_page o_;

            public DisplayRangeOfEntries() { }
            public DisplayRangeOfEntries(user_page o) { o_ = o; }

            protected override Java.Lang.Void RunInBackground(params string[] @params)
            {
                try
                {
                    int count = @params.Length;
                    if (count == 6)
                    {
                        // Retrieve storage account from connection-string.
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(@params[5]);
                        // Create the table client.
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                        // Create a cloud table object for the table.
                        CloudTable cloudTable = tableClient.GetTableReference(@params[0]);
                        // Create the table query.
                        TableQuery<AzureTableEntity> rangeQuery = new TableQuery<AzureTableEntity>().Where(
                            TableQuery.CombineFilters(
                                TableQuery.GenerateFilterCondition(@params[1], QueryComparisons.GreaterThanOrEqual, @params[2]),
                                TableOperators.And,
                                TableQuery.GenerateFilterCondition(@params[3], QueryComparisons.LessThanOrEqual, @params[4])));
                        do
                        {
                            TableQuerySegment<AzureTableEntity> seg = cloudTable.ExecuteQuerySegmentedAsync<AzureTableEntity>(rangeQuery, token).Result;
                            token = seg.ContinuationToken;
                            items.AddRange(seg);
                        } while (token != null);

                    }
                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

                return null;
            }
            protected override void OnPostExecute(Java.Lang.Void results)
            {
                foreach (var v in items)  System.Diagnostics.Debug.WriteLine(v.Value);

                o_.UpdateHistoryChart(items);
                //TO DO: call to populate history graph
            }
        }

        private void UpdateHistoryChart(List<AzureTableEntity> input_items)
        {
            try
            {
                int num;
                List<double> points = new List<double>();
                List<double> points_trim = new List<double>();
                List<AzureTableEntity> items = new List<AzureTableEntity>();
                items = input_items.ToList<AzureTableEntity>();

                foreach (var v in items)
                {

                    int sum = 0, count = 0;
                    foreach (string x in v.Value.Split(','))
                    {
                        num = Convert.ToInt32(x);
                        if (num != EmptyReading)
                        {
                            sum = sum + num;
                            count = count + 1;
                        }
                    }
                    if (count>0)
                        points.Add(sum / count);
                    
                }
                if (points.Count > 150)
                {
                    points_trim = points.Take(150).ToList();
                }
                else
                {
                    points_trim = points;
                }
                //TO DO: find k local maximums (spread apart)
                int index = 0;
                LineChart chart = new LineChart()
                {
                    Entries = points_trim.Select(v => new Entry((float)v)
                    {
                        Color = colorGraph(v),
                        ValueLabel = Label(index++, (float)v)
                    }),

                };
                
                chart.BackgroundColor = SkiaSharp.SKColors.Transparent;
                RunOnUiThread(() =>
                {
                    historyChartView.Chart = chart;
                    historyChartView.Visibility = ViewStates.Visible;
                    fromDatePckr.Visibility = ViewStates.Gone;
                    toDatePckr.Visibility = ViewStates.Gone;
                    statistics.Text = string.Format("Your average bpm: " + (points.Sum()/points.Count).ToString() + 
                                                  "\nYour global average bpm: " + avg.ToString());
                    dateBtn.Text = GetString(Resource.String.clean);
                });
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }
        string Label(int index, float value)
        {
            if (index % 30 == 0)
            {
                return value.ToString();
            }
            return "";
        }
        //called by onCreate to initialize the datePickers 
        private void DatePickerInit(DatePicker DatePckr)
        {
            
            DateChangedListener dateChangedListener = new DateChangedListener();
            DatePckr.Init(2018, 4, 21, dateChangedListener);
        }
    } //end of activity/class
} //end of namespace