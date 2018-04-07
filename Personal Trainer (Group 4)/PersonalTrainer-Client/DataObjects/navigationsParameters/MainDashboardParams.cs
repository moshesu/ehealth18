using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalTrainer_Client.DataObjects.navigationsParameters
{
    public class MainDashboardParams
    {
        public MainDashboardParams(User user)
        {
            User = user;
        }

        public MainDashboardParams(User user, DefaultWorkout currentWorkout, bool isCurrentWorkoutFinished) 
            : this(user)
        {
            CurrentWorkout = currentWorkout;
            IsCurrentWorkoutFinished = isCurrentWorkoutFinished;
        }

        public User User { get; set;}
        public DefaultWorkout CurrentWorkout { get; set; }
        public bool IsCurrentWorkoutFinished { get; set; }
    }
}
