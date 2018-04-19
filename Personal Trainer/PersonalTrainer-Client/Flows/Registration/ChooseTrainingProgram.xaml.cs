using Microsoft.WindowsAzure.MobileServices;
using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.MainFlow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PersonalTrainer_Client.Flows.TrainingProgramDetails;
using Personal_TrainerService.DataObjects;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Core;

namespace PersonalTrainer_Client.Flows.Registration
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChooseTrainingProgram : Page
    {
        User user { get; set; }
        IMobileServiceTable<DefaultTrainingProgram> DefaultTrainingProgramsTable = App.MobileService.GetTable<DefaultTrainingProgram>();
        IMobileServiceTable<User> UsersTable = App.MobileService.GetTable<User>();
        IMobileServiceTable<UserTrainingProgram> UserTrainingProgramsTable = App.MobileService.GetTable<UserTrainingProgram>();
        List<UserTrainingProgram> optionalPrograms = new List<UserTrainingProgram>();


        public ChooseTrainingProgram()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        public async Task<List<UserTrainingProgram>> CalculateUserTrainingProgram(User user)
        {
            var body = new ChooseTrainingProgramBody()
            {
                User = user,
                AllTrainingPrograms = App.defaultTrainingPrograms
            };

            var response = await App.MobileService.InvokeApiAsync<ChooseTrainingProgramBody, List<UserTrainingProgram>>(
                "ChooseTrainingProgram",              // The name of the API
                body,                       // The body of the POST
                HttpMethod.Post,                // The HTTP Method
                null);

            return response;
        }

        public UserTrainingProgram CreateUserTrainingProgram(DefaultTrainingProgram defaultProgram)
        {
            var userPreferredWorkoutDays = user.PreferredWorkoutDays.Split(';');


            var userProgram = new UserTrainingProgram()
            {
                Difficulty = defaultProgram.Difficulty,
                DurationInWeeks = defaultProgram.DurationInWeeks,
                TrainingProgramGoal = defaultProgram.TrainingProgramGoal,
                TrainingProgramType = defaultProgram.TrainingProgramType,
                Name = defaultProgram.Name,
                Id = Guid.NewGuid().ToString()
            };

            int currentWorkoutId = 0;

            var workoutsIds = defaultProgram.WorkoutIds.Split(';');

            foreach (var day in userPreferredWorkoutDays)
            {
                switch (day)
                {
                    case "1":
                        userProgram.SundayWorkoutId = workoutsIds[currentWorkoutId];
                        break;
                    case "2":
                        userProgram.MondayWorkoutId = workoutsIds[currentWorkoutId];
                        break;
                    case "3":
                        userProgram.TuesdayWorkoutId = workoutsIds[currentWorkoutId];
                        break;
                    case "4":
                        userProgram.WednesdayWorkoutId = workoutsIds[currentWorkoutId];
                        break;
                    case "5":
                        userProgram.ThursdayWorkoutId = workoutsIds[currentWorkoutId];
                        break;
                    case "6":
                        userProgram.FridayWorkoutId = workoutsIds[currentWorkoutId];
                        break;
                    case "7":
                        userProgram.SaturdayWorkoutId = workoutsIds[currentWorkoutId];
                        break;
                }

                currentWorkoutId = (currentWorkoutId + 1) % workoutsIds.Count();
            }

            return userProgram;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            UserTrainingProgram chosenTrainingProgram = null;
            if ((bool)FirstOption.IsChecked)
            {
                chosenTrainingProgram = optionalPrograms[0];
            }
            else if ((bool)SecondOption.IsChecked)
            {
                chosenTrainingProgram = optionalPrograms[1];
            }
            else
            {
                chosenTrainingProgram = optionalPrograms[2];
            }

            user.CurrentTrainingProgramId = chosenTrainingProgram.Id;

            try
            {
                // Save user details in DB
                await UserTrainingProgramsTable.InsertAsync(chosenTrainingProgram);
                await UsersTable.InsertAsync(user);

                // Show confirmation massage
                MessageDialog confirmation = new MessageDialog("Thanks, you are now registered!");
                await confirmation.ShowAsync();

                var parameters = new MainDashboardParams(user);

                Frame.Navigate(typeof(MainDashboard), parameters);
            }
            catch (Exception)
            {
                MessageDialog msgDialogError = new MessageDialog("Sorry, we had an error. Please try again.");
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = (User)e.Parameter;
            optionalPrograms = await CalculateUserTrainingProgram(user);

            // Show on screen optinal programs options
            RadioButton[] buttons = { FirstOption, SecondOption, ThirdOption };
            Image[] images = { FirstImage, SecondImage, ThirdImage };
            for (int i = 0; i < 3; i++)
            {
                buttons[i].Content = optionalPrograms[i].Name;
                switch (optionalPrograms[i].TrainingProgramGoal)
                {
                    case TrainingProgramGoal.Fitness:
                        images[i].Source = new BitmapImage(new Uri("ms-appx:///Images/inside_fitness.jpg"));
                        break;
                    case TrainingProgramGoal.Flexibility:
                        images[i].Source = new BitmapImage(new Uri("ms-appx:///Images/inside_flex.jpg"));
                        break;
                    case TrainingProgramGoal.Size:
                        images[i].Source = new BitmapImage(new Uri("ms-appx:///Images/inside_strength.jpg"));
                        break;
                    case TrainingProgramGoal.Speed:
                        if (optionalPrograms[i].TrainingProgramType == TrainingProgramType.Gym)
                        {
                            images[i].Source = new BitmapImage(new Uri("ms-appx:///Images/inside_run.jpeg")); break;
                        }
                        else
                        {
                            images[i].Source = new BitmapImage(new Uri("ms-appx:///Images/outside_run.jpg")); break;
                        }
                    case TrainingProgramGoal.Strength:
                        images[i].Source = new BitmapImage(new Uri("ms-appx:///Images/inside_strength.jpg"));
                        break;
                    case TrainingProgramGoal.WeightLoss:
                        images[i].Source = new BitmapImage(new Uri("ms-appx:///Images/inside_fat.jpg"));
                        break;
                }
            }

            Save.IsEnabled = true;

        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstOption == sender)
            {
                FirstStack.BorderThickness = new Thickness(4);
            }
            else
            {
                FirstStack.BorderThickness = new Thickness(0);
                FirstOption.IsChecked = false;
            }
            if (SecondOption == sender)
            {
                SecondStack.BorderThickness = new Thickness(4);
            }
            else
            {
                SecondStack.BorderThickness = new Thickness(0);
                SecondOption.IsChecked = false;
            }
            if (ThirdOption == sender)
            {
                ThirdStack.BorderThickness = new Thickness(4);
            }
            else
            {
                ThirdStack.BorderThickness = new Thickness(0);
                ThirdOption.IsChecked = false;
            }
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            Button[] buttons = {FirstButton, SecondButton, ThirdButton};
            for (int i = 0; i < 3; i++)
            {
                if(buttons[i] == sender)
                {
                    Frame.Navigate(typeof(TrainingProgramDetails.TrainingProgramDetails), optionalPrograms[i]);
                }
            }
        }
    }
}
