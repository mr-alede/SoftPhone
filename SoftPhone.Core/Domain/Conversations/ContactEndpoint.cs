namespace SoftPhone.Core.Domain.Conversations
{
	public class ContactEndpoint
	{
		public string Name { get; set; }
		public string Uri { get; set; }

		public EndpointType Type { get; set; }
	}

	public enum EndpointType
	{
		Invalid = -1,
		WorkPhone = 0,
		MobilePhone = 1,
		HomePhone = 2,
		OtherPhone = 3,
		Lync = 4,
		VoiceMail = 5
	}

}
