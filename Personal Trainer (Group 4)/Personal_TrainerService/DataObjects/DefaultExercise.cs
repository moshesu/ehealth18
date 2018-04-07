using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace Personal_TrainerService.DataObjects
{
    public class DefaultExercise : EntityData
    {
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