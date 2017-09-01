using Newtonsoft.Json.Linq;
using System;

namespace JsonRpc
{

	static class JsonRpcRequestParser
	{
		private const string JconRpcMethodPropertyName = "method";

		public static JsonRpcRequestParserResult Parse(string json)
		{
			if (String.IsNullOrWhiteSpace(json))
				return JsonRpcRequestParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.ParseError,
						"The request must not be null or empty string."
					)
				);

			JObject requestObject = null;

			try
			{
				requestObject = JObject.Parse(json);
			}
			catch
			{
				return JsonRpcRequestParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.ParseError,
						"Invalid json format."
					)
				);
			}

			var methodProperty = requestObject.Property(JconRpcMethodPropertyName);

			if (requestObject.Property(JconRpcMethodPropertyName) == null)
				return JsonRpcRequestParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.InvalidRequest,
						$"Property \"{JconRpcMethodPropertyName}\" is required"
					)
				);

			if (String.IsNullOrWhiteSpace(requestObject.Property(JconRpcMethodPropertyName).Value.ToString()))
				return JsonRpcRequestParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.InvalidRequest,
						$"Property \"{JconRpcMethodPropertyName}\" is must be not empty."
					)
				);

			if (requestObject.Property(JconRpcMethodPropertyName).Value.Type != JTokenType.String)
				return JsonRpcRequestParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.InvalidRequest,
						$"Incorrect type of property \"{JconRpcMethodPropertyName}\"."
					)
				);

			return JsonRpcRequestParserResult.CreateSuccess(requestObject);
		}
	}
}
