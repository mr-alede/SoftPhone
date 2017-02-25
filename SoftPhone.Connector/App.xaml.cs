using Hardcodet.Wpf.TaskbarNotification;
using SoftPhone.Connector.IoC;
using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using SoftPhone.Lync.ConversationTracker;
using System.Windows;

namespace SoftPhone.Connector
{
	public partial class App : Application
	{
		private TaskbarIcon notifyIcon;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			EventsAggregator.Container = UnityConfig.GetConfiguredContainer();
			CommandsBus.Container = UnityConfig.GetConfiguredContainer();
			QueryProcessor.Container = UnityConfig.GetConfiguredContainer();

			ConversationTracker.Init();

			CommandsBus.Execute(new SalesforceConnectCommand());

			//create the notifyicon (it's a resource declared in NotifyIconResources.xaml
			notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
		}

		protected override void OnExit(ExitEventArgs e)
		{
			notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
			base.OnExit(e);
		}

	}
}
