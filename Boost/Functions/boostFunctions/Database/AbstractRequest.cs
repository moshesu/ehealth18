using System;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace boostFunctions.Database
{
	public abstract class AbstractRequest
	{
		public abstract string TableName { get; }

		protected const string DateFormat = "yyyy-MM-ddTHH\\:mm\\:ss";
		public IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter {DateTimeFormat = DateFormat};

		public abstract void InitSaveData(HttpRequestMessage req);
		public abstract void InitGetData(HttpRequestMessage req);

		public HttpResponseMessage Save(HttpRequestMessage req, TraceWriter log)
		{
			try
			{
				InitSaveData(req);
				var query = GetUpsertQuery();

				var successful = SaveToDatabase(query, log);

				return !successful
					? req.CreateResponse(HttpStatusCode.BadRequest, "Unable to process your request!")
					: req.CreateResponse(HttpStatusCode.OK, "OK");
			}
			catch (Exception e)
			{
				log.Error(e.ToString());
				return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
			}
		}

		public HttpResponseMessage Get(HttpRequestMessage req, TraceWriter log)
		{
			InitGetData(req);
			try
			{
				var query = GetSelectQuery();

				var result = GetFromDatabase(query, log);

				if (result == null)
					return req.CreateResponse(HttpStatusCode.NotFound, "No such user!");

				return SerializeRespone(req, result);
			}
			catch (Exception e)
			{
				log.Error(e.ToString());
				return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
			}
		}

		public object Get(string query, TraceWriter log = null)
		{
			return GetFromDatabase(query, log);
		}

		public abstract string GetUpsertQuery();
		public abstract string GetSelectQuery();

		public object GetFromDatabase(string query, TraceWriter log)
		{
			try
			{
				var cnnString = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;

				using (var connection = new SqlConnection(cnnString))
				{
					connection.Open();

					using (var command = new SqlCommand(query, connection))
					using (var reader = command.ExecuteReader())
					{
						log?.Info("Query executed successfully!");
						return ReaderToObject(reader);
					}
				}
			}
			catch (Exception e)
			{
				log?.Info(e.ToString());
				return null;
			}
		}

		public static bool SaveToDatabase(string query, TraceWriter log)
		{
			try
			{
				var cnnString = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;

				using (var connection = new SqlConnection(cnnString))
				{
					var command = new SqlCommand(query, connection);

					connection.Open();

					command.ExecuteReader();
				}
			}
			catch (Exception e)
			{
				log.Info(e.ToString());
				return false;
			}

			return true;
		}

		public abstract object ReaderToObject(SqlDataReader reader);

		public static string GetParameter(HttpRequestMessage req, string key)
		{
			var result = req.GetQueryNameValuePairs()
				.FirstOrDefault(q => String.Compare(q.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
				.Value;

			return result.Contains("\\\"") ? Regex.Unescape(result) : result;
		}

		public static string PadString(string str) => "'" + str + "'";

		public static string PadDate(DateTime date) => PadString(date.ToString("yyyy-MM-ddTHH\\:mm\\:ss"));

		public static HttpResponseMessage SerializeRespone(HttpRequestMessage req, object result)
		{
			var response = req.CreateResponse(HttpStatusCode.OK);

			response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
			return response;
		}
	}
}