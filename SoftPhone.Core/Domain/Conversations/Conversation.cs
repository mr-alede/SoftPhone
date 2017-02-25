using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Core.Domain.Conversations
{
	public class Conversation
	{
		public string Id { get; private set; }

		public List<Contact> Contacts { get; set; }

		public Contact Caller { get { return this.Contacts.First(); }}

		public Contact Self { get; set; }

		public Conversation(string id)
		{
			Id = id;
		}
	}
}
