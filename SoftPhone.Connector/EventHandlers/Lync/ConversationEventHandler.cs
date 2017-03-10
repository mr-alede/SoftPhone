using Hardcodet.Wpf.TaskbarNotification;
using SoftPhone.Connector.Popups;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Core;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SoftPhone.Connector.EventHandlers.Lync
{
	public class ConversationEventHandler : IDomainEventHandler<ConversationEvent>
	{
		public void Handle(ConversationEvent evt)
		{
			if (evt.Conversation.Status == ConversationStatus.Finished)
				return;

			Application.Current.Dispatcher.Invoke(() =>
			{
				var win = new ConversationWindow();
				ShowDialog(win, evt);

				//ShowBalloon(evt);
			});
		}

		private void ShowDialog(Window window, ConversationEvent evt)
		{
			var caller = evt.Conversation.Caller;

			window.DataContext = new ConversationWindowViewModel
			{
				Name = caller.Name,
				Uri = caller.Uri
			};

			//window.Owner = this;
			window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			window.ShowDialog();
		}

		private void ShowBalloon(ConversationEvent evt)
		{
			var notifyIcon = Application.Current.Resources["NotifyIcon"] as TaskbarIcon;
			if (notifyIcon != null)
			{
				var balloon = new ConversationBalloon();
				balloon.BalloonText = "Custom Balloon";

				//show balloon and close it after 4 seconds
				notifyIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 4000);
			}
		}

	}
}
