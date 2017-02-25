using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftPhone.Core.Core
{
	public static class CommandsBus
	{
		[ThreadStatic] //so that each thread has its own callbacks
		private static List<Delegate> actions;

		public static IUnityContainer Container { get; set; } //as before

		//Registers a callback for the given domain event
		public static void Register<T>(Action<T> callback) where T : IAppCommand
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
		public static void Execute<T>(T command) where T : IAppCommand
		{
			if (Container != null)
			{
				var handlers = new[] { Container.Resolve<ICommandHandler<T>>() }.Union(Container.ResolveAll<ICommandHandler<T>>()).ToArray();

				foreach (var handler in handlers)
					handler.Execute(command);
			}

			if (actions != null)
				foreach (var action in actions)
					if (action is Action<T>)
						((Action<T>)action)(command);
		}
	}

}
