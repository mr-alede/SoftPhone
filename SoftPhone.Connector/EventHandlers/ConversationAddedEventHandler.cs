using Hardcodet.Wpf.TaskbarNotification;
using SoftPhone.Connector.Popups;
using SoftPhone.Core.Conversations;
using SoftPhone.Core.DomainEvents;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SoftPhone.Connector.EventHandlers
{
	public class ConversationAddedEventHandler : IDomainEventHandler<ConversationAddedEvent>
	{
		public void Handle(ConversationAddedEvent evt)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var win = new ConversationWindow();
				ShowDialog(win, evt);

				//ShowBalloon(evt);
			});
		}

		private void ShowDialog(Window window, ConversationAddedEvent evt)
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

		private void ShowBalloon(ConversationAddedEvent evt)
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
