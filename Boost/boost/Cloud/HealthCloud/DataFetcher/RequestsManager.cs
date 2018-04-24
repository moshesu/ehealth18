using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using boost.Cloud.HealthCloud.Authentication;
using boost.Cloud.HealthCloud.Exceptions;

namespace boost.Cloud.HealthCloud.DataFetcher
{
	class RequestsManager
	{
		private const int RequestRetries = 3;
		private const string ApiVersion = "v1";
		private const string HealthApiUri = "https://api.microsofthealth.net";
		private readonly HealthTokenGenerator _tokenGenerator;

		public RequestsManager()
		{
			_tokenGenerator = new HealthTokenGenerator();
		}

		public async Task<string> MakeRequest(string path, string query = "")
		{
			for (var count = 0; count < RequestRetries; count++)
			{
				using (var http = new HttpClient())
				{
					var token = await _tokenGenerator.GetHealthAuthenticationToken();
					http.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", token);

					var ub = new UriBuilder(HealthApiUri)
					{
						Path = ApiVersion + "/" + path,
						Query = query
					};

					using (var resp = await http.GetAsync(ub.Uri).AsTask().ConfigureAwait(false))
					{
						if (resp.StatusCode == HttpStatusCode.Unauthorized)
						{
							if (count == 1)
								_tokenGenerator.ClearVault();
							await _tokenGenerator.RefreshTokenInVault();
							continue;
						}

						if (resp.IsSuccessStatusCode)
						{
							var resStr = await resp.Content.ReadAsStringAsync().AsTask().ConfigureAwait(false);
							return resStr;
						}

						throw new HealthRequestException($"Unsuccessful request - {resp.ReasonPhrase}\nContent: {resp.Content}");
					}
				}
			}

			throw new UnauthorizedRequestExeption("Failed sending request 3 times");
		}

		public void ClearVault()
		{
			_tokenGenerator.ClearVault();
		}
	}
}