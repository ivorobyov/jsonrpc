using Newtonsoft.Json;

namespace JsonRpc
{
	/// <summary>
	/// JsonRpc 2.0 response model
	/// </summary>
	public class JsonRpcResponse
	{
		internal JsonRpcResponse()
		{

		}
		/// <summary>
		/// Version of the JSON-RPC protocol
		/// </summary>
		[JsonProperty("jsonrpc", Order = 1)]
		public string JsonRpc => "2.0";

		/// <summary>
		/// Result
		/// </summary>
		[JsonProperty("result", Order = 2)]
		public object Result { get; internal set; }

		/// <summary>
		/// Id
		/// </summary>
		[JsonProperty("id", Order = 3)]
		public string Id { get; internal set; }

		public string ToJsonString()
		{
			var settings = new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Ignore
			};

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
