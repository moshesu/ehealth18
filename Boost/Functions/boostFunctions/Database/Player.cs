using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	public class Player : SingleAbstractRequest
	{
		public override string TableName => "players";

		private PlayerModel Model { get; set; }

		public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data");
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);

			Model = JsonConvert.DeserializeObject<PlayerModel>(data, DateTimeConverter);
		}

		public override void InitGetData(HttpRequestMessage req)
		{
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);
		}

		public override (string, string)[] GetColumns() => new[]
		{
			("firstName", PadString(Model.FirstName)),
			("lastName", PadString(Model.LastName)),
			("gender", PadString(Model.Gender)),
			("height", Model.Height.ToString()),
			("weight", Model.Weight.ToString()),
			("postalCode", PadString(Model.PostalCode)),
			("preferredLocale", PadString(Model.PreferredLocale)),
			("birthdate", PadDate(Model.Birthdate)),
			("createdTime", PadDate(Model.CreatedTime)),
			("lastUpdateTime", PadDate(Model.LastUpdateTime)),
			("coachId", PadString(Model.CoachId))
		};

		public class PlayerModel : UserModel
		{
			public string CoachId { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			//reader.Read();
			//return ReaderToModel(reader);

			var result = new List<PlayerModel>();
			while (reader.Read())
			{
				result.Add(ReaderToModel(reader));
			}

			if (result.Count == 0)
				return null;

			if (result.Count == 1)
				return result[0];

			return result;
		}

		public PlayerModel ReaderToModel(SqlDataReader reader)
		{
			return new PlayerModel
			{
				UserId = reader.GetString(0),
				FirstName = reader.GetString(1),
				LastName = reader.GetString(2),
				Gender = reader.GetString(3),

				Height = reader.GetInt32(4),
				Weight = reader.GetInt32(5),

				PostalCode = reader.GetString(6),
				PreferredLocale = reader.GetString(7),

				Birthdate = reader.GetDateTime(8),
				CreatedTime = reader.GetDateTime(9),
				LastUpdateTime = reader.GetDateTime(10),
				CoachId = reader.GetString(11),
			};
		}

		public List<PlayerModel> GetAll()
		{
			var query = $"SELECT * FROM [dbo].[{TableName}]; ";

			var result = Get(query);

			if (result is PlayerModel)
				return new List<PlayerModel> {(PlayerModel) result};

			return (List<PlayerModel>) result;
		}

		public bool Save(PlayerModel player)
		{
			GetRequestIdValue = player.UserId;

			Model = player;

			var query = GetUpsertQuery();

			var successful = SaveToDatabase(query, null);

			return successful;
		}
	}
}