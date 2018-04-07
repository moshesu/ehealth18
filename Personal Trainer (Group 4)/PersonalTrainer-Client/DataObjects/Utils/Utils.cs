using Personal_TrainerService.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalTrainer_Client.DataObjects.Utils
{
    public class Utils
    {
        public static UserWorkoutStatistics GetUserWorkoutStatisticsCopyWithDifferentId(UserWorkoutStatistics source)
        {
            return new UserWorkoutStatistics()
            {
                UserId = source.UserId,
                WorkoutId = source.WorkoutId,
                CaloriesBurnedAvarage = source.CaloriesBurnedAvarage,
                DistanceAvarage = source.DistanceAvarage,
                GalvanicResponseAvarage = source.GalvanicResponseAvarage,
                GeneralColumn1 = source.GeneralColumn1,
                GeneralColumns2 = source.GeneralColumns2, 
                Id = Guid.NewGuid().ToString(),
                HeartReatAvarage = source.HeartReatAvarage,
                MaximalHeartRateAvarage = source.MaximalHeartRateAvarage,
                PerformanceNumberCount = source.PerformanceNumberCount,
                PerformanceTimeAvarage = source.PerformanceTimeAvarage, 
                StepsAvarage = source.StepsAvarage
            };
        }

        public static double CalculatePrevWorkoutsPerformanceTimeAverage(List<UserWorkoutStatistics> prevWorkoutsStatistics)
        {


            var prevWorkoutsPerformanceTimeAvarage = 0.0;
            prevWorkoutsStatistics.ForEach(stat => prevWorkoutsPerformanceTimeAvarage += stat.PerformanceTimeAvarage);
            prevWorkoutsPerformanceTimeAvarage = prevWorkoutsPerformanceTimeAvarage / prevWorkoutsStatistics.Count;

            return Math.Round(prevWorkoutsPerformanceTimeAvarage, 2);
        }

        public static double CalculatePrevWorkoutsExNumAverage(List<UserWorkoutStatistics> prevWorkoutsStatistics)
        {
            var prevWorkoutsExNumPerformedAverage = 0.0;
            prevWorkoutsStatistics.ForEach(stat => prevWorkoutsExNumPerformedAverage += stat.GeneralColumn1);
            prevWorkoutsExNumPerformedAverage = Math.Round(prevWorkoutsExNumPerformedAverage / prevWorkoutsStatistics.Count);

            return Math.Round(prevWorkoutsExNumPerformedAverage);
        }

        public static double CalculatePrevWorkoutsAverageHeartRateAverage(List<UserWorkoutStatistics> prevWorkoutsStatistics)
        {
            var prevWorkoutsAverageHeartRateAverage = 0.0;
            prevWorkoutsStatistics.ForEach(stat => prevWorkoutsAverageHeartRateAverage += stat.HeartReatAvarage);
            prevWorkoutsAverageHeartRateAverage = Math.Round(prevWorkoutsAverageHeartRateAverage / prevWorkoutsStatistics.Count);

            return Math.Round(prevWorkoutsAverageHeartRateAverage);
        }

        public static double CalculatePrevWorkoutsPeakHeartRateAverage(List<UserWorkoutStatistics> prevWorkoutsStatistics)
        {
            var prevWorkoutsPeakHeartRateAverage = 0.0;
            prevWorkoutsStatistics.ForEach(stat => prevWorkoutsPeakHeartRateAverage += stat.MaximalHeartRateAvarage);
            prevWorkoutsPeakHeartRateAverage = Math.Round(prevWorkoutsPeakHeartRateAverage / prevWorkoutsStatistics.Count);

            return Math.Round(prevWorkoutsPeakHeartRateAverage);
        }

        public static double CalculatePrevWorkoutsCaloriesAverage(List<UserWorkoutStatistics> prevWorkoutsStatistics)
        {
            var prevWorkoutsCaloriesAverage = 0.0;
            prevWorkoutsStatistics.ForEach(stat => prevWorkoutsCaloriesAverage += stat.CaloriesBurnedAvarage);
            prevWorkoutsCaloriesAverage = Math.Round(prevWorkoutsCaloriesAverage / prevWorkoutsStatistics.Count);

            return Math.Round(prevWorkoutsCaloriesAverage);
        }
    }
}
