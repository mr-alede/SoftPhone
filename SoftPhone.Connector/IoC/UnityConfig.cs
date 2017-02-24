using Microsoft.Practices.Unity;
using SoftPhone.Core.DomainEvents;
using System;

namespace SoftPhone.Connector.IoC
{
	public class UnityConfig
	{
		#region Unity Container
		private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() => {
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});

		public static IUnityContainer GetConfiguredContainer()
		{
			return container.Value;
		}
		#endregion

		public static void RegisterTypes(IUnityContainer container)
		{
			string[] assemblies = new[] { "SoftPhone.Connector", "SoftPhone.Lync" };

			foreach (var ev in assemblies.GetIntefaceImplementations<IDomainEvent>())
			{
				container.RegisterType(ev.Key, ev.Value);
			}

		}
	}
}
