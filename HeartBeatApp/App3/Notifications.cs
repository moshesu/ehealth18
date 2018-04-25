using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Java.Lang;



using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Entry = Microcharts.Entry;
using SkiaSharp;
using Microcharts;
using static HBApp.Resource;




namespace HBApp
{
    public enum HRStatus { Normal, Hypo, Hyper, NotStable };
    public enum NoteStatus { Ignore, Call, None };

    class NotificationHandler
    {
        //Refernce to the current class
        private static NotificationHandler instance;

        // Having a unique ID for the motification
        private static readonly int ButtonClickNotificationId = 1000;

        // This list will contain the notification history - this way we can track the
        // numbers of notification and reduce notification number
        static List<NotifyItem> statusHistoryList;

        // We will keep history for historyTime seconds
        static TimeSpan historyTime;

        // When getting notStable HB, any ubnormal HB in notStableTime after it will consider notStable as well
        static TimeSpan notStableTime;

        // Ignore flag - when on we will not get any notifications
        public static bool ignore;

        // Depends on the age of the patient the bounderies of normal heart rate are changes
        static int minHRBound;
        static int maxHRBound;

        // this class is a Singelton
        private NotificationHandler(int historyTimeSec, int notStableTimeSec, int minHRBound = 60, int maxHRBound = 120)
        {
            historyTime = new TimeSpan(0, 0, historyTimeSec);
            notStableTime = new TimeSpan(0, 0, notStableTimeSec);
            NotificationHandler.minHRBound = minHRBound;
            NotificationHandler.maxHRBound = maxHRBound;
            statusHistoryList = new List<NotifyItem>();
        }

        public static void Initialize(int historyTimeSec, int notStableTimeSec, int minHRBound = 60, int maxHRBound = 120)
        {
            // We are creating this class only one time!
            if (instance == null)
            {
                instance = new NotificationHandler(historyTimeSec, notStableTimeSec, minHRBound, maxHRBound);
            }
        }

        public static NotificationHandler getInstance()
        {
            if (instance == null)
            {
                throw new IllegalStateException("NotificationHandler is not initialized");
            }
            return instance;
        }

        public static void CheckAndCreateNotification(Context context, user_page userPage, int HR)
        {
            // In case the user ask to ignore and the ignore counter is still counting we will not do anything
            if (ignore) return;

            // Extracting the current HB status
            HRStatus heartStatus = calcCurrentStatus(userPage, HR);

            // Create NotifyItem item and put it in the 
            statusHistoryList.Add(new NotifyItem(heartStatus, HR));

            // Creating notifications for heach one of the cases
            if (heartStatus != HRStatus.Normal)
            {
                NotifyHeartRate(context, userPage, HR, heartStatus);
            }
        }

        private static void NotifyHeartRate(Context context, user_page userPage, int HR, HRStatus status)
        {
            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(userPage, typeof(NotificationActivity));

            // Passing HR and HRStatus to notification activity
            Bundle valuesForActivity = new Bundle();
            valuesForActivity.PutInt("status", (int)status);
            valuesForActivity.PutInt("HR", HR);
            resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(NotificationActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent | (int)PendingIntentFlags.OneShot);

            // Build the notification:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                .SetContentTitle("Is everything OK?")      // Set its title
                .SetNumber(HR)                       // Display the HR in the Content Info
                .SetSmallIcon(Resource.Drawable.emptyheart_white)  // Display this icon
                .SetContentText(string.Format(
                    "Your heart beat {0} to {1} bpm.",
                    status == HRStatus.Hypo ? "decreased dramaticaly" :
                    status == HRStatus.Hyper ? "increased dramaticaly" :
                    "is not stable and changed dramatically", HR)); // The message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager =
                (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.Notify(ButtonClickNotificationId, builder.Build());

        }

        private static HRStatus calcCurrentStatus(user_page userPage, int HR)
        {
            HRStatus last;
            HRStatus retStatus = HRStatus.Normal;
            DateTime now = DateTime.Now;
            var lastItem = statusHistoryList.LastOrDefault();
            last = lastItem.Equals(default(NotifyItem)) ? HRStatus.Normal : lastItem.heartStatus;

            // Fixme: there is a period of time inwhich no matter how "normal" your heart rate you are still in 
            // abnormal condition - maybe we should add a counter or some thing that will tell us until when the 
            // status is still abnormal.
            // Meanwhile - when the current status is the normal range we will consider it as normal condition
            // REGARDLESS the previous condition.

            switch (last)
            {
                case HRStatus.Hyper:
                    // Hypo
                    if (HR < minHRBound || HR <= (userPage.avg-2*System.Math.Sqrt(userPage.variance))) // or less than HR < |avg -2*std|
                    {
                        retStatus = HRStatus.NotStable;
                    }
                    // Hyper
                    if (HR > maxHRBound || HR >= (userPage.avg + 2 * System.Math.Sqrt(userPage.variance))) // or more than HR > |avg +2*std|
                    {
                        retStatus = HRStatus.Hyper;
                    }
                    break;
                case HRStatus.Hypo:
                    // Hypo
                    if (HR < minHRBound || HR <= (userPage.avg - 2 * System.Math.Sqrt(userPage.variance))) // or less than HR < |avg -2*std|
                    {
                        retStatus = HRStatus.Hypo;
                    }
                    // Hyper
                    if (HR > maxHRBound || HR >= (userPage.avg + 2 * System.Math.Sqrt(userPage.variance))) // or more than HR > |avg +2*std|
                    {
                        retStatus = HRStatus.NotStable;
                    }
                    break;
                case HRStatus.NotStable:
                    // Check if the limit time of none regular HB time is passed
                    if (now.Subtract(notStableTime) < lastItem.time)
                    {
                        retStatus = HRStatus.NotStable;
                    }
                    // Hypo
                    if (HR < minHRBound || HR <= (userPage.avg - 2 * System.Math.Sqrt(userPage.variance))) // or less than HR < |avg -2*std|
                    {
                        retStatus = HRStatus.Hypo;
                    }
                    // Hyper
                    if (HR > maxHRBound || HR >= (userPage.avg + 2 * System.Math.Sqrt(userPage.variance))) // or more than HR > |avg +2*std|
                    {
                        retStatus = HRStatus.Hyper;
                    }
                    break;
                case HRStatus.Normal:
                    if (HR < minHRBound || HR <= (userPage.avg - 2 * System.Math.Sqrt(userPage.variance))) // or less than HR < |avg -2*std|
                    {
                        retStatus = HRStatus.Hypo;
                    }
                    // Hyper
                    if (HR > maxHRBound || HR >= (userPage.avg + 2 * System.Math.Sqrt(userPage.variance))) // or more than HR > |avg +2*std|
                    {
                        retStatus = HRStatus.Hyper;
                    }
                    break;
            }
            // In case of ublormal HR we will remove far history items from statusHistoryList
            if (retStatus != HRStatus.Normal)
            {
                statusHistoryList.RemoveAll(x => now.Subtract(historyTime) >= x.time);
            }

            // In case the status OK
            return retStatus;
        }
    }

