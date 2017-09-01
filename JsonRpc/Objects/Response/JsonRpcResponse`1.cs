using Newtonsoft.Json;

namespace JsonRpc
{
	/// <summary>
	/// JsonRpc 2.0 response model
	/// </summary>
	/// <typeparam name="TResult">Type of result</typeparam>
	public class JsonRpcResponse<TResult> : JsonRpcResponse
	{
		/// <summary>
		/// Result
		/// </summary>
		[JsonProperty("result", Order = 2)]
		public new TResult Result
		{
			get
			{
				return (TResult)base.Result;
			}
			set
			{
				base.Result = value;
			}
		}
	}
}
