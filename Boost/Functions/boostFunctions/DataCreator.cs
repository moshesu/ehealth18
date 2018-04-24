using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using boostFunctions.Util;

namespace boostFunctions
{
	public static class DataCreator
	{
		[FunctionName("DataCreator")]
		public static HttpResponseMessage Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, TraceWriter log)
		{
			ComparisonUserCreator.CreateUsers();
			return req.CreateResponse(HttpStatusCode.OK);
		}
	}
}