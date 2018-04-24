using System;
using System.Text.RegularExpressions;

namespace boost.Util
{
	public class TimeUtil
	{
		public static TimeSpan GetDuration(string duration)
		{
			try
			{
				var expression = new Regex(@"PT((?<hours>\d+)H)?((?<minutes>\d+)M)?((?<seconds>\d+)S)?");
				var match = expression.Match(duration);

				if (match.Success)
				{
					var hours = GetValue("hours", match);
					var minutes = GetValue("minutes", match);
					var seconds = GetValue("seconds", match);
					return new TimeSpan(hours, minutes, seconds);
				}

				return new TimeSpan(0, 0, 0);
			}
			catch
			{
				return new TimeSpan(0,0,0);
			}

		}

		private static int GetValue(string key, Match match)
		{
			return match.Groups[key].Success ? int.Parse(match.Groups[key].Value) : 0;
		}
	}
}
