using System.Data.SqlClient;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	class Goals : SingleAbstractRequest
	{
		public override string TableName => "goals";

		private GoalsModel Model { get; set; }

		public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data");
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);

			Model = JsonConvert.DeserializeObject<GoalsModel>(data, DateTimeConverter);
		}

		public override void InitGetData(HttpRequestMessage req)
		{
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);
		}

		public override (string, string)[] GetColumns() => new[]
		{
			("stepsTaken", Model.StepsTaken.ToString()),
			("stepsTakenReward", Model.StepsTakenReward.ToString()),
			("sleepMinutes", Model.SleepMinutes.ToString()),
			("sleepMinutesReward", Model.SleepMinutesReward.ToString()),
			("CaloriesBurned", Model.CaloriesBurned.ToString()),
			("CaloriesBurnedReward", Model.CaloriesBurnedReward.ToString()),
			("weeklyActiveMinutes", Model.WeeklyActiveMinutes.ToString()),
			("weeklyActiveMinutesReward", Model.WeeklyActiveMinutesReward.ToString()),
			("weeklyCaloriesBurned", Model.WeeklyCaloriesBurned.ToString()),
			("weeklyCaloriesBurnedReward", Model.WeeklyCaloriesBurnedReward.ToString())
		};

		public class GoalsModel
		{
			public int StepsTaken { get; set; }
			public int StepsTakenReward { get; set; }
			public int SleepMinutes { get; set; }
			public int SleepMinutesReward { get; set; }
			public int CaloriesBurned { get; set; }
			public int CaloriesBurnedReward { get; set; }
			public int WeeklyActiveMinutes { get; set; }
			public int WeeklyActiveMinutesReward { get; set; }
			public int WeeklyCaloriesBurned { get; set; }
			public int WeeklyCaloriesBurnedReward { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			reader.Read();
			return new GoalsModel
			{
				//UserId = reader.GetString(0),
				StepsTaken = reader.GetInt32(1),
				StepsTakenReward = reader.GetInt32(2),
				SleepMinutes = reader.GetInt32(3),
				SleepMinutesReward = reader.GetInt32(4),
				CaloriesBurned = reader.GetInt32(5),
				CaloriesBurnedReward = reader.GetInt32(6),
				WeeklyActiveMinutes = reader.GetInt32(7),
				WeeklyActiveMinutesReward = reader.GetInt32(8),
				WeeklyCaloriesBurned = reader.GetInt32(9),
				WeeklyCaloriesBurnedReward = reader.GetInt32(10),
			};
		}
	}
}