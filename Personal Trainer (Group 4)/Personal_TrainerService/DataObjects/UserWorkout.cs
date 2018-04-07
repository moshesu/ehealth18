using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace Personal_TrainerService.DataObjects
{
    public class UserWorkout : EntityData
    {
        public string EstiamtedDurationInMinutes { get; set; }
        public string ExercisesId { get; set; }
    }
}