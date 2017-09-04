namespace JsonRpc.Tests
{
	class TestJsonRpcService : JsonRpcService
	{
		public JsonRpcResponse Method1()
		{
			return Result("Ok");
		}

		public JsonRpcResponse Method2(int x, int y)
		{
			return Result(x + y);
		}

		public JsonRpcResponse Method3(TestRequestModel request)
		{
			return Result(request.X + request.Y);
		}
	}
}
