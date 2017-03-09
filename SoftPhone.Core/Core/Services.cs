using Microsoft.Practices.Unity;
using System;

namespace SoftPhone.Core.Core
{
	public static class Services
	{
		public static IUnityContainer Container { get; set; } //as before

		//Raises the given domain event
		public static T Gety<T>() where T : IService
		{
			if (Container != null)
			{
				return Container.Resolve<T>();
			}

			throw new InvalidOperationException(string.Format("Cannot resolve type: {0}", typeof(T)));
		}
	}

}
