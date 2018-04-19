using System;
using System.Collections.Generic;
using System.Linq;


namespace PersonalTrainer_Client.DataObjects
{
    public class UserTrainingProgram
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TrainingProgramType TrainingProgramType { get; set; }
        public TrainingProgramGoal TrainingProgramGoal { get; set; }
        public Difficulty Difficulty { get; set; }
        public int DurationInWeeks { get; set; }
        public string SundayWorkoutId { get; set; }
        public string MondayWorkoutId { get; set; }
        public string TuesdayWorkoutId { get; set; }
        public string WednesdayWorkoutId { get; set; }
        public string ThursdayWorkoutId { get; set; }
        public string FridayWorkoutId { get; set; }
        public string SaturdayWorkoutId { get; set; }

        public string getFirstNotEmptyWorkoutId()
        {
            if (!String.IsNullOrEmpty(SundayWorkoutId))
            {
                return SundayWorkoutId;
            }
            else if (!String.IsNullOrEmpty(MondayWorkoutId))
            {
                return MondayWorkoutId;
            }
            else if (!String.IsNullOrEmpty(TuesdayWorkoutId))
            {
                return TuesdayWorkoutId;
            }
            else if (!String.IsNullOrEmpty(WednesdayWorkoutId))
            {
                return WednesdayWorkoutId;
            }
            else if (!String.IsNullOrEmpty(ThursdayWorkoutId))
            {
                return ThursdayWorkoutId;
            }
            else if (!String.IsNullOrEmpty(FridayWorkoutId))
            {
                return FridayWorkoutId;
            }
            else
            {
                return SaturdayWorkoutId;
            }
        }

    }

}