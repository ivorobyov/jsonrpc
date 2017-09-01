namespace JsonRpc
{
    public abstract class JsonRpcService
    {	
		protected JsonRpcResponse Result(object data)
		{
			return Result(data);
		}

		protected JsonRpcResponse Result<TResult>(TResult data)
		{
			return new JsonRpcResponse<TResult>
			{
				Result = data
			};
		}
	}
}
