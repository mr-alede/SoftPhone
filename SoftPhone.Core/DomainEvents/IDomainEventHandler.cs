namespace SoftPhone.Core.DomainEvents
{
	public interface IDomainEventHandler<T> where T : IDomainEvent
	{
		void Handle(T args);
	}
}
