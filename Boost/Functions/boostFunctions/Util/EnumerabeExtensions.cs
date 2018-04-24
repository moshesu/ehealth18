using System;
using System.Collections.Generic;

namespace boostFunctions.Util
{
	public static class EnumerableExtenstions
	{
		public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}

		public static IEnumerable<IList<T>> GetBulk<T>(this IEnumerable<T> enumerable, int bulkSize)
		{
			var list = new List<T>(bulkSize);
			foreach (var item in enumerable)
			{
				list.Add(item);
				if (list.Count >= bulkSize)
				{
					yield return list;
					list = new List<T>(bulkSize);
				}
			}

			if (list.Count > 0)
				yield return list;
		}
	};
}
