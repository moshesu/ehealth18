using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;

namespace boostFunctions.Database
{
	class PlayersByCoach : BulkAbstractRequest
	{
		public override string TableName => _playerRepositoryInstance.TableName;

		private string _coachId;

		private readonly Player _playerRepositoryInstance = new Player();

		public override void InitGetData(HttpRequestMessage req)
		{
			_coachId = GetParameter(req, "coachId");
		}

		public override string GetSelectQuery()
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE [coachId] = {PadString(_coachId)}; ";
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			var result = new List<Player.PlayerModel>();
			while (reader.Read())
			{
				result.Add(_playerRepositoryInstance.ReaderToModel(reader));
			}

			return result.ToArray();
		}

		public override void InitSaveData(HttpRequestMessage req)
		{
			throw new System.NotImplementedException();
		}

		public override string ExistsRowIfCheck(int index)
		{
			throw new System.NotImplementedException();
		}

		public override string UpdateRowQuery(int index)
		{
			throw new System.NotImplementedException();
		}

		public override string InsertRowQuery(int index)
		{
			throw new System.NotImplementedException();
		}
	}
}