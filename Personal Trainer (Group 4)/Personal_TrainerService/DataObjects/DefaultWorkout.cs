using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace Personal_TrainerService.DataObjects
{
    public class DefaultWorkout : EntityData
    {
        public int EstiamtedDurationInMinutes { get; set; }
        public string ExercisesIds { get; set; }
        public string ExercisesSets { get; set; }
        public string ExercisesReps { get; set; }
        public string ExercisesWeight { get; set; }
    }
}