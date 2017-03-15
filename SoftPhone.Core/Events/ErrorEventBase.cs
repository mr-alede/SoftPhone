using SoftPhone.Core.Core;

namespace SoftPhone.Core.Events
{
	public abstract class ErrorEventBase : IDomainEvent
	{
		public string Message { get; private set; }
		public ErrorEventBase(string message)
		{
			Message = message;
		}
	}

}
