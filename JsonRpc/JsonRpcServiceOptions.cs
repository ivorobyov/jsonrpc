using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace JsonRpc
{
	class JsonRpcServiceOptions<TJsonRpcService> : IJsonRpcServiceOptions<TJsonRpcService>
		where TJsonRpcService : JsonRpcService
	{
		private Dictionary<string, MethodInfo> methodSelectors = new Dictionary<string, MethodInfo>();

		public void RegisterJsonRpcMethod(string name, Expression<Func<TJsonRpcService, JsonRpcResponse>> methodSelector)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));
			if (methodSelector == null)
				throw new ArgumentNullException(nameof(methodSelector));

			var methodCallExpression = methodSelector.Body as MethodCallExpression;

			if (methodCallExpression == null)
				throw new ArgumentException("The passed expression is not correct.");

			var methodInfo = methodCallExpression.Method;

			if (methodSelectors.ContainsKey(name))
				throw new InvalidOperationException($"Method with name \"{ name }\" already registered.");

			methodSelectors.Add(name, methodInfo);
		}

		public MethodInfo TryGetMethodInfo(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			if (!methodSelectors.ContainsKey(name))
				return null;

			return methodSelectors[name];
		}
	}
}
