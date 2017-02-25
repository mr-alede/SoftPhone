using System.Collections.Generic;

namespace SoftPhone.Core.Domain.Conversations
{
	public  class Contact
	{
		public string Uri { get; set; }
		public string Name { get; set; }

		public List<ContactEndpoint> Endpoints { get; set; }
	}
}