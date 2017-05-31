using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Practices.Unity;
using SoftPhone.Connector.Domain;
using SoftPhone.Connector.IoC;
using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using SoftPhone.Lync.ConversationTracker;
using System;
using System.Windows;
using System.Windows.Threading;

namespace SoftPhone.Connector
{
	public partial class App : Application
	{
		private TaskbarIcon notifyIcon;
		private static readonly IUnityContainer _container = UnityConfig.GetConfiguredContainer();

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

			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += CurrentDomain_UnhandledException;
		}

		protected override void OnExit(ExitEventArgs e)
		{
			notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
			base.OnExit(e);
		}

		public static T Resolve<T>()
		{
			return _container.Resolve<T>();
		}


		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = (Exception)e.ExceptionObject;
			AppLogger.Logger.Error("UnhandledException caught : " + ex.Message);
			AppLogger.Logger.Error("UnhandledException StackTrace : " + ex.StackTrace);
			AppLogger.Logger.Fatal("Runtime terminating: {0}", e.IsTerminating);
		}  
	}
}