    struct NotifyItem
    {
        public int HR;
        public HRStatus heartStatus;
        public DateTime time;
        public NoteStatus noteStat;

        public NotifyItem(HRStatus curr, int HR)
        {
            time = DateTime.Now;
            heartStatus = curr;
            noteStat = NoteStatus.None;
            this.HR = HR;
        }

    }


    [Activity(Label = "NotificationActivity")]
    public class NotificationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Getting staus and HR from last activity
            HRStatus status = (HRStatus)Intent.Extras.GetInt("status", -1);
            int HR = Intent.Extras.GetInt("HR", -1);
            int ignoreTIme = 60; // in Seconds

            // UI chart
            SetContentView(Resource.Layout.activity_notification);  
            Button bdismiss = (Button)FindViewById(Id.dismiss);
            Button bemergency = (Button)FindViewById(Id.emergency);
            Button bignore = (Button)FindViewById(Id.ignore);


            // Need to add layout and content to the Notification
            // Display the count sent from the first activity:
            TextView textView = FindViewById<TextView>(Resource.Id.textView);
            textView.Text = string.Format(
                    "Your heart beat {1} to {0} bpm.", HR,
                    status == HRStatus.Hypo ? "decreased dramaticaly" :
                    status == HRStatus.Hyper ? "increased dramaticaly" :
                    "is not stable and changed dramatically");

            TextView textHR = FindViewById<TextView>(Resource.Id.myImageViewText);
            textHR.Text = string.Format("{0}", HR);

            // Clicking ignore will start a downtime counter for the next 60 seconds
            // Maybe should replace it to ignore till says otherwise
            bignore.Click += async (sender, e) =>
            {
                NotificationHandler.ignore = true;
                var cts = new CancellationTokenSource();
                cts.CancelAfter(ignoreTIme*1000); // 60 sec
                try
                {
                    await TimerAsync(1000, cts.Token);
                    Finish();
                }
                catch (AggregateException aex)
                {
                    aex.Handle(ex =>
                    {
                        // Handle the cancelled tasks
                        TaskCanceledException tcex = ex as TaskCanceledException;
                        if (tcex != null)
                        {
                            NotificationHandler.ignore = false;
                            return true;
                        }

                        // Not handling any other types of exception.
                        System.Diagnostics.Debug.WriteLine(aex.StackTrace);
                        return false;
                    });
                }

            };

            bemergency.Click += delegate
            {
                var uri = Android.Net.Uri.Parse("tel:101");
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            };

            bdismiss.Click += async (sender, e) =>
            {
                Finish();
            };


            // This task runs in the background til it gets a cancellation request
            async Task TimerAsync(int interval, CancellationToken token)
            {
                while (token.IsCancellationRequested)
                {
                    await Task.Delay(interval, token);
                }
            }
        }  
    }
}