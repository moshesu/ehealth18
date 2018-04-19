﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Personal_TrainerService.DataObjects;

namespace Personal_TrainerService.Models
{
    public class Personal_TrainerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public Personal_TrainerContext() : base(connectionStringName)
        {
        } 
        public DbSet<DefaultExercise> DefaultExercises { get; set; }
        public DbSet<DefaultTrainingProgram> DefaultTrainingPrograms { get; set; }
        public DbSet<DefaultWorkout> DefaultWorkouts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTrainingProgram> UserTrainingPrograms { get; set; }
        public DbSet<UserWorkout> UserWorkouts { get; set; }
        public DbSet<UserWorkoutStatistics> UserWorkoutStatistics { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }
    }

}