using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JsonRpc
{
	/// <summary>
	/// Configuration of JsonRpc 2.0  service
	/// </summary>
	/// <typeparam name="TJsonRpcService">Type of of JsonRpc 2.0  service</typeparam>
	public interface IJsonRpcServiceOptions<TJsonRpcService>
		where TJsonRpcService : JsonRpcService
	{
		/// <summary>
		/// Register JsonRpc 2.0  method
		/// </summary>
		/// <param name="name">JsonRpc 2.0  method name</param>
		/// <param name="methodSelector">An expression that selects a method from the service</param>
		void RegisterJsonRpcMethod(string name, Expression<Func<TJsonRpcService, JsonRpcResponse>> methodSelector);
	}
}
