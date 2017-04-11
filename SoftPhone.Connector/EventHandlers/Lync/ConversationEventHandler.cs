using Hardcodet.Wpf.TaskbarNotification;
using SoftPhone.Connector.Popups;
using SoftPhone.Core.Domain.Conversations;
using SoftPhone.Core.Core;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Connector.EventHandlers.Lync
{
	public class ConversationEventHandler : IDomainEventHandler<ConversationEvent>
	{
		static Dictionary<string, ConversationWindow> ActiveConversationPopups = new Dictionary<string, ConversationWindow>();

		public void Handle(ConversationEvent evt)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (evt.Conversation.Status == ConversationStatus.Finished ||
					evt.Conversation.Status == ConversationStatus.Unanswered)
				{
					foreach (var popup in ActiveConversationPopups.Select(x=>x).ToList())
					{
						popup.Value.Close();
						ActiveConversationPopups.Remove(popup.Key);
					}
				}
				else
				{
					ConversationWindow win;

					if (!ActiveConversationPopups.ContainsKey(evt.Conversation.Id))
					{
						win = new ConversationWindow();
						ActiveConversationPopups[evt.Conversation.Id] = win;
					}
					else
					{
						win = ActiveConversationPopups[evt.Conversation.Id];
					}

					ShowDialog(win, evt);
				}
			});
		}

		private void ShowDialog(Window window, ConversationEvent evt)
		{
			var caller = evt.Conversation.Other;

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
