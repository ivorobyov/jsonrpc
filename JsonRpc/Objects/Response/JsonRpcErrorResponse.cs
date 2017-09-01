using Newtonsoft.Json;

namespace JsonRpc
{
	/// <summary>
	/// JsonRpc 2.0 response with error model
	/// </summary>
	public class JsonRpcErrorResponse : JsonRpcResponse
    {
		/// <summary>
		/// Error
		/// </summary>
		[JsonProperty("error", Order = 2)]
		public JsonRpcError Error { get; set; }
	}
}
