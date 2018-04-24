namespace boost.Core.Entities.Users
{
	public class UserTypeRecord
	{
		public string UserId { get; set; }
		public UserType UserType { get; set; }
		public string CoachId { get; set; }

		public UserTypeRecord() { }

		public UserTypeRecord(string userId, UserType userType, string coachId=null)
		{
			UserId = userId;
			UserType = userType;
			CoachId = coachId;
		}
	}
}
