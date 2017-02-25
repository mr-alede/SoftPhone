namespace SoftPhone.Core.Core
{
	public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
	{
		void Handle(TEvent domainEvent);
	}
}
