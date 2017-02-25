namespace SoftPhone.Core.Core
{
	class ICommandHandler
	{
	}

	public interface ICommandHandler<in TCommand> where TCommand : IAppCommand
	{
		void Execute(TCommand command);
	}

}
