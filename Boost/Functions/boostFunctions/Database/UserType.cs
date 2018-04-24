using System.Data.SqlClient;
using System.Net.Http;
using Newtonsoft.Json;

namespace boostFunctions.Database
{
	class UserType : SingleAbstractRequest
	{
		public override string TableName => "user-types";
		private UserTypeModel Model { get; set; }

		public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data");
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);

			Model = JsonConvert.DeserializeObject<UserTypeModel>(data);
		}

		public override void InitGetData(HttpRequestMessage req)
		{
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);
		}

		public override (string, string)[] GetColumns() => new[]
		{
			("userType", Model.UserType.ToString()),
			("coachId", PadString(Model.CoachId))
		};

		class UserTypeModel
		{
			public string UserId;
			public int UserType;
			public string CoachId;
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			reader.Read();
			return new UserTypeModel
			{
				UserId = reader.GetString(0),
				UserType = reader.GetInt32(1),
				CoachId = reader.GetString(2)
			};
		}
	}
}