Select * FROM [dbo].[players];
Select * FROM [dbo].[user-types];
Select * FROM [dbo].[players];
Select * FROM [dbo].[sleep];
Select * FROM [dbo].[progress];
Select * FROM [dbo].[activities];

CREATE TABLE [user-types] (
	userId VARCHAR(50) PRIMARY KEY,
	userType INT,
	coachId VARCHAR(50) 
)

CREATE TABLE players (
    userId VARCHAR(50) PRIMARY KEY,
	firstName VARCHAR(50),
	lastName VARCHAR(50),
	gender VARCHAR(50),
	height INT,
	weight INT,
	postalCode VARCHAR(50),
	preferredLocale VARCHAR(50),
	birthdate DATETIME,
	createdTime DATETIME,
	lastUpdateTime DATETIME,
	coachId VARCHAR(50)
);

CREATE TABLE coaches (
    userId VARCHAR(50) PRIMARY KEY,
	firstName VARCHAR(50),
	lastName VARCHAR(50),
	gender VARCHAR(50),
	height INT,
	weight INT,
	postalCode VARCHAR(50),
	preferredLocale VARCHAR(50),
	birthdate DATETIME,
	createdTime DATETIME,
	lastUpdateTime DATETIME,
	paymentLastDigits VARCHAR(50)
);

CREATE TABLE sleep (
	id VARCHAR(50) PRIMARY KEY,
	userId VARCHAR(50),
	dayId DATETIME,
	startTime DATETIME,
	endTime DATETIME,
	fallAsleepTime DATETIME,
	wakeupTime DATETIME,
	duration VARCHAR(50),
	sleepDuration VARCHAR(50),
	fallAsleepDuration VARCHAR(50),
	awakeDuration VARCHAR(50),
	totalRestfulSleepDuration VARCHAR(50),
	totalRestlessSleepDuration VARCHAR(50),
	NumberOfWakeups INT,
	restingHeartRate INT,
	totalCalories INT,
	averageHeartRate INT,
	lowestHeartRate INT,
	peakHeartRate INT	
);

Drop table progress;

CREATE TABLE progress (
	userId VARCHAR(50),
	dayId DATETIME,
	
	stepsTaken INT,
	stepsTakenGoal INT,
	floorsClimbed INT,
	floorsClimbedGoal INT,
	activeHours INT,
	activeHoursGoal INT,
	totalDistance INT,
	totalDistanceGoal INT,
	totalDistanceOnFoot INT,
	totalDistanceOnFootGoal INT,
	caloriesBurned INT,
	caloriesBurnedGoal INT,
	sleepMinutes INT,
	sleepMinutesGoal INT,
	primary key(userId, dayId)	
);

CREATE TABLE activities (
	id VARCHAR(50) PRIMARY KEY,
	userId VARCHAR(50),
	activityType int,

	dayId DATETIME,	
	startTime DATETIME,
	endTime DATETIME,
	duration VARCHAR(50),
	totalDistance INT,
	totalDistanceOnFoot INT,
	totalCalories INT,
	averageHeartRate INT,
	lowestHeartRate INT,
	peakHeartRate INT	
);

CREATE TABLE transactions (
	userId VARCHAR(50),
	transactionTime DATETIME,	
	balanceBefore INT,
	transactionAmount INT,
	details VARCHAR(256),
	primary key(userId, transactionTime)	
);

CREATE TABLE goals (
    userId VARCHAR(50) PRIMARY KEY,
	stepsTaken INT,
	stepsTakenReward INT,
	sleepMinutes INT,
	sleepMinutesReward INT,
	CaloriesBurned INT,
	CaloriesBurnedReward INT,
	weeklyActiveMinutes INT,
	weeklyActiveMinutesReward INT,
	weeklyCaloriesBurned INT,
	weeklyCaloriesBurnedReward INT
);
