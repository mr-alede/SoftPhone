using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Core.Conversations
{
	public class Conversation
	{
		public List<Contact> Contacts { get; set; }

		public Contact Caller { get { return this.Contacts.First(); }}
	}
}
