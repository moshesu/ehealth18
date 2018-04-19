using System;
using System.Collections.Generic;
using System.Linq;


namespace PersonalTrainer_Client.DataObjects
{
    public class DefaultWorkout
    {
        public string Id { get; set; }
        public int EstiamtedDurationInMinutes { get; set; }
        public string ExercisesIds { get; set; }
        public string ExercisesSets { get; set; }
        public string ExercisesReps { get; set; }
        public string ExercisesWeight { get; set; }
    }
}