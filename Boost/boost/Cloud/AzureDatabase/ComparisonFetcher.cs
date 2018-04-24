using boost.Core.Entities.Users;
using boost.Core.UserComparison;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public interface IComparisonFetcher
	{
		Comparison GetUserComparison(string userId);
	}
	public class ComparisonFetcher : AbstractAzureRepository, IComparisonFetcher
	{
		public Comparison GetUserComparison(string userId)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var result = CallAzureUserComparison(userIdParameter);
			if (result == null)
				return null;

			return JsonConvert.DeserializeObject<Comparison>(result);
		}
	}
}
