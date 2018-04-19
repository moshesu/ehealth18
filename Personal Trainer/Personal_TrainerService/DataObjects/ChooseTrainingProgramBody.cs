using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personal_TrainerService.DataObjects
{
    public class ChooseTrainingProgramBody
    {
        public User User;
        public List<DefaultTrainingProgram> AllTrainingPrograms;
    }
}