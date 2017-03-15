using Hardcodet.Wpf.TaskbarNotification;
using SoftPhone.Connector.Popups;
using SoftPhone.Core.Core;
using SoftPhone.Core.Events.Lync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SoftPhone.Connector.EventHandlers.Lync
{

	public class LyncClientErrorEventHandler : IDomainEventHandler<LyncClientErrorEvent>
	{
		public void Handle(LyncClientErrorEvent domainEvent)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				ShowBalloon(domainEvent.Message);
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
