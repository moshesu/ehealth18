using System.Data.SqlClient;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	class Coach : SingleAbstractRequest
	{
		public override string TableName => "coaches";
		private CoachModel Model { get; set; }

		public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data");
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);

			Model = JsonConvert.DeserializeObject<CoachModel>(data, DateTimeConverter);
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
			("paymentLastDigits", PadString(Model.PaymentLastDigits))
		};

		public class CoachModel : UserModel
		{
			public string PaymentLastDigits { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			reader.Read();
			return new CoachModel
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
				PaymentLastDigits = reader.GetString(11),
			};
		}
	}
}