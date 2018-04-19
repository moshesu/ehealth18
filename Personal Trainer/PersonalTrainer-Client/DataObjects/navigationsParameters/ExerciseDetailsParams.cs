using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalTrainer_Client.DataObjects.navigationsParameters
{
    class ExerciseDetailsParams
    {
        public DefaultExercise ex;
        public UserTrainingProgram trainingProgram;

        public ExerciseDetailsParams(DefaultExercise ex, UserTrainingProgram trainingProgram)
        {
            this.ex = ex;
            this.trainingProgram = trainingProgram;
        }
    }
}
