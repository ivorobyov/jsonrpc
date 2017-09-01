using Newtonsoft.Json.Linq;

namespace JsonRpc
{
	class JsonRpcRequestParserResult
	{
		public static JsonRpcRequestParserResult CreateSuccess(JObject jObject)
		{
			return new JsonRpcRequestParserResult
			{
				Success = true,
				JsonRpcRequest = new JsonRpcRequest(jObject)
			};
		}

		public static JsonRpcRequestParserResult CreateFailed(JsonRpcError error)
		{
			return new JsonRpcRequestParserResult
			{
				Success = false,
				JsonRpcError = error
			};
		}

		private JsonRpcRequestParserResult()
		{

		}

		public bool Success { get; private set; }

		public JsonRpcRequest JsonRpcRequest { get; private set; }

		public JsonRpcError JsonRpcError { get; private set; }
	}
}
