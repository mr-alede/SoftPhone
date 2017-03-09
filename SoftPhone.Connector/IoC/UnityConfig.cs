using Microsoft.Practices.Unity;
using SoftPhone.Core.Core;
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
			string[] assemblies = new[] { "SoftPhone.Connector", "SoftPhone.Lync", "SoftPhone.Core", "SoftPhone.Salesforce" };

			foreach (var domainEvent in assemblies.GetIntefaceImplementationsWithWrapper<IDomainEvent>(typeof(IDomainEventHandler<>)))
			{
				foreach (Type eventHandler in domainEvent.Value)
				{
					container.RegisterType(domainEvent.Key, eventHandler);
				}
			}

			foreach (var command in assemblies.GetIntefaceImplementationsWithWrapper<IAppCommand>(typeof(ICommandHandler<>)))
			{
				foreach (Type eventHandler in command.Value)
				{
					container.RegisterType(command.Key, eventHandler);
				}
			}

			foreach (var query in assemblies.GetIntefaceImplementations<IQuery>())
			{
				container.RegisterType(query.Key, query.Value);
			}

			foreach (var repo in assemblies.GetIntefaceImplementations<IRepository>())
			{
				container.RegisterType(repo.Key, repo.Value);
			}

			foreach (var service in assemblies.GetIntefaceImplementations<IService>())
			{
				container.RegisterType(service.Key, service.Value);
			}

		}
	}
}
