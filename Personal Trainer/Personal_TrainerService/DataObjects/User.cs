using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace Personal_TrainerService.DataObjects
{
    public class User : EntityData
    {
        public string Name { get; set; }
        public string AuthenticationKey { get; set; }
        public string CreationDate { get; set; }
        public string Age { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public Gender Gender { get; set; }
        public string PreferredWorkoutDays { get; set; }
        public TrainingProgramGoal PreferredTrainingGoal { get; set; }
        public TrainingProgramType PreferredTrainingProgramType { get; set; }
        public Difficulty PreferredTrainingProgramDifficulty { get; set; }
        public string CurrentTrainingProgramId { get; set; }
        public string CurrentTrainingProgramPeformanceId { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }

}