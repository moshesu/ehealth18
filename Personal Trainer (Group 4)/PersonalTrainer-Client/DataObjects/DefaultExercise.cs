using System;
using System.Collections.Generic;
using System.Linq;


namespace PersonalTrainer_Client.DataObjects
{
    public class DefaultExercise
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public int BegginerSets { get; set; }
        public int IntermediateSets { get; set; }
        public int ExpertSets { get; set; }
        public int BegginerReps { get; set; }
        public int IntermediateReps { get; set; }
        public int ExpertReps { get; set; }
        public int BegginerWeight { get; set; }
        public int IntermediateWeight { get; set; }
        public int ExpertWeight { get; set; }
        public string HowToPerform { get; set; }
        public string GuidnaceImages { get; set; }
        public string MusclesImage { get; set; }
        public Muscle Muscle { get; set; }
        public string Equipment { get; set; }


        public string getMuscleName()
        {
            switch (Muscle)
            {
                case Muscle.Abs:
                    return "Abs";
                case Muscle.Back:
                    return "Back";
                case Muscle.Biceps:
                    return "Biceps";
                case Muscle.Chest:
                    return "Chest";
                case Muscle.Legs:
                    return "Legs";
                case Muscle.Shoulders:
                    return "Shoulders";
                case Muscle.Triceps:
                    return "Triceps";
                default:
                    return null;
            }
        }
    }

    public enum ExerciseType
    {
        RepsWithWeight = 0,
        RepsWithoutWeight = 1,
        Running = 2
    }

    public enum Muscle
    {
        Chest = 0,
        Back = 1,
        Biceps = 2,
        Triceps = 3,
        Shoulders = 4,
        Legs = 5,
        Abs = 6
    }

}