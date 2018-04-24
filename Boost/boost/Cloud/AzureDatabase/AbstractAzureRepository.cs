using System;
using System.Linq;
using Windows.Web.Http;
using Newtonsoft.Json.Converters;

namespace boost.Cloud.AzureDatabase
{
	public abstract class AbstractAzureRepository
	{
		private const string Host = "https://boostdbfunction.azurewebsites.net/api"; //Production
		//private const string Host = "http://localhost:7071/api"; //Debug

		private readonly string DatabaseUrl = $"{Host}/BoostDatabase";
		private readonly string ComparisonUrl = $"{Host}/UserComparison";
		private const string DatabaseCode = "ugNs8M/a2shd9CLYVZgO7P2IIK91hNOaoy6nMfyM1TZkblHkDX6w4A==";
		private const string ComparisonCode = "LrzyjhfcraIN1pFASf20XbpyNMR8zLkJov3xaRe9q9lMgF7pzKLlBg==";

		protected const string UserIdKey = "userId";
		protected const string DateFormat = "yyyy-MM-ddTHH\\:mm\\:ss";
		public IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter {DateTimeFormat = DateFormat};

		public string CallAzureDatabase(string action, params Parameter[] externalParameters)
		{
			return CallAzureFunction(DatabaseUrl, action, DatabaseCode, externalParameters);
		}

		public string CallAzureUserComparison(params Parameter[] externalParameters)
		{
			return CallAzureFunction(ComparisonUrl, "comparison", ComparisonCode, externalParameters);
		}

		private string CallAzureFunction(string url, string action, string code, params Parameter[] externalParameters)
		{
			try
			{
				var ownParameters = new[]
				{
					new Parameter("code", code),
					new Parameter("action", action)
				};

				var parameters = ownParameters.Concat(externalParameters);
				var headers = parameters.Select(p => $"{p.Key}={p.Value}").ToArray();

				var ub = new UriBuilder(url)
				{
					Query = string.Join("&", headers)
				};

				using (var http = new HttpClient())
				using (var resp = http.GetAsync(ub.Uri).AsTask().ConfigureAwait(false).GetAwaiter().GetResult())
				{
					if (resp.IsSuccessStatusCode)
						return resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

					if (resp.StatusCode == HttpStatusCode.NotFound)
						return null;

					throw new AzureApiBadResponseCodeExcpetion(resp.StatusCode, resp.Content.ToString());
				}
			}
			catch (Exception e)
			{
				throw new AzureApiBadResponseCodeExcpetion(e);
			}
		}
	}

	public class Parameter
	{
		public Parameter(string key, string value)
		{
			Key = key;
			Value = value;
		}

		public string Key { get; set; }
		public string Value { get; set; }
	}
}