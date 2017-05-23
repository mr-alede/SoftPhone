using System.Collections.Generic;
using System.Configuration;

namespace SoftPhone.Core.Domain.Salesforce
{
	public class SalesforceCredentials
	{
		public string Login { get; set; }
		public string Password { get; set; }

		private string _instanceName;
		public string InstanceName
		{
			get
			{
				if (string.IsNullOrEmpty(this._instanceName))
					return DefaultInstanceName;

				return _instanceName;
			}

			set 
			{ 
				_instanceName = value; 
			}
		}

		public static readonly string DefaultInstanceName = "UAT";

		public static readonly string[] InstanceNames = new string[] 
		{
			DefaultInstanceName,
			"Sandbox"
		};

		public SalesforceSettings Settings
		{
			get 
			{
				string settingsSrc = ConfigurationManager.AppSettings[InstanceName];
				return SalesforceSettings.Parse(settingsSrc);
			}
		}
	}

}
