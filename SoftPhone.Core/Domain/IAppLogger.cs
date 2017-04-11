namespace SoftPhone.Core.Domain
{
	public interface  IAppLogger
	{
		void Debug(string message);
		void Error(string message);
	}
}
