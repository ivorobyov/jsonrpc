using Newtonsoft.Json;

namespace JsonRpc
{
	class JsonRpcCommand : JsonRpcNotification
	{
		[JsonProperty("id")]
		public string Id { get; set; }
	}
}
