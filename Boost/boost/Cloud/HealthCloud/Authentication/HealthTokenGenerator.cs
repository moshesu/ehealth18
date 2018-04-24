using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using boost.Cloud.HealthCloud.Exceptions;
using boost.Util;

namespace boost.Cloud.HealthCloud.Authentication
{
	interface IHealthTokenGenerator
	{
		Task<string> GetHealthAuthenticationToken();
		Task RefreshTokenInVault();
	}

	public class HealthTokenGenerator : IHealthTokenGenerator
	{
		private const string Scopes =
				"mshealth.ReadDevices mshealth.ReadActivityHistory mshealth.ReadActivityLocation mshealth.ReadDevices mshealth.ReadProfile offline_access"
			;

		private const string RedirectUri = "https://login.live.com/oauth20_desktop.srf";
		private const string TokenUri = "https://login.live.com/oauth20_token.srf";

		private static string ClientId = "ba292a15-8474-4d6b-a341-0ae5bfddebe0";

		private const string ResourceName = "BoostOauthToken";
		private const string RefreshResourceName = "BoostOauthTokenRefresh";
		private const string UserName = "User";

		public async Task<string> GetHealthAuthenticationToken()
		{
			var token = GetTokenFromVault(ResourceName);
			if (!string.IsNullOrEmpty(token.Item2))
				return token.Item2;

			var code = await GetAcessCode();

			var authToken = await GetTokensFromAuthCode(code);
			return authToken;
		}

		public async Task RefreshTokenInVault()
		{
			var currentToken = GetTokenFromVault(RefreshResourceName);
			string refreshToken = currentToken.Item2.Trim('"');
			await SendTokenRequest("refresh_token", refreshToken, "refresh_token");
		}

		private Tuple<string, string> GetTokenFromVault(string resName)
		{
			string userName = string.Empty;
			string password = string.Empty;

			var vault = new PasswordVault();
			try
			{
				var credential = vault.FindAllByResource(resName).FirstOrDefault();
				if (credential?.Properties != null)
				{
					userName = credential.UserName;
					password = vault.Retrieve(resName, userName).Password;
				}
			}
			catch (Exception)
			{
				Debug.WriteLine($"Could not find resorce {resName} in vault");
			}
			return new Tuple<string, string>(userName, password);
		}

		public void ClearVault()
		{
			try
			{
				var vault = new PasswordVault();
				vault.RetrieveAll().ForEach(x => vault.Remove(x));
			}
			catch (Exception e)
			{
				throw new TokenAcquiringException("Failed clearing vault", e);
			}
		}

		private void AddTokenToVault(string resName, string userName, string token)
		{
			try
			{
				var vault = new PasswordVault();
				var credential = new PasswordCredential(resName, userName, token);
				vault.Add(credential);
			}
			catch (Exception e)
			{
				throw new TokenAcquiringException($"Failed saving token {resName} in vault", e);
			}
		}

		private static async Task<string> GetAcessCode()
		{
			var oAuthRequestUri = CreateOAuthCodeRequestUri();
			Debug.WriteLine($"OAuth request URI = {oAuthRequestUri}");

			try
			{
				var authenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None,
					oAuthRequestUri,
					new Uri(RedirectUri));

				var responseStatus = authenticationResult.ResponseStatus;
				if (responseStatus == WebAuthenticationStatus.UserCancel)
					throw new UserCancelSignInExeption("User canceled the sign in request");

				var errorDetail = authenticationResult.ResponseErrorDetail;
				var responseData = authenticationResult.ResponseData;

				if (responseData.Contains("The%20user%20has%20denied%20access"))
					throw new UserDeniedAccessExeption("User did not accept required scopes");

				var responseUri = new Uri(responseData);

				if (errorDetail > 0 || responseUri.Fragment.Length > 0)
				{
					throw new HealthRequestException(responseUri.Fragment);
				}

				var code = responseUri.Query.Split('=')[1].Split('&')[0];
				return code;
			}
			catch (UserDeniedAccessExeption)
			{
				throw;
			}
			catch (UserCancelSignInExeption)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new HealthRequestException("Failed to sign in user", e);
			}
		}

		private static Uri CreateOAuthCodeRequestUri()
		{
			UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_authorize.srf");
			var query = new StringBuilder();

			query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(RedirectUri));
			query.AppendFormat("&client_id={0}", Uri.EscapeUriString(ClientId));
			query.AppendFormat("&scope={0}", Uri.EscapeUriString(Scopes));
			query.Append("&response_type=code");

			uri.Query = query.ToString();
			return uri.Uri;
		}

		private async Task<string> GetTokensFromAuthCode(string code)
		{
			return await SendTokenRequest("code", code, "authorization_code");
		}

		private async Task<string> SendTokenRequest(string argKey, string argValue, string grantType)
		{
			var tokenUri = new UriBuilder(TokenUri).Uri;

			Debug.WriteLine($"Token Request Uri = {tokenUri}");

			var formContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("client_id", ClientId),
				new KeyValuePair<string, string>("redirect_uri", RedirectUri),
				new KeyValuePair<string, string>(argKey, argValue),
				new KeyValuePair<string, string>("grant_type", grantType),
			});

			string result;
			try
			{
				using (var http = new HttpClient())
				{
					var resp = await http.PostAsync(tokenUri, formContent).ConfigureAwait(false);
					result = await resp.Content.ReadAsStringAsync();
				}
			}
			catch (Exception e)
			{
				throw new TokenAcquiringException("Error acquiring token", e);
			}

			string authToken, refreshToken;
			try
			{
				var value = JsonValue.Parse(result).GetObject();
				authToken = value.GetNamedValue("access_token").ToString();
				refreshToken = value.GetNamedValue("refresh_token").ToString();
				Debug.WriteLine($"Auth Token = {authToken}");
				Debug.WriteLine($"Refresh Token = {refreshToken}");
			}
			catch (Exception e)
			{
				throw new TokenAcquiringException("Error parsing token", e);
			}

			AddTokenToVault(ResourceName, UserName, authToken);
			AddTokenToVault(RefreshResourceName, UserName, refreshToken);

			return authToken;
		}
	}
}