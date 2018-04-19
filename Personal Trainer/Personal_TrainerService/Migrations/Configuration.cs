namespace Personal_TrainerService.Migrations
{
    using Microsoft.Azure.Mobile.Server.Tables;
    using Personal_TrainerService.DataObjects;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Personal_TrainerService.Models.Personal_TrainerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(Personal_TrainerService.Models.Personal_TrainerContext context)
        {
            String ex1 = Guid.NewGuid().ToString();
            String ex2 = Guid.NewGuid().ToString();
            String ex3 = Guid.NewGuid().ToString();
            String ex4 = Guid.NewGuid().ToString();
            String ex5 = Guid.NewGuid().ToString();
            String ex6 = Guid.NewGuid().ToString();
            String ex7 = Guid.NewGuid().ToString();
            String ex8 = Guid.NewGuid().ToString();
            String ex9 = Guid.NewGuid().ToString();
            String ex10 = Guid.NewGuid().ToString();
            String ex11 = Guid.NewGuid().ToString();
            String ex12 = Guid.NewGuid().ToString();
            String ex13 = Guid.NewGuid().ToString();
            String ex14 = Guid.NewGuid().ToString();
            String ex15 = Guid.NewGuid().ToString();
            String ex16 = Guid.NewGuid().ToString();
            String ex17 = Guid.NewGuid().ToString();
            String ex18 = Guid.NewGuid().ToString();
            String ex19 = Guid.NewGuid().ToString();
            String ex20 = Guid.NewGuid().ToString();
            String ex21 = Guid.NewGuid().ToString();
            String ex22 = Guid.NewGuid().ToString();
            String ex23 = Guid.NewGuid().ToString();
            List<DefaultExercise> exercises = new List<DefaultExercise>
            {
                new DefaultExercise {Id = ex1, Name = "Dumbbell Bench Press", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Lie down on a flat bench with a dumbbell in each hand resting on top of your thighs. The palms of your hands will be facing each other. Then, using your thighs to help raise the dumbbells up, lift the dumbbells one at a time so that you can hold them in front of you at shoulder width. Once at shoulder width, rotate your wrists forward so that the palms of your hands are facing away from you.The dumbbells should be just to the sides of your chest, with your upper arm and forearm creating a 90 degree angle. Be sure to maintain full control of the dumbbells at all times. This will be your starting position. Then, as you breathe out, use your chest to push the dumbbells up. Lock your arms at the top of the lift and squeeze your chest, hold for a second and then begin coming down slowly.Tip: Ideally, lowering the weight should take about twice as long as raising it. Repeat the movement for the prescribed amount of repetitions of your training program.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/1/Male/m/1_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/1/Male/m/1_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-1.gif",
                    Muscle = Muscle.Chest, Equipment = "Dumbbell" },


                new DefaultExercise {Id = ex2, Name = "Pushups", ExerciseType = ExerciseType.RepsWithoutWeight,
                    BegginerSets = 3, IntermediateSets = 3, ExpertSets = 4,
                    BegginerReps = 10, IntermediateReps = 15, ExpertReps = 20,
                    BegginerWeight = 0, IntermediateWeight = 0, ExpertWeight = 0,
                    HowToPerform = "Lie on the floor face down and place your hands about 36 inches apart while holding your torso up at arms length. Next, lower yourself downward until your chest almost touches the floor as you inhale. Now breathe out and press your upper body back up to the starting position while squeezing your chest. After a brief pause at the top contracted position, you can begin to lower yourself downward again for as many repetitions as needed.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/70/Male/m/70_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/70/Male/m/70_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-1.gif",
                    Muscle = Muscle.Chest, Equipment = "None" },


                new DefaultExercise {Id = ex3, Name = "Wide-Grip Barbell Bench Press", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Lie back on a flat bench with feet firm on the floor. Using a wide, pronated (palms forward) grip that is around 3 inches away from shoulder width (for each hand), lift the bar from the rack and hold it straight over you with your arms locked. The bar will be perpendicular to the torso and the floor. This will be your starting position. As you breathe in, come down slowly until you feel the bar on your middle chest. After a second pause, bring the bar back to the starting position as you breathe out and push the bar using your chest muscles.Lock your arms and squeeze your chest in the contracted position, hold for a second and then start coming down slowly again.Tip: It should take at least twice as long to go down than to come up. Repeat the movement for the prescribed amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/33/Male/m/33_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/33/Male/m/33_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-1.gif",
                    Muscle = Muscle.Chest, Equipment = "Barbell" },


                new DefaultExercise {Id = ex4, Name = "Reverse Grip Bent-Over Rows", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Stand erect while holding a barbell with a supinated grip (palms facing up). Bend your knees slightly and bring your torso forward, by bending at the waist, while keeping the back straight until it is almost parallel to the floor.Tip: Make sure that you keep the head up. The barbell should hang directly in front of you as your arms hang perpendicular to the floor and your torso.This is your starting position. While keeping the torso stationary, lift the barbell as you breathe out, keeping the elbows close to the body and not doing any force with the forearm other than holding the weights. On the top contracted position, squeeze the back muscles and hold for a second. Slowly lower the weight again to the starting position as you inhale. Repeat for the recommended amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/128/Male/m/128_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/128/Male/m/128_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-4.gif",
                    Muscle = Muscle.Back, Equipment = "Barbell" },


                new DefaultExercise {Id = ex5, Name = "Seated Cable Rows", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "For this exercise you will need access to a low pulley row machine with a V-bar. Note: The V-bar will enable you to have a neutral grip where the palms of your hands face each other. To get into the starting position, first sit down on the machine and place your feet on the front platform or crossbar provided making sure that your knees are slightly bent and not locked. Lean over as you keep the natural alignment of your back and grab the V-bar handles. With your arms extended pull back until your torso is at a 90-degree angle from your legs. Your back should be slightly arched and your chest should be sticking out. You should be feeling a nice stretch on your lats as you hold the bar in front of you. This is the starting position of the exercise.Keeping the torso stationary, pull the handles back towards your torso while keeping the arms close to it until you touch the abdominals. Breathe out as you perform that movement.At that point you should be squeezing your back muscles hard.Hold that contraction for a second and slowly go back to the original position while breathing in.Repeat for the recommended amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/45/Male/m/45_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/45/Male/m/45_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-4.gif",
                    Muscle = Muscle.Back, Equipment = "Cable" },


                new DefaultExercise {Id = ex6, Name = "Leverage High Row", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Load an appropriate weight onto the pins and adjust the seat height so that you can just reach the handles above you. Adjust the knee pad to help keep you down. Grasp the handles with a pronated grip. This will be your starting position. Pull the handles towards your torso, retracting your shoulder blades as you flex the elbow. Pause at the bottom of the motion, and then slowly return the handles to the starting position. For multiple repetitions, avoid completely returning the weight to the stops to keep tension on the muscles being worked.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/892/Male/m/892_2.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/892/Male/m/892_1.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-4.gif",
                    Muscle = Muscle.Back, Equipment = "Machine" },


                new DefaultExercise {Id = ex7, Name = "Wide-Grip Standing Barbell Curl", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Stand up with your torso upright while holding a barbell at the wide outer handle. The palm of your hands should be facing forward. The elbows should be close to the torso. This will be your starting position. While holding the upper arms stationary, curl the weights forward while contracting the biceps as you breathe out. Tip: Only the forearms should move. Continue the movement until your biceps are fully contracted and the bar is at shoulder level.Hold the contracted position for a second and squeeze the biceps hard. Slowly begin to bring the bar back to starting position as your breathe in. Repeat for the recommended amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/287/Male/m/287_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/287/Male/m/287_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-15.gif",
                    Muscle = Muscle.Biceps, Equipment = "Barbell" },


                new DefaultExercise {Id = ex8, Name = "Overhead Cable Curl", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "To begin, set a weight that is comfortable on each side of the pulley machine. Note: Make sure that the amount of weight selected is the same on each side. Now adjust the height of the pulleys on each side and make sure that they are positioned at a height higher than that of your shoulders. Stand in the middle of both sides and use an underhand grip (palms facing towards the ceiling) to grab each handle. Your arms should be fully extended and parallel to the floor with your feet positioned shoulder width apart from each other. Your body should be evenly aligned the handles. This is the starting position. While exhaling, slowly squeeze the biceps on each side until your forearms and biceps touch. While inhaling, move your forearms back to the starting position. Note: Your entire body is stationary during this exercise except for the forearms. Repeat for the recommended amount of repetitions prescribed in your program.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/213/Male/m/213_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/213/Male/m/213_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-15.gif",
                    Muscle = Muscle.Biceps, Equipment = "Cable" },


                new DefaultExercise {Id = ex9, Name = "Machine Preacher Curls", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Sit down on the Preacher Curl Machine and select the weight. Place the back of your upper arms (your triceps) on the preacher pad provided and grab the handles using an underhand grip(palms facing up).Tip: Make sure that when you place the arms on the pad you keep the elbows in. This will be your starting position. Now lift the handles as you exhale and you contract the biceps. At the top of the position make sure that you hold the contraction for a second. Tip: Only the forearms should move.The upper arms should remain stationary and on the pad at all times. Lower the handles slowly back to the starting position as you inhale. Repeat for the recommended amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/148/Male/m/148_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/148/Male/m/148_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-15.gif",
                    Muscle = Muscle.Biceps, Equipment = "Machine" },


                new DefaultExercise {Id = ex10, Name = "Dips - Triceps Version", ExerciseType = ExerciseType.RepsWithoutWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 6, IntermediateReps = 6, ExpertReps = 8,
                    BegginerWeight = 0, IntermediateWeight = 0, ExpertWeight = 0,
                    HowToPerform = "To get into the starting position, hold your body at arm's length with your arms nearly locked above the bars. Now, inhale and slowly lower yourself downward. Your torso should remain upright and your elbows should stay close to your body. This helps to better focus on tricep involvement. Lower yourself until there is a 90 degree angle formed between the upper arm and forearm. Then, exhale and push your torso back up using your triceps to bring your body back to the starting position. Repeat the movement for the prescribed amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/55/Male/m/55_2.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/55/Male/m/55_1.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-10.gif",
                    Muscle = Muscle.Triceps, Equipment = "None" },


                new DefaultExercise {Id = ex11, Name = "Band Skull Crusher", ExerciseType = ExerciseType.RepsWithoutWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 0, IntermediateWeight = 0, ExpertWeight = 0,
                    HowToPerform = "Secure a band to the base of a rack or the bench. Lay on the bench so that the band is lined up with your head. Take hold of the band, raising your elbows so that the upper arm is perpendicular to the floor. With the elbow flexed, the band should be above your head. This will be your starting position. Extend through the elbow to straighten your arm, keeping your upper arm in place. Pause at the top of the motion, and return to the starting position.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/1211/Male/m/1211_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/1211/Male/m/1211_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-10.gif",
                    Muscle = Muscle.Triceps, Equipment = "Bands" },


                new DefaultExercise {Id = ex12, Name = "Seated Triceps Press", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 25,
                    HowToPerform = "Sit down on a bench with back support and grasp a dumbbell with both hands and hold it overhead at arm's length. Tip: a better way is to have somebody hand it to you especially if it is very heavy. The resistance should be resting in the palms of your hands with your thumbs around it. The palm of the hand should be facing inward. This will be your starting position. Keeping your upper arms close to your head (elbows in) and perpendicular to the floor, lower the resistance in a semi-circular motion behind your head until your forearms touch your biceps. Tip: The upper arms should remain stationary and only the forearms should move. Breathe in as you perform this step. Go back to the starting position by using the triceps to raise the dumbbell. Breathe out as you perform this step. Repeat for the recommended amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/341/Male/m/341_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/341/Male/m/341_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-10.gif",
                    Muscle = Muscle.Triceps, Equipment = "Dumbbell" },


                new DefaultExercise {Id = ex13, Name = "Single-Arm Linear Jammer", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 8, IntermediateReps = 8, ExpertReps = 8,
                    BegginerWeight = 5, IntermediateWeight = 10, ExpertWeight = 15,
                    HowToPerform = "Position a bar into a landmine or securely anchor it in a corner. Load the bar to an appropriate weight. Raise the bar from the floor, taking it to your shoulders with one or both hands. Adopt a wide stance. This will be your starting position. Perform the movement by extending the elbow, pressing the weight up. Move explosively, extending the hips and knees fully to produce maximal force. Return to the starting position.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/1741/Male/m/1741_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/1741/Male/m/1741_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-12.gif",
                    Muscle = Muscle.Shoulders, Equipment = "Barbell" },


                new DefaultExercise {Id = ex14, Name = "Leverage Shoulder Press", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 8, IntermediateReps = 8, ExpertReps = 8,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Load an appropriate weight onto the pins and adjust the seat for your height. The handles should be near the top of the shoulders at the beginning of the motion. Your chest and head should be up and handles held with a pronated grip. This will be your starting position. Press the handles upward by extending through the elbow. After a brief pause at the top, return the weight to just above the start position, keeping tension on the muscles by not returning the weight to the stops until the set is complete.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/897/Male/m/897_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/897/Male/m/897_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-12.gif",
                    Muscle = Muscle.Shoulders, Equipment = "Machine" },


                new DefaultExercise {Id = ex15, Name = "Power Partials", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 8, IntermediateReps = 8, ExpertReps = 8,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Stand up with your torso upright and a dumbbell on each hand being held at arms length. The elbows should be close to the torso. The palms of the hands should be facing your torso. Your feet should be about shoulder width apart. This will be your starting position. Keeping your arms straight and the torso stationary, lift the weights out to your sides until they are about shoulder level height while exhaling. Feel the contraction for a second and begin to lower the weights back down to the starting position while inhaling.Tip: Keep the palms facing down with the little finger slightly higher while lifting and lowering the weights as it will concentrate the stress on your shoulders mainly. Repeat for the recommended amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/270/Male/m/270_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/270/Male/m/270_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-12.gif",
                    Muscle = Muscle.Shoulders, Equipment = "Dumbbell" },


                new DefaultExercise {Id = ex16, Name = "Romanian Deadlift With Dumbbells", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Begin in a standing position with a dumbbell in each hand. Ensure that your back is straight and stays that way for the duration of the exercise. Allow your arms to hang perpendicular to the floor, with the wrists pronated and the elbows pointed to your sides. This will be your starting position. Initiate the movement by flexing your hips, slowly pushing your butt as far back as you can. This should entail a horizontal movement of the hips, rather than a downward movement. The knees should only partially bend, and your weight should remain on your heels. Drive your butt back as far as you can, which should generate tension in your hamstrings as your hands approach knee level. Maintain an arch in your back throughout the exercise. When your hips cannot perform any further backward movement, pause, and then slowly return to the starting position by extending the hips.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/3791/Male/m/3791_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/3791/Male/m/3791_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-8.gif",
                    Muscle = Muscle.Legs, Equipment = "Dumbbell" },


                new DefaultExercise {Id = ex17, Name = "Floor Glute-Ham Raise", ExerciseType = ExerciseType.RepsWithoutWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 8, IntermediateReps = 8, ExpertReps = 8,
                    BegginerWeight = 0, IntermediateWeight = 0, ExpertWeight = 0,
                    HowToPerform = "You can use a partner for this exercise or brace your feet under something stable. Begin on your knees with your upper legs and torso upright. If using a partner, they will firmly hold your feet to keep you in position.This will be your starting position. Lower yourself by extending at the knee, taking care to NOT flex the hips as you go forward. Place your hands in front of you as you reach the floor.This movement is very difficult and you may be unable to do it unaided. Use your arms to lightly push off the floor to aid your return to the starting position.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/1121/Male/m/1121_4.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/1121/Male/m/1121_3.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-8.gif",
                    Muscle = Muscle.Legs, Equipment = "None" },


                new DefaultExercise {Id = ex18, Name = "Single-Leg Press", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 10, IntermediateWeight = 20, ExpertWeight = 30,
                    HowToPerform = "Load the sled to an appropriate weight. Seat yourself on the machine, planting one foot on the platform in line with your hip. Your free foot can be placed on the ground. Maintain good spinal position with your head and chest up. Supporting the weight, fully extend the knee and unlock the sled. This will be your starting position. Lower the weight by flexing the hip and knee, continuing as far as flexibility allows. Do not allow your lumbar to take the load by moving your pelvis. At the bottom of the motion pause briefly and then return to the starting position by extending the hip and knee. Complete all repetitions for one leg before switching to the other.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/3941/Male/m/3941_2.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/3941/Male/m/3941_1.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-7.gif",
                    Muscle = Muscle.Legs, Equipment = "Machine" },


                new DefaultExercise {Id = ex19, Name = "Bodyweight Lunge", ExerciseType = ExerciseType.RepsWithoutWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 8, IntermediateReps = 10, ExpertReps = 12,
                    BegginerWeight = 0, IntermediateWeight = 0, ExpertWeight = 0,
                    HowToPerform = "Stand erect with your feet hip-width apart, chest out, and shoulders back, maintaining the natural curvature of your spine. Your knees should be unlocked and your hand on your hips. This is your starting position. Take a moderate-length step forward with one foot, descending to a point in which your rear knee approaches the floor without touching, maintaining your body's upright posture. Your front knee should bend about 90 degrees, but for knee health it should not be forward of the vertical plane that extends straight up from your toes. If so, take a slightly longer step. From the bottom position, push back up from your forward foot, bringing it back beside the other. Repeat on the opposite side for the required number of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/4461/Male/m/4461_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/4461/Male/m/4461_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-7.gif",
                    Muscle = Muscle.Legs, Equipment = "None" },


                new DefaultExercise {Id = ex20, Name = "One-Arm Medicine Ball Slam", ExerciseType = ExerciseType.RepsWithoutWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 8, IntermediateReps = 10, ExpertReps = 12,
                    BegginerWeight = 0, IntermediateWeight = 0, ExpertWeight = 0,
                    HowToPerform = "Start in a standing position with a staggered, athletic stance. Hold a medicine ball in one hand, on the same side as your back leg. This will be your starting position. Begin by winding the arm, raising the medicine ball above your head. As you do so, extend through the hips, knees, and ankles to load up for the slam. At peak extension, flex the shoulders, spine, and hips to throw the ball hard into the ground directly in front of you. Catch the ball on the bounce and continue for the desired number of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/2081/Male/m/2081_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/2081/Male/m/2081_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-13.gif",
                    Muscle = Muscle.Abs, Equipment = "Medicine Ball" },


                new DefaultExercise {Id = ex21, Name = "Ab Crunch Machine", ExerciseType = ExerciseType.RepsWithWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 12, IntermediateReps = 12, ExpertReps = 12,
                    BegginerWeight = 8, IntermediateWeight = 12, ExpertWeight = 16,
                    HowToPerform = "Select a light resistance and sit down on the ab machine placing your feet under the pads provided and grabbing the top handles. Your arms should be bent at a 90 degree angle as you rest the triceps on the pads provided. This will be your starting position. At the same time, begin to lift the legs up as you crunch your upper torso. Breathe out as you perform this movement. Tip: Be sure to use a slow and controlled motion.Concentrate on using your abs to move the weight while relaxing your legs and feet. After a second pause, slowly return to the starting position as you breathe in. Repeat the movement for the prescribed amount of repetitions.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/225/Male/m/225_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/225/Male/m/225_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-13.gif",
                    Muscle = Muscle.Abs, Equipment = "Machine" },


                new DefaultExercise {Id = ex22, Name = "Spider Crawl", ExerciseType = ExerciseType.RepsWithoutWeight,
                    BegginerSets = 3, IntermediateSets = 4, ExpertSets = 5,
                    BegginerReps = 8, IntermediateReps = 12, ExpertReps = 16,
                    BegginerWeight = 0, IntermediateWeight = 0, ExpertWeight = 0,
                    HowToPerform = "Begin in a prone position on the floor. Support your weight on your hands and toes, with your feet together and your body straight. Your arms should be bent to 90 degrees. This will be your starting position. Initiate the movement by raising one foot off of the ground. Externally rotate the leg and bring the knee toward your elbow, as far forward as possible. Return this leg to the starting position and repeat on the opposite side.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/2061/Male/m/2061_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/2061/Male/m/2061_2.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-13.gif",
                    Muscle = Muscle.Abs, Equipment = "None" },


                new DefaultExercise {Id = ex23, Name = "Outdoor Running", ExerciseType = ExerciseType.Running,
                    BegginerSets = 1, IntermediateSets = 1, ExpertSets = 1,
                    BegginerReps = 8, IntermediateReps = 10, ExpertReps = 12,
                    BegginerWeight = 1, IntermediateWeight = 3, ExpertWeight = 5,
                    HowToPerform = "Running or hiking on trails will get the blood pumping and heart beating almost immediately. Make sure you have good shoes. While you use the muscles in your calves and buttocks to pull yourself up a hill, the knees, joints and ankles absorb the bulk of the pounding coming back down. Take smaller steps as you walk downhill, keep your knees bent to reduce the impact and slow down to avoid falling. A 68 kg person can burn over 200 calories for 30 minutes walking uphill, compared to 175 on a flat surface. If running the trail, a 68 kg person can burn well over 500 calories in 30 minutes.",
                    GuidnaceImages ="https://www.bodybuilding.com/exercises/exerciseImages/sequences/657/Male/m/657_1.jpg;https://www.bodybuilding.com/exercises/exerciseImages/sequences/657/Male/m/657_3.jpg",
                    MusclesImage = "https://artifacts.bbcomcdn.com/exercises-app/1.1.0/img/guide-13.gif",
                    Muscle = Muscle.Legs, Equipment = "None" }
            };
            foreach (DefaultExercise exercise in exercises)
            {
                context.Set<DefaultExercise>().Add(exercise);
            }


            String wo1 = Guid.NewGuid().ToString();
            String wo2 = Guid.NewGuid().ToString();
            String wo3 = Guid.NewGuid().ToString();
            String wo4 = Guid.NewGuid().ToString();
            String wo5 = Guid.NewGuid().ToString();
            String wo6 = Guid.NewGuid().ToString();
            String wo7 = Guid.NewGuid().ToString();
            String wo8 = Guid.NewGuid().ToString();
            String wo9 = Guid.NewGuid().ToString();
            List<DefaultWorkout> workouts = new List<DefaultWorkout>
            {
                new DefaultWorkout {Id = wo1, EstiamtedDurationInMinutes = 70,
                    ExercisesIds = ex1+";"+ex6+";"+ex9+";"+ex11+";"+ex14+";"+ex18+";"+ex21 }, // FBW - GYM

                new DefaultWorkout {Id = wo2, EstiamtedDurationInMinutes = 70,
                    ExercisesIds = ex3+";"+ex5+";"+ex8+";"+ex12+";"+ex13+";"+ex16+";"+ex20 }, // FBW - GYM

                new DefaultWorkout {Id = wo3, EstiamtedDurationInMinutes = 60,
                    ExercisesIds = ex1+";"+ex12+";"+ex13}, // A (AB) - GYM

                new DefaultWorkout {Id = wo4, EstiamtedDurationInMinutes = 65,
                    ExercisesIds = ex4+";"+ex18+";"+ex7+";"+ex22 }, // B (AB) - GYM

                new DefaultWorkout {Id = wo5, EstiamtedDurationInMinutes = 50,
                    ExercisesIds = ex3+";"+ex12}, // A (ABC) - GYM

                new DefaultWorkout {Id = wo6, EstiamtedDurationInMinutes = 50,
                    ExercisesIds = ex5+";"+ex7}, // B (ABC) - GYM

                new DefaultWorkout {Id = wo7, EstiamtedDurationInMinutes = 55,
                    ExercisesIds = ex15+";"+ex16+";"+ex20}, // C (ABC) - GYM

                new DefaultWorkout {Id = wo8, EstiamtedDurationInMinutes = 30,
                    ExercisesIds = ex23}, // Running - Outdise

                new DefaultWorkout {Id = wo9, EstiamtedDurationInMinutes = 40,
                    ExercisesIds = ex2+";"+ex10+";"+ex19+";"+ex22}, // FBW - Outdise
            };
            foreach (DefaultWorkout workout in workouts)
            {
                context.Set<DefaultWorkout>().Add(workout);
            }


            String pr1 = Guid.NewGuid().ToString();
            String pr2 = Guid.NewGuid().ToString();
            String pr3 = Guid.NewGuid().ToString();
            String pr4 = Guid.NewGuid().ToString();
            String pr5 = Guid.NewGuid().ToString();
            String pr6 = Guid.NewGuid().ToString();
            List<DefaultTrainingProgram> programs = new List<DefaultTrainingProgram>
            {
                new DefaultTrainingProgram{Id = pr1, Name="Indoor Full Body Workout Program", TrainingProgramType = TrainingProgramType.Gym,
                    TrainingProgramGoal = TrainingProgramGoal.Fitness, Difficulty = Difficulty.Begginer,
                    DurationInWeeks = 6, WorkoutIds = wo1},

                new DefaultTrainingProgram{Id = pr2, Name="Indoor Full Body Workout Program", TrainingProgramType = TrainingProgramType.Gym,
                    TrainingProgramGoal = TrainingProgramGoal.Fitness, Difficulty = Difficulty.Begginer,
                    DurationInWeeks = 6, WorkoutIds = wo2},

                new DefaultTrainingProgram{Id = pr3, Name="Indoor AB Workout Program", TrainingProgramType = TrainingProgramType.Gym,
                    TrainingProgramGoal = TrainingProgramGoal.Size, Difficulty = Difficulty.Intermediate,
                    DurationInWeeks = 8, WorkoutIds = wo3+";"+wo4},

                new DefaultTrainingProgram{Id = pr4, Name="Indoor ABC Workout Program", TrainingProgramType = TrainingProgramType.Gym,
                    TrainingProgramGoal = TrainingProgramGoal.Strength, Difficulty = Difficulty.Expert,
                    DurationInWeeks = 8, WorkoutIds = wo5+";"+wo6+";"+wo7},

                new DefaultTrainingProgram{Id = pr5, Name="Outdoor Running Workout Program", TrainingProgramType = TrainingProgramType.Gym,
                    TrainingProgramGoal = TrainingProgramGoal.Speed, Difficulty = Difficulty.Intermediate,
                    DurationInWeeks = 8, WorkoutIds = wo8},

                new DefaultTrainingProgram{Id = pr6, Name="Outdoor Weight Loss Workout Program", TrainingProgramType = TrainingProgramType.Gym,
                    TrainingProgramGoal = TrainingProgramGoal.WeightLoss, Difficulty = Difficulty.Intermediate,
                    DurationInWeeks = 8, WorkoutIds = wo8+";"+wo9},
            };
            foreach (DefaultTrainingProgram program in programs)
            {
                context.Set<DefaultTrainingProgram>().Add(program);
            }

            base.Seed(context);
        }
    }
}
