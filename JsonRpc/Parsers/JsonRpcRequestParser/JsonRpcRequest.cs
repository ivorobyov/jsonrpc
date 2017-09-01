using Newtonsoft.Json.Linq;

namespace JsonRpc
{
	class JsonRpcRequest
	{
		private JObject jObject;

		public JsonRpcRequest(JObject jObject)
		{
			this.jObject = jObject;
		}

		public string MethodName => jObject["method"].ToString();

		public JToken Parameters => jObject["params"] ?? new JObject();

		public string Id => jObject["id"]?.ToString();
	}
}
