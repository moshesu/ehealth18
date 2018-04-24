using System;

namespace boostFunctions.Util
{
	public static class RandomExtensions
	{
		public static int NormalRandomInt(this Random rand, double mean, double standardDeviation, int min, int max)
		{
			double u1 = 1.0 - rand.NextDouble();
			double u2 = 1.0 - rand.NextDouble();
			double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
			                       Math.Sin(2.0 * Math.PI * u2); 
			double randNormal =
				mean + standardDeviation * randStdNormal;

			if (randNormal > max)
				return max;
			if (randNormal < min)
				return min;

			return (int) randNormal;
		}
	}
}
