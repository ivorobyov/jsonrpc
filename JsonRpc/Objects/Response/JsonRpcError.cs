using Newtonsoft.Json;

namespace JsonRpc
{
	/// <summary>
	/// JsonRpc 2.0 error model
	/// </summary>
	public class JsonRpcError
	{
		internal static JsonRpcError Create(JsonRpcErrorCodes code, string message, object data = null)
		{
			return new JsonRpcError
			{
				Code = code,
				Message = message,
				Data = data
			};
		}

		/// <summary>
		/// A number that indicates the error type that occurred.
		/// </summary>
		[JsonProperty("code", Order = 1)]
		public JsonRpcErrorCodes Code { get; internal set; }

		/// <summary>
		/// Short description of the error.
		/// </summary>
		[JsonProperty("message", Order = 2)]
		public string Message { get; internal set; }

		/// <summary>
		/// A Primitive or Structured value that contains additional information about the error.
		/// </summary>
		[JsonProperty("data", Order = 3)]
		public object Data { get; internal set; }
	}
}
