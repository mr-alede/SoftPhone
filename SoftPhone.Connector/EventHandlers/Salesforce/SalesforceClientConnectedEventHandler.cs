using Hardcodet.Wpf.TaskbarNotification;
using SoftPhone.Connector.Popups;
using SoftPhone.Core.Core;
using System.Windows;
using System.Windows.Controls.Primitives;
using SoftPhone.Core.Events.Salesforce;

namespace SoftPhone.Connector.EventHandlers.Salesforce
{
	public class SalesforceClientConnectedEventHandler : IDomainEventHandler<SalesforceClientConnectedEvent>
	{
		public void Handle(SalesforceClientConnectedEvent domainEvent)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				ShowBalloon("Connected to Salesforce.");
			});
		}

		private void ShowBalloon(string message)
		{
			var notifyIcon = Application.Current.Resources["NotifyIcon"] as TaskbarIcon;
			if (notifyIcon != null)
			{
				var balloon = new ToastBalloon();
				balloon.BalloonText = message;

				//show balloon and close it after 3 seconds
				notifyIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 3000);
			}
		}

	}
}

