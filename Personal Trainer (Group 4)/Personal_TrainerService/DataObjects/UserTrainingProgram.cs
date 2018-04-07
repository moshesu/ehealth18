using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace Personal_TrainerService.DataObjects
{
    public class UserTrainingProgram : EntityData
    {
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
    }

}