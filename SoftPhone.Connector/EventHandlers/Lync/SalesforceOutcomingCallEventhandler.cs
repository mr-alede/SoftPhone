using Hardcodet.Wpf.TaskbarNotification;
using SoftPhone.Connector.Popups;
using SoftPhone.Core.Core;
using SoftPhone.Core.Events.Salesforce;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SoftPhone.Connector.EventHandlers.Lync
{

	public class SalesforceOutcomingCallEventhandler : IDomainEventHandler<SalesforceOutcomingCallEvent>
	{
		public void Handle(SalesforceOutcomingCallEvent domainEvent)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				ShowBalloon("Outcoming call " + domainEvent.Id);
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

