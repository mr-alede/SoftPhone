using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Reflection;

namespace SoftPhone.Core.Domain.Salesforce
{

	public class SalesforceSettings
	{
		public string ConsumerKey { get; private set; }
		public string ConsumerSecret { get; private set; }

		public string SecurityToken { get; private set; }
		public string InstanceUrl { get; private set; }

		private SalesforceSettings()
		{
		}

		public static SalesforceSettings Parse(string source)
		{
			var settings = new JsonSerializerSettings
			{
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				ContractResolver = new PrivateResolver()
			};

			var result = JsonConvert.DeserializeObject<SalesforceSettings>(source, settings);
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
	}


}
