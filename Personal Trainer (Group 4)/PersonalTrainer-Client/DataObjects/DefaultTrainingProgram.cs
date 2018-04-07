using System;
using System.Collections.Generic;
using System.Linq;


namespace PersonalTrainer_Client.DataObjects
{
    public class DefaultTrainingProgram
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TrainingProgramType TrainingProgramType { get; set; }
        public TrainingProgramGoal TrainingProgramGoal { get; set; }
        public Difficulty Difficulty { get; set; }
        public int DurationInWeeks { get; set; }
        public string WorkoutIds { get; set; }
    }

    public enum TrainingProgramType
    {
        Gym = 0,
        Outside = 1,
        GymAndOutside = 2
    }

    public enum TrainingProgramGoal
    {
        Strength = 0,
        Size = 1,
        WeightLoss = 2,
        Fitness = 3,
        Speed = 4,
        Flexibility = 5
    }

    public enum Difficulty
    {
        Begginer = 0,
        Intermediate = 1,
        Expert = 2
    }

}