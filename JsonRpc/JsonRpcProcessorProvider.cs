using System;
using System.Collections.Generic;

namespace JsonRpc
{
	/// <summary>
	/// Provider of JsonRpc 2.0 services
	/// </summary>
	public class JsonRpcProcessorProvider : IJsonRpcProcessorProvider
	{
		private Dictionary<Type, IJsonRpcProcessor> processors = new Dictionary<Type, IJsonRpcProcessor>();

		/// <summary>
		/// Register JsonRpc 2.0 service
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of JsonRpc 2.0  processor</typeparam>
		/// <param name="configurator">Configurator of JsonRpc 2.0  processor</param>
		public IJsonRpcProcessorProvider Register<TJsonRpcService>(Action<IJsonRpcServiceOptions<TJsonRpcService>> configurator)
			where TJsonRpcService : JsonRpcService, new()
		{
			return Register(() => new TJsonRpcService(), configurator);
		}

		/// <summary>
		/// Register JsonRpc 2.0 service
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of JsonRpc 2.0  processor</typeparam>
		/// <param name="configurator">Configurator of JsonRpc 2.0  processor</param>
		public IJsonRpcProcessorProvider Register<TJsonRpcService>(Func<TJsonRpcService> serviceFactory, Action<IJsonRpcServiceOptions<TJsonRpcService>> configurator)
			where TJsonRpcService : JsonRpcService
		{
			if (configurator == null)
				throw new ArgumentNullException(nameof(configurator));

			var service = serviceFactory();

			var processor = new JsonRpcProcessor<TJsonRpcService>(service, configurator);

			var serviceType = typeof(TJsonRpcService);

			if (processors.ContainsKey(serviceType))
				throw new InvalidOperationException($"Service with type \"{ serviceType.FullName }\" already registered.");

			processors.Add(serviceType, processor);

			return this;
		}

		/// <summary>
		/// Get JsonRpc 2.0  processor
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of registered JsonRpc 2.0  processor</typeparam>
		/// <returns>JsonRpc 2.0  processor</returns>
		public IJsonRpcProcessor Get<TJsonRpcService>()
			where TJsonRpcService : JsonRpcService
		{
			var serviceType = typeof(TJsonRpcService);

			if (!processors.ContainsKey(serviceType))
				throw new InvalidOperationException($"Service with type \"{ serviceType.FullName }\" is not registered.");

			return processors[typeof(TJsonRpcService)];
		}
	}
}
