using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace SoftPhone.Core.Domain.Salesforce
{
	public class SalesforceCredentials : IEquatable<SalesforceCredentials>
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

		public bool Equals(SalesforceCredentials other)
		{
			if (other == null)
				return false;

			return this.Login == other.Login &&
				this.Password == other.Password &&
				this.InstanceName == other.InstanceName;
		}

		public static SalesforceCredentials Parse(string source)
		{
			var credentials = new JsonSerializerSettings
			{
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				ContractResolver = new PrivateResolver()
			};

			var result = JsonConvert.DeserializeObject<SalesforceCredentials>(source, credentials);
			return result;
		}

		private class PrivateResolver : DefaultContractResolver
		{
			protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
			{
				var prop = base.CreateProperty(member, memberSerialization);

				if (!prop.Writable)
				{
					var property = member as PropertyInfo;
					if (property != null)
					{
						var hasPrivateSetter = property.GetSetMethod(true) != null;
						prop.Writable = hasPrivateSetter;
					}
				}

				return prop;
			}
		}


		public string ToJSON()
		{
			return JsonConvert.SerializeObject(this);
		}
	}

}
