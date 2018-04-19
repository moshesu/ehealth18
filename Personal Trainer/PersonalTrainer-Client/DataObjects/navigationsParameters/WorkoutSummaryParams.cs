using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalTrainer_Client.DataObjects.navigationsParameters
{
    class WorkoutSummaryParams
    {
        public static int numberOfExercisesPerformed;
        public static int numberOfExercisesTotal;
        public static string WorkoutId;
        public static bool isFirstNavigation = true;

        //public WorkoutSummaryParams(int numberOfExercisesPerformed, int numberOfExercisesTotal, string workoutId)
        //{
        //    this.numberOfExercisesPerformed = numberOfExercisesPerformed;
        //    this.numberOfExercisesTotal = numberOfExercisesTotal;
        //    this.WorkoutId = workoutId;
        //}
    }
}
