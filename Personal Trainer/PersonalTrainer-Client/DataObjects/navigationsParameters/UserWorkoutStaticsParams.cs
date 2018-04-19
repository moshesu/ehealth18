using Personal_TrainerService.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalTrainer_Client.DataObjects.navigationsParameters
{

    class UserWorkoutStaticsParams
    {

        public static string WorkoutId { get; set; }
        public static bool Myself { get; set; }

        //public UserWorkoutStaticsParams(String workoutId, bool myself)
        //{
        //    WorkoutId = workoutId;
        //    Myself = myself;
        //}

        //public String WorkoutId { get; set; }
        //public bool Myself { get; set; }
    }
}
