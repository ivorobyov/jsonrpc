namespace JsonRpc
{
	/// <summary>
	/// JsonRpc 2.0 processor
	/// </summary>
	public interface IJsonRpcProcessor
	{
		/// <summary>
		/// Process JsonRpc 2.0 request
		/// </summary>
		/// <typeparam name="TResult">Type of result</typeparam>
		/// <param name="json">JsonRpc 2.0 request json string</param>
		/// <returns>If response is command return JsonRpcResponse else return null</returns>
		JsonRpcResponse<TResult> Process<TResult>(string json);

		/// <summary>
		/// Process JsonRpc 2.0 request
		/// </summary>
		/// <param name="json">JsonRpc 2.0 request json string</param>
		/// <returns>If response is command return JsonRpcResponse else return null</returns>
		JsonRpcResponse Process(string json);
	}
}
