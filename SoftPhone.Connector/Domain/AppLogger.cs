using NLog;
using NLog.Config;
using SoftPhone.Core.Domain;

namespace SoftPhone.Connector.Domain
{
	public class AppLogger : IAppLogger
	{
		public static readonly Logger Logger;

		static AppLogger()
		{
			Logger = LogManager.GetLogger("file");
		}

		public void Debug(string message)
		{
			Logger.Debug(message);
		}

		public void Error(string message)
		{
			Logger.Error(message);
		}
	}
}
