using PersonalTrainer_Client.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Personal_TrainerService.DataObjects
{
    public class ChooseTrainingProgramBody
    {
        public User User;
        public List<DefaultTrainingProgram> AllTrainingPrograms;
    }
}