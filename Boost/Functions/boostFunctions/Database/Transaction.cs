using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	class Transaction : BulkAbstractRequest
	{
		public override string TableName => "transactions";
		private TransactionModel[] Model { get; set; }
		private GetRequestData _getRequestDataObject;

		//data for get request:
		private class GetRequestData
		{
			public string UserId { get; set; }
			public DateTime StartTime { get; set; }
			public DateTime EndTime { get; set; }
		}

		public override void InitGetData(HttpRequestMessage req)
		{
			_getRequestDataObject = new GetRequestData
			{
				UserId = GetParameter(req, "userId"),
				StartTime = new DateTime(long.Parse(GetParameter(req, "startTime"))),
				EndTime = new DateTime(long.Parse(GetParameter(req, "endTime")))
			};
		}

		public override string GetSelectQuery()
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE [userId] = {PadString(_getRequestDataObject.UserId)} " +
			       $"AND [transactionTime] >= {PadDate(_getRequestDataObject.StartTime)} " +
			       $"AND [transactionTime] <= {PadDate(_getRequestDataObject.EndTime)}" +
			       "; ";
		}

		public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data");

			Model = JsonConvert.DeserializeObject<TransactionModel[]>(data, DateTimeConverter);

			BulkSize = Model.Length;
		}

		public override string ExistsRowIfCheck(int index)
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE " +
			       $"[userId] = {PadString(Model[index].UserId)} AND [transactionTime] = {PadDate(Model[index].TransactionTime)} ";
		}

		public override string UpdateRowQuery(int index)
		{
			var updateStrings = GetColumns(index).Select(column => $"[{column.Item1}] = {column.Item2} ");
			var updatePart = string.Join(", ", updateStrings);

			return $"UPDATE [dbo].[{TableName}] SET {updatePart} WHERE " +
			       $"[userId] = {PadString(Model[index].UserId)} AND [transactionTime] = {PadDate(Model[index].TransactionTime)}";
		}

		public override string InsertRowQuery(int index)
		{
			var allColumns = new[]
			{
				("userId", PadString(Model[index].UserId)),
				("transactionTime", PadDate(Model[index].TransactionTime)),
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
				("balanceBefore", Model[index].BalanceBefore.ToString()),
				("transactionAmount", Model[index].TransactionAmount.ToString()),
				("details", PadString(Model[index].Details)),
			};
		}

		public class TransactionModel
		{
			public string UserId { get; set; }
			public DateTime TransactionTime { get; set; }

			public int BalanceBefore { get; set; }
			public int TransactionAmount { get; set; }
			public string Details { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			var result = new List<TransactionModel>();
			while (reader.Read())
			{
				result.Add(new TransactionModel
				{
					UserId = reader.GetString(0),
					TransactionTime = reader.GetDateTime(1),

					BalanceBefore = reader.GetInt32(2),
					TransactionAmount = reader.GetInt32(3),
					Details = reader.GetString(4)
				});
			}

			return result.ToArray();
		}
	}
}