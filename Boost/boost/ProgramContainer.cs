using boost.Cloud.AzureDatabase;
using boost.Core;
using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Progress;
using boost.Core.UserActivity;
using boost.Core.UserComparison;
using boost.PushNotifications;
using boost.Repositories;
using SimpleInjector;

namespace boost
{
	public class ProgramContainer
	{
		public static readonly Container container;

		static ProgramContainer()
		{
			container = new Container();

			container.Register <ILocalStorage, LocalStorage>();
			container.Register <IBalanceHandler, BalanceHandler>();
			container.Register <IGoalsHandler, GoalsHandler> ();
			container.Register <IProgressFetcher, ProgressFetcher> ();
			container.Register <ISignInFlow, SignInFlow> ();
			container.Register <ITransactionRepository, TransactionRepository> ();
			container.Register <ICoachRepository, CoachRepository> ();
			container.Register <IGoalsRepository, GoalsRepository> ();
			container.Register <IPlayerRepository, PlayerRepository> ();
			container.Register <IProgressRepository, ProgressRepository> ();
			container.Register <IUserTypeRepository, UserTypeRepository> ();
			container.Register <ISleepRepository, SleepRepository> ();
			container.Register <IActivityRepository, ActivityRepository>();
			container.Register <IActivityBuilder, ActivityBuilder>();
			container.Register <IActivityFetcher, ActivityFetcher>();
			container.Register<IProgressBuilder, ProgressBuilder>();
			container.Register<ICoachPlayersPage, CoachPlayersPage>();
			container.Register<IBalanceUpdater, BalanceUpdater>();
			container.Register<ISleepFetcher, SleepFetcher>();
			container.Register<IComparisonFetcher, ComparisonFetcher>();
			container.Register<ICompareUser, CompareUser>();
			container.Register< INotificationsCenter, NotificationsCenter>();
			container.Register< INotificationBuilder, NotificationBuilder>();
			container.RegisterCollection<INotificationsFetcher>(
				new[]
				{
					typeof(MorningNotification),
					typeof(ComparisonNotification),
					typeof(CrystalsNotification)
				});

			container.Verify();
		}
	}

}

