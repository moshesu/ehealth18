using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalTrainer_Client.DataObjects.navigationsParameters
{
    class WorkoutDetailsParams
    {
        public string workoutId;
        public UserTrainingProgram trainingProgram;
        public DefaultWorkout workout;

        public WorkoutDetailsParams(string workoutId, UserTrainingProgram trainingProgram)
        {
            this.workoutId = workoutId;
            this.trainingProgram = trainingProgram;
        }


        public WorkoutDetailsParams(DefaultWorkout workout, UserTrainingProgram trainingProgram)
        {
            this.workout = workout;
            this.trainingProgram = trainingProgram;
        }
    }
}
