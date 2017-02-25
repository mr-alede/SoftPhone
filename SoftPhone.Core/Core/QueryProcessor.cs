using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace SoftPhone.Core.Core
{
	public static class QueryProcessor
	{
		[ThreadStatic] //so that each thread has its own callbacks
		private static List<Delegate> actions;

		public static IUnityContainer Container { get; set; } //as before

		//Registers a callback for the given domain event
		public static void Register<T>(Action<T> callback) where T : IDomainEvent
		{
			if (actions == null)
				actions = new List<Delegate>();

			actions.Add(callback);
		}

		//Clears callbacks passed to Register on the current thread
		public static void ClearCallbacks()
		{
			actions = null;
		}

		//Raises the given domain event
		public static T GetQuery<T>() where T : IQuery
		{
			if (Container != null)
			{
				return Container.Resolve<T>();
			}

			throw new InvalidOperationException(string.Format("Cannot resolve type: {0}", typeof(T)));
		}
	}

}
