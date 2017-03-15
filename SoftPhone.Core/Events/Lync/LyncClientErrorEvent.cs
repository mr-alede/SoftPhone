using SoftPhone.Core.Core;

namespace SoftPhone.Core.Events.Lync
{
	public class LyncClientErrorEvent : ErrorEventBase
	{
		public LyncClientErrorEvent(string message):base(message)
		{

		}
	}

}
