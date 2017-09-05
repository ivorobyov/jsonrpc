using System;

namespace JsonRpc
{
	/// <summary>
	/// Provider of JsonRpc 2.0  processors
	/// </summary>
	public interface IJsonRpcProcessorProvider
	{
		/// <summary>
		/// Register JsonRpc 2.0  service
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of JsonRpc 2.0  processor</typeparam>
		/// <param name="configurator">Configurator of JsonRpc 2.0  processor</param>
		IJsonRpcProcessorProvider Register<TJsonRpcService>(Action<IJsonRpcServiceOptions<TJsonRpcService>> configurator)
			where TJsonRpcService : JsonRpcService, new();

		/// <summary>
		/// Register JsonRpc 2.0  service
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of JsonRpc 2.0  processor</typeparam>
		/// <param name="serviceFactory">Factory of JsonRpc 2.0 service </param>
		/// <param name="configurator">Configurator of JsonRpc 2.0  processor</param>
		IJsonRpcProcessorProvider Register<TJsonRpcService>(Func<TJsonRpcService> serviceFactory, Action<IJsonRpcServiceOptions<TJsonRpcService>> configurator)
			where TJsonRpcService : JsonRpcService;

		/// <summary>
		/// Get JsonRpc 2.0  processor
		/// </summary>
		/// <typeparam name="TJsonRpcService">Type of registered JsonRpc 2.0  processor</typeparam>
		/// <returns>JsonRpc 2.0  processor</returns>
		IJsonRpcProcessor Get<TJsonRpcService>()
			where TJsonRpcService : JsonRpcService;
	}
}
