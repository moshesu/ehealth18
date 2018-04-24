using System;
using boost.Cloud.AzureDatabase;
using boost.Core.App;

namespace boost.Core.UserComparison
{
	public class CompareUser : ICompareUser
	{
		private readonly ILocalStorage _localStorage;
		private readonly IComparisonFetcher _comparisonFetcher;

		public CompareUser(ILocalStorage localStorage, IComparisonFetcher comparisonFetcher)
		{
			_localStorage = localStorage;
			_comparisonFetcher = comparisonFetcher;
		}

		public ComparisonData GetSleepComparison(string userId = null)
		{
			var comparison = GetComparison(userId);
			return comparison.SleepComparison;
		}

		public ComparisonData GetActiveHoursComparison(string userId = null)
		{
			var comparison = GetComparison(userId);
			return comparison.ActiveHoursComparison;
		}

		public ComparisonData GetStepsComparison(string userId = null)
		{
			var comparison = GetComparison(userId);
			return comparison.StepsComparison;
		}

		public Comparison GetComparison(string userId = null)
		{
			if (userId == null)
				userId = _localStorage.GetCurrentUserId();

			Comparison comparison = null;
			//comparison = _localStorage.GetComparison();//TODO RETURN after presentation

			if (comparison == null || InvalidDate(comparison))
			{
				comparison = _comparisonFetcher.GetUserComparison(userId);
				_localStorage.SetComparison(comparison);
				return comparison;
			}

			return comparison;
		}

		private static bool InvalidDate(Comparison comparison)
		{
			return DateTime.Today - comparison.LastCalculated > new TimeSpan(5, 0, 0, 0);
		}
	}
}
