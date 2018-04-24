using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	class Sleep : BulkAbstractRequest
	{
		public override string TableName => "sleep";
		private SleepModel[] Model { get; set; }
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

			Model = JsonConvert.DeserializeObject<SleepModel[]>(data, DateTimeConverter);

			BulkSize = Model.Length;
		}

		public override string ExistsRowIfCheck(int index)
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE [id] = {PadString(Model[index].Id)} ";
		}

		public override string UpdateRowQuery(int index)
		{
			var updateStrings = GetColumns(index).Select(column => $"[{column.Item1}] = {column.Item2} ");
			var updatePart = string.Join(", ", updateStrings);

			return $"UPDATE [dbo].[{TableName}] SET {updatePart} WHERE [id] = {PadString(Model[index].Id)} ";
		}

		public override string InsertRowQuery(int index)
		{
			var allColumns = new[] {("id", PadString(Model[index].Id))}.Concat(GetColumns(index)).ToArray();

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
				("userId", PadString(Model[index].UserId)),
				("dayId", PadDate(Model[index].DayId)),
				("startTime", PadDate(Model[index].StartTime)),
				("endTime", PadDate(Model[index].EndTime)),
				("fallAsleepTime", PadDate(Model[index].FallAsleepTime)),
				("wakeupTime", PadDate(Model[index].WakeupTime)),
				("duration", PadString(Model[index].Duration)),
				("sleepDuration", PadString(Model[index].SleepDuration)),
				("fallAsleepDuration", PadString(Model[index].FallAsleepDuration)),
				("awakeDuration", PadString(Model[index].AwakeDuration)),
				("totalRestfulSleepDuration", PadString(Model[index].TotalRestfulSleepDuration)),
				("totalRestlessSleepDuration", PadString(Model[index].TotalRestlessSleepDuration)),
				("NumberOfWakeups", Model[index].NumberOfWakeups.ToString()),
				("restingHeartRate", Model[index].RestingHeartRate.ToString()),
				("totalCalories", Model[index].TotalCalories.ToString()),
				("averageHeartRate", Model[index].AverageHeartRate.ToString()),
				("lowestHeartRate", Model[index].LowestHeartRate.ToString()),
				("peakHeartRate", Model[index].PeakHeartRate.ToString())
			};
		}

		public class SleepModel
		{
			public string Id { get; set; }
			public string UserId { get; set; }
			public DateTime DayId { get; set; }

			public DateTime StartTime { get; set; }
			public DateTime EndTime { get; set; }
			public DateTime FallAsleepTime { get; set; }
			public DateTime WakeupTime { get; set; }

			public string Duration { get; set; }
			public string SleepDuration { get; set; }
			public string FallAsleepDuration { get; set; }
			public string AwakeDuration { get; set; }
			public string TotalRestfulSleepDuration { get; set; }
			public string TotalRestlessSleepDuration { get; set; }

			public int NumberOfWakeups { get; set; }

			public int RestingHeartRate { get; set; }
			public int TotalCalories { get; set; }
			public int AverageHeartRate { get; set; }
			public int LowestHeartRate { get; set; }
			public int PeakHeartRate { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			var result = new List<SleepModel>();
			while (reader.Read())
			{
				result.Add(new SleepModel
				{
					Id = reader.GetString(0),
					UserId = reader.GetString(1),
					DayId = reader.GetDateTime(2),

					StartTime = reader.GetDateTime(3),
					EndTime = reader.GetDateTime(4),
					FallAsleepTime = reader.GetDateTime(5),
					WakeupTime = reader.GetDateTime(6),

					Duration = reader.GetString(7),
					SleepDuration = reader.GetString(8),
					FallAsleepDuration = reader.GetString(9),
					AwakeDuration = reader.GetString(10),
					TotalRestfulSleepDuration = reader.GetString(11),
					TotalRestlessSleepDuration = reader.GetString(12),

					NumberOfWakeups = reader.GetInt32(13),

					RestingHeartRate = reader.GetInt32(14),
					TotalCalories = reader.GetInt32(15),
					AverageHeartRate = reader.GetInt32(16),
					LowestHeartRate = reader.GetInt32(17),
					PeakHeartRate = reader.GetInt32(18),
				});
			}

			return result.ToArray();
		}
	}
}