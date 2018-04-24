using System;

namespace boost.Core.UserComparison
{
	public class Comparison
	{
		public DateTime LastCalculated { get; set; }
		public ComparisonData SleepComparison { get; set; }
		public ComparisonData StepsComparison { get; set; }
		public ComparisonData ActiveHoursComparison { get; set; }
	}
}
