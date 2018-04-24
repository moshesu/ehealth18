using System.Linq;
using boostFunctions.Comparison;
using boostFunctions.Mocking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace boostFunctionsTests
{
	[TestClass]
	public class ComparisonTest
	{
		[TestMethod]
		public void ComparisonProviderTest()
		{
			var target = new ComparisonProvider(new BoostRepositoryMock());

			var comparison = target.GetUserComparison("Leiah");

			var blockCount = comparison.ActiveHoursComparison.PercentileAmounts;

			Assert.IsTrue(blockCount.Length.Equals(10) && blockCount.Sum()>50);
		}
	}
}
