using System;
using Windows.Web.Http;
using boost.Cloud.HealthCloud.Exceptions;

namespace boost.Cloud.AzureDatabase
{
	public class AzureApiBadResponseCodeExcpetion : HealthRequestException
	{
		public AzureApiBadResponseCodeExcpetion(HttpStatusCode statusCode, string content) :
			base($"Bad Azure API response - {statusCode} - {content}")
		{
		}

		public AzureApiBadResponseCodeExcpetion(Exception e) :
			base("Exception in Azure API call", e)
		{
		}
	}
}