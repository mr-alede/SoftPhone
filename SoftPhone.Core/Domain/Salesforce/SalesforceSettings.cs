using System.Configuration;

namespace SoftPhone.Core.Domain.Salesforce
{

	public static class SalesforceSettings
	{
		public static string ConsumerKey { get { return ConfigurationSettings.AppSettings["consumerKey"]; } }
		public static string ConsumerSecret { get { return ConfigurationSettings.AppSettings["consumerSecret"]; } }
	}

}
