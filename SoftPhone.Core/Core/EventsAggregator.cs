using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Core.Core
{
	public static class EventsAggregator
	{
		public static IUnityContainer Container { get; set; } //as before
		//Raises the given domain event
		public static void Raise<T>(T args) where T : IDomainEvent
		{
			if (Container != null)
			{
				var handlers = Container.ResolveAll<IDomainEventHandler<T>>();

				foreach (var handler in handlers)
					handler.Handle(args);
			}
		}
	}
}
