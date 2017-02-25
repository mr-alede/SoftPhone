using SoftPhone.Core.Domain.Salesforce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone.Salesforce.Client
{
	public static class SfClient
	{
		internal static SfClientStateBase State = new DisconnectedSfClient();

		public static void Init()
		{
			State.Connect(new SalesforceCredentials { Login = "mr.alede1@gmail.com", Password="_passpass123" + "fND1mf1NKH9IKuMfcNEIfICiu" });
		}
	}
}
