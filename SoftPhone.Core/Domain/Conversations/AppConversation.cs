using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Core.Domain.Conversations
{
	public class AppConversation
	{
		public string Id { get; private set; }
		public string SalesforceId { get; set; }

		public List<Contact> Contacts { get; set; }

		public Contact Caller { get { return this.Contacts.First(); }}

		public Contact Self { get; set; }

		public ConversationStatus Status { get; private set; }

		public AppConversation(string id, ConversationStatus status)
		{
			Id = id;
			Status = status;
		}
	}
}
