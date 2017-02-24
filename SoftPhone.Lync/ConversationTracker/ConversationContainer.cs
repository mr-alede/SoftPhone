using Microsoft.Lync.Model.Conversation;
using System;

namespace SoftPhone.Lync.ConversationTracker
{
	public class ConversationContainer
	{
		public Conversation Conversation { get; set; }
		public DateTime ConversationCreated { get; set; }
	}
}
