using boost.Core.UserComparison;

namespace boost.Core.App
{
	public interface ICompareUser
	{
		ComparisonData GetSleepComparison(string userId = null);

		ComparisonData GetActiveHoursComparison(string userId = null);

		ComparisonData GetStepsComparison(string userId = null);

		Comparison GetComparison(string userId = null);
	}
}
