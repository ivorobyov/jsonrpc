using System;

namespace JsonRpc
{
    public abstract class JsonRpcService
    {	
		protected JsonRpcResponse Result(object data)
		{
			return Result(data);
		}

		protected JsonRpcResponse<TResult> Result<TResult>(TResult data)
		{
			return new JsonRpcResponse<TResult>
			{
				Result = data
			};
		}

		protected JsonRpcErrorResponse ErrorResult(string message, object data = null)
		{
			if (String.IsNullOrWhiteSpace(message))
				throw new ArgumentNullException(nameof(message));

			return new JsonRpcErrorResponse
			{
				Error = JsonRpcError.Create(JsonRpcErrorCodes.InternalError, message, data)
			};
		}
	}
}
