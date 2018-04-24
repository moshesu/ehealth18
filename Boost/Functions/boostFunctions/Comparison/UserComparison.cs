using System.Net.Http;
using boostFunctions.Database;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace boostFunctions.Comparison
{
	public static class UserComparison
	{
		[FunctionName("UserComparison")]
		public static HttpResponseMessage Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, TraceWriter log)
		{
			var userParameters = AbstractRequest.GetParameter(req, "userId");
			var result = new ComparisonProvider(new BoostRepository()).GetUserComparison(userParameters);

			return AbstractRequest.SerializeRespone(req, result);
		}
	}
}