using System;
using boost.Core.Entities;

namespace boost.Core.App
{
	public interface ISleepFetcher
	{
		Sleep[] GetSleepDuringSpan(DateTime start, DateTime end, string userId = null);
		Sleep[] GetSleepForMonth(DateTime dayInMonth, string userId = null);
	}
}
