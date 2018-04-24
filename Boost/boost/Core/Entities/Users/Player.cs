using System;
using boost.Cloud.HealthCloud.HealthTypes;

namespace boost.Core.Entities.Users
{
	public class Player : User
	{
		public string CoachId;

		public Player() { }
		public Player(HealthProfileInformation healthProfileInfo, string coachId) : base(healthProfileInfo)
		{
			CoachId = coachId;
		}
	}
}
