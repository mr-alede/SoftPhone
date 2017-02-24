using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoftPhone.Connector.IoC
{
	public static class TypeExtensions
	{
		public static IEnumerable<Assembly> GetAssemblies(this IEnumerable<string> assembliesNames)
		{
			Assembly[] assemblies = assembliesNames
				.Select(Assembly.Load)
				.ToArray();

			return assemblies;
		}

		public static IDictionary<Type, Type> GetIntefaceImplementations<TInterface>(this IEnumerable<string> assembliesNames)
			where TInterface : class
		{
			if (assembliesNames == null)
				throw new ArgumentNullException("assembliesNames");

			IEnumerable<Assembly> assemblies = GetAssemblies(assembliesNames);

			Type interfaceType = typeof(TInterface);

			IEnumerable<Type> allTypes = assemblies
				.SelectMany(x => x.GetTypes())
				.ToArray();

			var result = allTypes
				.Where(x => interfaceType.IsAssignableFrom(x) && (x.IsAbstract == false) && (x.IsInterface == false))
				.Where(x => x.GetInterfaces()
					.Where(t => t != interfaceType)
					.Any(interfaceType.IsAssignableFrom))
				.ToDictionary(x => x.GetInterfaces()
					.Where(t => t != interfaceType)
					.FirstOrDefault(interfaceType.IsAssignableFrom),
					x => x)
				.Where(x => x.Value != null)
				.ToDictionary(x => x.Key, x => x.Value);

			return result;
		}

		public static IDictionary<Type, Type[]> GetIntefaceImplementationsWithWrapper<TInterface>(this IEnumerable<string> assembliesNames, Type wrappingType)
			where TInterface : class
		{
			if (assembliesNames == null)
				throw new ArgumentNullException("assembliesNames");

			if (wrappingType == null)
				throw new ArgumentNullException("wrappingType");

			if (wrappingType.IsGenericType == false)
				throw new ArgumentException("Parameter wrappingType contains not generic type.");

			IEnumerable<Assembly> assemblies = GetAssemblies(assembliesNames);

			Type interfaceType = typeof(TInterface);

			IEnumerable<Type> allTypes = assemblies
				.SelectMany(x => x.GetTypes())
				.ToArray();

			Dictionary<Type, Type[]> result = allTypes
				.Where(x => interfaceType.IsAssignableFrom(x) && (x.IsAbstract == false) && (x.IsInterface == false))
				.Select(x => wrappingType.MakeGenericType(x))
				.ToDictionary(x => x,
					x => allTypes
						.Where(x.IsAssignableFrom)
						.ToArray())
				.Where(x => (x.Value != null) && (x.Value.Any()))
				.ToDictionary(x => x.Key, x => x.Value);

			return result;
		}
	}
}
