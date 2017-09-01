using System;
using System.Collections.Generic;

namespace JsonRpc
{
	/// <summary>
	/// Provider of JsonRpc 2.0  processors
	/// </summary>
	public static class JsonRpcProcessorProvider
	{
		private static Dictionary<Type, Func<IJsonRpcProcessor>> factories = new Dictionary<Type, Func<IJsonRpcProcessor>>();

		/// <summary>
		/// Register JsonRpc 2.0  processor
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of JsonRpc 2.0  processor</typeparam>
		/// <param name="configurator">Configurator of JsonRpc 2.0  processor</param>
		public static void RegisterProcessorFactory<TJsonRpcService>(Action<IJsonRpcServiceOptions<TJsonRpcService>> configurator)
			where TJsonRpcService : JsonRpcService, new()
		{
			if (configurator == null)
				throw new ArgumentNullException(nameof(configurator));

			Func<IJsonRpcProcessor> factory = () => new JsonRpcProcessor<TJsonRpcService>(configurator);

			var serviceType = typeof(TJsonRpcService);

			if (factories.ContainsKey(serviceType))
				throw new InvalidOperationException($"Service with type \"{ serviceType.FullName }\" already registered.");

			factories.Add(serviceType, factory);
		}

		/// <summary>
		/// Create JsonRpc 2.0  processor
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of registered JsonRpc 2.0  processor</typeparam>
		/// <returns>JsonRpc 2.0  processor</returns>
		public static IJsonRpcProcessor CreateProcessor<TJsonRpcService>()
			where TJsonRpcService : JsonRpcService, new()
		{
			var serviceType = typeof(TJsonRpcService);

			if (!factories.ContainsKey(serviceType))
				throw new InvalidOperationException($"Service with type \"{ serviceType.FullName }\" is not registered.");

			return factories[typeof(TJsonRpcService)]();
		}
	}
}
