namespace Personal_TrainerService.DataObjects
{
    public class UserWorkoutStatistics
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string WorkoutId { get; set; }
        public int PerformanceNumberCount { get; set; }
        public double PerformanceTimeAvarage { get; set; }
        public double HeartReatAvarage { get; set; }
        public double MaximalHeartRateAvarage { get; set; }
        public double CaloriesBurnedAvarage { get; set; }
        public double GalvanicResponseAvarage { get; set; }
        public double DistanceAvarage { get; set; }
        public double StepsAvarage { get; set; }

        // Represents number of exercise performed in average(rounded to int)
        public double GeneralColumn1 { get; set; }
        public double GeneralColumns2 { get; set; }
    }
}