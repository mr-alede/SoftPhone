using NLog;
using SoftPhone.Core.Domain;

namespace SoftPhone.Connector.Domain
{
	public class AppLogger : IAppLogger
	{
		private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
		public void Debug(string message)
		{
			_logger.Debug(message);
		}

		public void Error(string message)
		{
			_logger.Error(message);
		}
	}
}
