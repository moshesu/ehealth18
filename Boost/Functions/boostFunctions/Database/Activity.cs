using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	class Activity : BulkAbstractRequest
	{
		public override string TableName => "activities";
		private ActivityModel[] Model { get; set; }
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

			Model = JsonConvert.DeserializeObject<ActivityModel[]>(data, DateTimeConverter);

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
				("activityType", Model[index].ActivityType.ToString()),
				("dayId", PadDate(Model[index].DayId)),
				("startTime", PadDate(Model[index].StartTime)),
				("endTime", PadDate(Model[index].EndTime)),
				("duration", PadString(Model[index].Duration)),
				("totalDistance", Model[index].TotalDistance.ToString()),
				("totalDistanceOnFoot", Model[index].TotalDistanceOnFoot.ToString()),
				("totalCalories", Model[index].TotalCalories.ToString()),
				("averageHeartRate", Model[index].AverageHeartRate.ToString()),
				("lowestHeartRate", Model[index].LowestHeartRate.ToString()),
				("peakHeartRate", Model[index].PeakHeartRate.ToString())
			};
		}

		public class ActivityModel
		{
			public string UserId { get; set; }

			public int ActivityType { get; set; }
			public string Id { get; set; }

			public DateTime DayId { get; set; }
			public DateTime StartTime { get; set; }
			public DateTime EndTime { get; set; }
			public string Duration { get; set; }

			public int TotalDistance { get; set; }
			public int TotalDistanceOnFoot { get; set; }

			public int TotalCalories { get; set; }
			public int AverageHeartRate { get; set; }
			public int LowestHeartRate { get; set; }
			public int PeakHeartRate { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			var result = new List<ActivityModel>();
			while (reader.Read())
			{
				result.Add(new ActivityModel
				{
					Id = reader.GetString(0),
					UserId = reader.GetString(1),
					ActivityType = reader.GetInt32(2),
					DayId = reader.GetDateTime(3),

					StartTime = reader.GetDateTime(4),
					EndTime = reader.GetDateTime(5),
					Duration = reader.GetString(6),
					TotalDistance = reader.GetInt32(7),
					TotalDistanceOnFoot = reader.GetInt32(8),
					TotalCalories = reader.GetInt32(9),
					AverageHeartRate = reader.GetInt32(10),
					LowestHeartRate = reader.GetInt32(11),
					PeakHeartRate = reader.GetInt32(12),
				});
			}

			return result.ToArray();
		}
	}
}