namespace boost.Core.Entities
{
	public enum ActivityType
	{
		Run = 0,
		Bike = 1,
		Golf = 2,
		Hike = 3,
		FreePlay = 4,
		GuidedWorkout = 5,
	}

	public static class ActivityTypeExtensions
	{
		public static string ToFriendlyString(this ActivityType type)
		{
			switch (type)
			{
				case ActivityType.FreePlay:
					return "freePlay";
				case ActivityType.GuidedWorkout:
					return "guidedWorkout";

				default:
					return type.ToString().ToLower();
			}
		}
	}
}