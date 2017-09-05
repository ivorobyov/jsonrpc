using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JsonRpc
{
	class JsonRpcProcessor<TJsonRpcService> : IJsonRpcProcessor
		where TJsonRpcService : JsonRpcService
	{
		private TJsonRpcService service;

		private JsonRpcServiceOptions<TJsonRpcService> options;

		public JsonRpcProcessor(TJsonRpcService service, Action<IJsonRpcServiceOptions<TJsonRpcService>> configurator)
		{
			this.service = service;

			options = new JsonRpcServiceOptions<TJsonRpcService>();

			configurator(options);
		}

		public JsonRpcResponse<TResult> Process<TResult>(string json)
		{
			return (JsonRpcResponse<TResult>)Process(json);
		}

		public JsonRpcResponse Process(string json)
		{
			var parseResult = JsonRpcRequestParser.Parse(json);

			if (!parseResult.Success)
				return CreateErrorResponse(parseResult.JsonRpcError);

			var request = parseResult.JsonRpcRequest;

			var methodName = request.MethodName;

			var methodInfo = options.TryGetMethodInfo(methodName);

			if (methodInfo == null)
				return CreateErrorResponse(JsonRpcErrorCodes.MethodNotFound, $"Method \"{methodName}\" not found.");

			var parseParametersResult = JsonRpcParametersParser.Parse(request.Parameters, methodInfo);

			if (!parseParametersResult.Success)
				return CreateErrorResponse(parseParametersResult.JsonRpcError);

			JsonRpcResponse result = null;

			try
			{
				result = (JsonRpcResponse)methodInfo.Invoke(service, parseParametersResult.JsonRpcParameters);
			}
			catch
			{
				return CreateErrorResponse(
					JsonRpcErrorCodes.InternalError,
					"Internal Server Error"
				);
			}

			if (request.Id == null)
				return null;
			else
				result.Id = request.Id;

			return result;
		}

		private bool ValidateParameters(dynamic jsonParameters, MethodInfo methodInfo, out JsonRpcErrorResponse errorResponse, out object[] arrayParams)
		{
			throw new NotSupportedException();
		}

		private JsonRpcErrorResponse CreateErrorResponse(JsonRpcErrorCodes code, string message = null)
		{
			return CreateErrorResponse(
				new JsonRpcError
				{
					Code = code,
					Message = message
				}
			);
		}

		private JsonRpcErrorResponse CreateErrorResponse(JsonRpcError error)
		{
			return new JsonRpcErrorResponse
			{
				Error = error
			};
		}

		private static bool IsAsyncMethod(MethodInfo method)
		{
			Type attType = typeof(AsyncStateMachineAttribute);

			var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

			return (attrib != null);
		}
	}
}
