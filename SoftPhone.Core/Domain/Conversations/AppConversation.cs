using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Core.Domain.Conversations
{
	public class AppConversation
	{
		public string Id { get; private set; }
		public string SalesforceId { get; set; }

		//public List<Contact> Contacts { get; set; }

		//public Contact Other { get { return this.Contacts.FirstOrDefault(); }}
		public Contact Other { get; set; }

		public Contact Self { get; set; }

		public ConversationStatus Status { get; private set; }

		public AppConversation(string id, ConversationStatus status)
		{
			Id = id;
			Status = status;
		}

		public bool IsExternalCall
		{
			get
			{
				var caller = this.Other;
				if (caller != null && caller.Uri.Contains("tel:"))
					return true;

				return false;
			}
		}
	}
}
