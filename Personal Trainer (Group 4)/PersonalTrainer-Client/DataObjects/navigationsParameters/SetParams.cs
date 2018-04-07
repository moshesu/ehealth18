using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalTrainer_Client.DataObjects.navigationsParameters
{
    class SetParams
    {
        public static DefaultExercise exercise;

        public DefaultWorkout CurrentWorkout { get; set; }
        public int currentExerciseIndex { get; set; }
        public DefaultExercise Exercise { get; set; }
        public int numbrtOfSetsRemained { get; set; }

        public SetParams(DefaultWorkout currentWorkout, int currentExerciseIndex, DefaultExercise exercise, int numbrtOfSetsRemained)
        {
            CurrentWorkout = currentWorkout;
            this.currentExerciseIndex = currentExerciseIndex;
            Exercise = exercise;
            this.numbrtOfSetsRemained = numbrtOfSetsRemained;
        }
    }
}
