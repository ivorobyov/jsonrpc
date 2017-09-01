using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace JsonRpc
{
	class JsonRpcNotification
	{
		[JsonProperty("method")]
		[Required]
		public string Method { get; set; }

		[JsonProperty("params")]
		public object Params { get; set; }
	}
}
