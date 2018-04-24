using boost.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using boost.Cloud.HealthCloud.Exceptions;
using boost.Cloud.HealthCloud.HealthTypes;
using boost.Core.Entities;

namespace boost.Cloud.HealthCloud.DataFetcher
{
	public class HealthDataFetcher
	{
		// Singelton
		private static HealthDataFetcher _instance;

		private HealthDataFetcher()
		{
			_requestsManager = new RequestsManager();
		}

		public static HealthDataFetcher Instance => _instance ?? (_instance = new HealthDataFetcher());

		private readonly RequestsManager _requestsManager;

		public HealthProfileInformation GetProfileInformation()
		{
			var apiResponse = _requestsManager.MakeRequest("me/profile").Result;
			var profile = JsonConvert.DeserializeObject<HealthProfileInformation>(apiResponse);
			return profile;
		}

		public HealthDailySummary[] GetSummaries(DateTime startTime, DateTime endTime)
		{
			var parameters = string.Format("startTime={0}&endTime={1}", startTime.ToApiTime(), endTime.ToApiTime());

			var apiResponse = _requestsManager.MakeRequest("me/summaries/Daily", parameters).Result;

			var json = JsonConvert.DeserializeObject<JObject>(apiResponse);
			if (IsEmpty(json))
				return new HealthDailySummary[0];

			var summaries = json.GetValue("summaries").ToObject<HealthDailySummary[]>();
			return summaries;
		}

		public HealthActivity[] GetActivities(ActivityType type, DateTime startTime, DateTime endTime)
		{
			var parameters = string.Format("startTime={0}&endTime={1}&activityTypes={2}",
				startTime.ToApiTime(), endTime.ToApiTime(), type.ToFriendlyString());

			var apiResponse = _requestsManager.MakeRequest("me/Activities/", parameters).Result;

			var json = JsonConvert.DeserializeObject<JObject>(apiResponse);
			if (IsEmpty(json))
				return new HealthActivity[0];

			var activities = json.GetValue(type.ToFriendlyString() + "Activities").ToObject<HealthActivity[]>();

			activities.ForEach(x => x.Type = type);
			return activities;
		}

		public HealthSleep[] GetSleepRecords(DateTime startTime, DateTime endTime)
		{
			var parameters = string.Format("startTime={0}&endTime={1}&activityTypes=sleep",
				startTime.ToApiTime(), endTime.ToApiTime());

			try
			{
				var apiResponse = _requestsManager.MakeRequest("me/Activities/", parameters).Result;
				var json = JsonConvert.DeserializeObject<JObject>(apiResponse);
				if (IsEmpty(json))
					return new HealthSleep[0];

				var sleepRecords = json.GetValue("sleepActivities").ToObject<HealthSleep[]>();
				return sleepRecords;
			}
			catch (HealthRequestException e)
			{
				throw e;
			}
		}

		private bool IsEmpty(JObject json)
		{
			try
			{
				return (int) json.GetValue("itemCount") == 0;
			}
			catch
			{
				throw new HealthRequestException("Could not find itemCount field in response");
			}
		}

		public void ClearVault()
		{
			_requestsManager.ClearVault();
		}
	}
}