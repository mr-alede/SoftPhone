using NLog;
using NLog.Config;
using SoftPhone.Core.Domain;

namespace SoftPhone.Connector.Domain
{
	public class AppLogger : IAppLogger
	{
		private readonly Logger _logger;

		public AppLogger()
		{
			this._logger = LogManager.GetLogger("file");
		}

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
