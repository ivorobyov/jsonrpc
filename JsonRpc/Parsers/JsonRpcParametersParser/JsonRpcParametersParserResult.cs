namespace JsonRpc
{
	class JsonRpcParametersParserResult
	{
		public static JsonRpcParametersParserResult CreateSuccess(object[] parameters)
		{
			return new JsonRpcParametersParserResult
			{
				Success = true,
				JsonRpcParameters = parameters
			};
		}

		public static JsonRpcParametersParserResult CreateFailed(JsonRpcError error)
		{
			return new JsonRpcParametersParserResult
			{
				Success = false,
				JsonRpcError = error
			};
		}

		private JsonRpcParametersParserResult()
		{

		}

		public bool Success { get; private set; }

		public object[] JsonRpcParameters { get; private set; }

		public JsonRpcError JsonRpcError { get; private set; }
	}
}
