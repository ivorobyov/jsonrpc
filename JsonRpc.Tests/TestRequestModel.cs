using Newtonsoft.Json;

namespace JsonRpc.Tests
{
	class TestRequestModel
	{
		[JsonProperty("x")]
		public int X { get; set; }

		[JsonProperty("y")]
		public int Y { get; set; }
	}
}
