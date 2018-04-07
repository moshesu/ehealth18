using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Personal_TrainerService.DataObjects;

namespace Personal_TrainerService.Controllers
{
    [MobileAppController]
    public class ChooseTrainingProgramController : ApiController
    {

        /// <summary>
        /// Calculate suggested training programs based on user details
        /// </summary>
        /// <param name="body">Request Body</param>
        /// <returns></returns>
        [HttpPost]
        public List<UserTrainingProgram> PostAsync([FromBody] ChooseTrainingProgramBody body)
        {


            var userPreferredWorkoutDays = body.User.PreferredWorkoutDays.Split(';');

            //First, filter by preferred training program type
            var optionalDefaultProgramsByType = body.AllTrainingPrograms.Where(program => program.TrainingProgramType == body.User.PreferredTrainingProgramType);

            // Weight functions definitions
            TrainingProgramGoal[] goalsDistance = { TrainingProgramGoal.Speed, TrainingProgramGoal.Flexibility, TrainingProgramGoal.WeightLoss, TrainingProgramGoal.Fitness, TrainingProgramGoal.Strength, TrainingProgramGoal.Size };
            Difficulty[] difficulties = { Difficulty.Begginer, Difficulty.Intermediate, Difficulty.Expert };

            double firstPlaceGrade = 0;
            double secondPlaceGrade = 0;
            double thirdPlaceGrade = 0;

            DefaultTrainingProgram firstPlace = null;
            DefaultTrainingProgram secondPlace = null;
            DefaultTrainingProgram thirdPlace = null;

            // Calculate grades for all programs
            foreach (var program in body.AllTrainingPrograms)
            {
                double currentProgramGoalWeight = goalsDistance.ToList().FindIndex(goal => goal.Equals(program.TrainingProgramGoal));
                double userPreferredGoalWeight = goalsDistance.ToList().FindIndex(goal => goal.Equals(body.User.PreferredTrainingGoal));

                double currentProgramDiffiucaltyWeight = difficulties.ToList().FindIndex(difficulty => difficulty.Equals(program.Difficulty));
                double userPreferredDiffiucaltyWeight = difficulties.ToList().FindIndex(difficulty => difficulty.Equals(body.User.PreferredTrainingProgramDifficulty));

                double goalsGrade = 1 - (Math.Abs(currentProgramGoalWeight - userPreferredGoalWeight) / goalsDistance.Count());
                double diffiucaltyGrade = 1 - (Math.Abs(currentProgramDiffiucaltyWeight - userPreferredDiffiucaltyWeight) / difficulties.Count());

                double totalProgramGrade = 6 * goalsGrade + diffiucaltyGrade;

                // Check if program's grade is bigger then the max grades
                if (totalProgramGrade > firstPlaceGrade)
                {
                    thirdPlaceGrade = secondPlaceGrade;
                    secondPlaceGrade = firstPlaceGrade;
                    firstPlaceGrade = totalProgramGrade;

                    thirdPlace = secondPlace;
                    secondPlace = firstPlace;
                    firstPlace = program;
                }
                else if (totalProgramGrade > secondPlaceGrade)
                {

                    thirdPlaceGrade = secondPlaceGrade;
                    secondPlaceGrade = totalProgramGrade;

                    thirdPlace = secondPlace;
                    secondPlace = program;
                }
                else if (totalProgramGrade > thirdPlaceGrade)
                {
                    thirdPlace = program;
                    thirdPlaceGrade = totalProgramGrade;
                }
            }

            var optionalUserPrograms = new List<UserTrainingProgram>();

            var firstSuggestedProgram = CreateUserTrainingProgram(firstPlace, body.User);
            optionalUserPrograms.Add(firstSuggestedProgram);

            var secondSuggestedProgram = CreateUserTrainingProgram(secondPlace, body.User);
            optionalUserPrograms.Add(secondSuggestedProgram);

            var thirdSuggestedProgram = CreateUserTrainingProgram(thirdPlace, body.User);
            optionalUserPrograms.Add(thirdSuggestedProgram);


            return optionalUserPrograms;
        }


        private UserTrainingProgram CreateUserTrainingProgram(DefaultTrainingProgram defaultProgram, User user)
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

    }
}
