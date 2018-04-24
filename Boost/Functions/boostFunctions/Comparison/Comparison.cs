using System;
using boostFunctions.UserComparison;

namespace boostFunctions.Comparison
{
    public class Comparison
    {
		public DateTime LastCalculated { get; set; }
	    public ComparisonData SleepComparison { get; set; }
	    public ComparisonData StepsComparison { get; set; }
		public ComparisonData ActiveHoursComparison { get; set; }
	}
}
