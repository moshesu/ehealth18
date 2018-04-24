using System;

namespace boostFunctions.Util
{
	public static class ArrayExtensions
	{
		public static T RandomItem<T>(this T[] array)
		{
			var rand = new Random();
			return array[rand.Next(0, array.Length)];
		}
	}
}
