using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boostFunctions.UserComparison
{
    public class ComparisonData
    {
	    public int Min { get; set; }
		public int Max { get; set; }
		public int UserPercentile { get; set; }
	    public int[] PercentileAmounts { get; set; }
    }
}
