using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	public class DailyProgress : BulkAbstractRequest
	{
		public override string TableName => "progress";
		private DailyProgressModel[] Model { get; set; }
		private GetRequestData GetRequestDataObject;

		//data for get request:
		private class GetRequestData
		{
			public string UserId { get; set; }
			public DateTime StartTime { get; set; }
			public DateTime EndTime { get; set; }
		}

		public override void InitGetData(HttpRequestMessage req)
		{
			GetRequestDataObject = new GetRequestData
			{
				UserId = GetParameter(req, "userId"),
				StartTime = new DateTime(long.Parse(GetParameter(req, "startTime"))),
				EndTime = new DateTime(long.Parse(GetParameter(req, "endTime")))
			};
		}

		public override string GetSelectQuery()
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE [userId] = {PadString(GetRequestDataObject.UserId)} " +
			       $"AND [dayId] >= {PadDate(GetRequestDataObject.StartTime)} " +
			       $"AND [dayId] <= {PadDate(GetRequestDataObject.EndTime)}" +
			       "; ";
		}

		public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data");

			Model = JsonConvert.DeserializeObject<DailyProgressModel[]>(data, DateTimeConverter);

			BulkSize = Model.Length;
		}

		public override string ExistsRowIfCheck(int index)
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE " +
			       $"[userId] = {PadString(Model[index].UserId)} AND [dayId] = {PadDate(Model[index].DayId)} ";
		}

		public override string UpdateRowQuery(int index)
		{
			var updateStrings = GetColumns(index).Select(column => $"[{column.Item1}] = {column.Item2} ");
			var updatePart = string.Join(", ", updateStrings);

			return $"UPDATE [dbo].[{TableName}] SET {updatePart} WHERE " +
			       $"[userId] = {PadString(Model[index].UserId)} AND [dayId] = {PadDate(Model[index].DayId)}";
		}

		public override string InsertRowQuery(int index)
		{
			var allColumns = new[]
			{
				("userId", PadString(Model[index].UserId)),
				("dayId", PadDate(Model[index].DayId)),
			}.Concat(GetColumns(index)).ToArray();

			var keys = allColumns.Select(column => $"[{column.Item1}]");
			var keysString = string.Join(", ", keys);

			var values = allColumns.Select(column => column.Item2);
			var valuesString = string.Join(", ", values);

			return $"INSERT INTO [dbo].[{TableName}] ({keysString}) VALUES ({valuesString});";
		}

		private (string, string)[] GetColumns(int index)
		{
			return new[]
			{
				("stepsTaken", Model[index].StepsTaken.ToString()),
				("stepsTakenGoal", Model[index].StepsTakenGoal.ToString()),
				("floorsClimbed", Model[index].FloorsClimbed.ToString()),
				("floorsClimbedGoal", Model[index].FloorsClimbedGoal.ToString()),
				("activeHours", Model[index].ActiveHours.ToString()),
				("activeHoursGoal", Model[index].ActiveHoursGoal.ToString()),
				("totalDistance", Model[index].TotalDistance.ToString()),
				("totalDistanceGoal", Model[index].TotalDistanceGoal.ToString()),
				("totalDistanceOnFoot", Model[index].TotalDistanceOnFoot.ToString()),
				("totalDistanceOnFootGoal", Model[index].TotalDistanceOnFootGoal.ToString()),
				("caloriesBurned", Model[index].CaloriesBurned.ToString()),
				("caloriesBurnedGoal", Model[index].CaloriesBurnedGoal.ToString()),
				("sleepMinutes", Model[index].SleepMinutes.ToString()),
				("sleepMinutesGoal", Model[index].SleepMinutesGoal.ToString())
			};
		}

		public class DailyProgressModel
		{
			public string UserId { get; set; }
			public DateTime DayId { get; set; }

			public int StepsTaken { get; set; }
			public int StepsTakenGoal { get; set; }

			public int FloorsClimbed { get; set; }
			public int FloorsClimbedGoal { get; set; }

			public int ActiveHours { get; set; }
			public int ActiveHoursGoal { get; set; }

			public int TotalDistance { get; set; }
			public int TotalDistanceGoal { get; set; }

			public int TotalDistanceOnFoot { get; set; }
			public int TotalDistanceOnFootGoal { get; set; }

			public int CaloriesBurned { get; set; }
			public int CaloriesBurnedGoal { get; set; }

			public int SleepMinutes { get; set; }
			public int SleepMinutesGoal { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			var result = new List<DailyProgressModel>();
			while (reader.Read())
			{
				result.Add(new DailyProgressModel
				{
					UserId = reader.GetString(0),
					DayId = reader.GetDateTime(1),

					StepsTaken = reader.GetInt32(2),
					StepsTakenGoal = reader.GetInt32(3),

					FloorsClimbed = reader.GetInt32(4),
					FloorsClimbedGoal = reader.GetInt32(5),

					ActiveHours = reader.GetInt32(6),
					ActiveHoursGoal = reader.GetInt32(7),

					TotalDistance = reader.GetInt32(8),
					TotalDistanceGoal = reader.GetInt32(9),

					TotalDistanceOnFoot = reader.GetInt32(10),
					TotalDistanceOnFootGoal = reader.GetInt32(11),

					CaloriesBurned = reader.GetInt32(12),
					CaloriesBurnedGoal = reader.GetInt32(13),

					SleepMinutes = reader.GetInt32(14),
					SleepMinutesGoal = reader.GetInt32(15),
				});
			}

			return result.ToArray();
		}

		public DailyProgressModel[] GetAll(DateTime startTime, DateTime endTime)
		{
			var query = $"SELECT * FROM [dbo].[{TableName}] WHERE  " +
			            $"[dayId] >= {PadDate(startTime)} " +
			            $"AND [dayId] <= {PadDate(endTime)}" +
			            "; ";

			var result = Get(query);

			return (DailyProgressModel[]) result;
		}

		public bool Save(DailyProgressModel[] progress)
		{
			Model = progress;

			BulkSize = Model.Length;

			var query = GetUpsertQuery();

			var successful = SaveToDatabase(query, null);

			return successful;
		}
	}
}