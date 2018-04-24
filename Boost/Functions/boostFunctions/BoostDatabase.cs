using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using boostFunctions.Database;
namespace boostFunctions
{
	public static class BoostDatabase
	{
		[FunctionName("BoostDatabase")]
		public static HttpResponseMessage Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, TraceWriter log)
		{
			string action = AbstractRequest.GetParameter(req, "action");

			switch (action)
			{
				/*
				 * Single requests
				 */
				case "SaveUserType":
					return new UserType().Save(req, log);

				case "GetUserType":
					return new UserType().Get(req, log);

				case "RemovePlayerType":
					return new UserType().Remove(req, log);

				case "SavePlayer":
					return new Player().Save(req, log);

				case "GetPlayer":
					return new Player().Get(req, log);

				case "RemovePlayer":
					return new Player().Remove(req, log);

				case "SaveCoach":
					return new Coach().Save(req, log);

				case "GetCoach":
					return new Coach().Get(req, log);

				case "RemoveCoach":
					return new Coach().Remove(req, log);

				case "SaveGoals":
					return new Goals().Save(req, log);

				case "GetGoals":
					return new Goals().Get(req, log);

				case "RemoveGoals":
					return new Goals().Remove(req, log);

				/*
				 * Bulk requests
				 */
				case "GetPlayersByCoach":
					return new PlayersByCoach().Get(req, log);

				case "SaveSleepRecords":
					return new Sleep().Save(req, log);

				case "GetSleepRecords":
					return new Sleep().Get(req, log);

				case "SaveProgress":
					return new DailyProgress().Save(req, log);

				case "GetProgress":
					return new DailyProgress().Get(req, log);

				case "SaveActivities":
					return new Activity().Save(req, log);

				case "GetActivities":
					return new Activity().Get(req, log);

				case "SaveTransactions":
					return new Transaction().Save(req, log);

				case "GetTransactions":
					return new Transaction().Get(req, log);

				default:
					return req.CreateResponse(HttpStatusCode.BadRequest, "Unknown action");
			}
		}
	}
}