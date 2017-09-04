using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace JsonRpc.Tests
{
	[TestClass]
	public class JsonRpcProcessorProviderTests
	{
		private const string Method1Name = "method1";
		private const string Method2Name = "method2";
		private const string Method3Name = "method3";

		static JsonRpcProcessorProviderTests()
		{
			JsonRpcProcessorProvider.RegisterProcessorFactory<TestJsonRpcService>(o =>
			{
				o.RegisterJsonRpcMethod(Method1Name, s => s.Method1());
				o.RegisterJsonRpcMethod(Method2Name, s => s.Method2(0, 0));
				o.RegisterJsonRpcMethod(Method3Name, s => s.Method3(null));
			});
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CheckRegisterExistingJsonRpcService()
		{
			JsonRpcProcessorProvider.RegisterProcessorFactory<TestJsonRpcService>(o =>
			{
			});
		}

		[TestMethod]
		public void CheckCreatedProcessorType()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			Assert.IsInstanceOfType(processor, typeof(IJsonRpcProcessor));
		}

		[TestMethod]
		public void CheckReturnedResponseType()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var result = processor.Process($"{{'method' : '{Method1Name}', 'id': '1'}}");

			Assert.IsInstanceOfType(result, typeof(JsonRpcResponse));
		}

		[TestMethod]
		public void CheckReturnedResponseValue()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var result = processor.Process($"{{'method' : '{Method1Name}', 'id': '1'}}");

			Assert.AreEqual("Ok", result.Result);
		}

		[TestMethod]
		public void CheckReturnedResponseSumValueWithArrayParams()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			int x = 1, y = 2;

			var result = processor.Process($"{{'method' : '{Method2Name}', 'params': [{x},{y}], 'id': '1'}}");

			Assert.AreEqual(x + y, result.Result);
		}

		[TestMethod]
		public void CheckReturnedResponseSumValueWithRequestModelParams()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var testRequestModel = new TestRequestModel
			{
				X = 1,
				Y = 2
			};

			var parameters = JsonConvert.SerializeObject(testRequestModel);

			var result = processor.Process($"{{'method' : '{Method2Name}', 'params': {parameters}, 'id': '1'}}");

			Assert.AreEqual(testRequestModel.X + testRequestModel.Y, result.Result);
		}

		[TestMethod]
		public void CheckReturnedResponseSumValueWithInvalidRequestModelParams()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var testRequestModel = new
			{
				X = 1,
				Z = 2
			};

			var parameters = JsonConvert.SerializeObject(testRequestModel);

			var result = processor.Process($"{{'method' : '{Method2Name}', 'params': {parameters}, 'id': '1'}}") as JsonRpcErrorResponse;

			Assert.IsTrue(result.Error != null && !String.IsNullOrWhiteSpace(result.Error.Message) && result.Error.Code == JsonRpcErrorCodes.InvalidParams);
		}

		[TestMethod]
		public void CheckReturnedErrorResponseType()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var result = processor.Process($"error");

			Assert.IsInstanceOfType(result, typeof(JsonRpcErrorResponse));
		}

		[TestMethod]
		public void CheckReturnedErrorResponseValue()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var result = processor.Process($"error") as JsonRpcErrorResponse;

			Assert.IsTrue(result.Error != null && !String.IsNullOrWhiteSpace(result.Error.Message) && result.Error.Code == JsonRpcErrorCodes.ParseError);
		}

		[TestMethod]
		public void CheckReturnedErrorResponseMethodNotFound()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var result = processor.Process($"{{'method':'invalidMethodName', 'id': '1'}}") as JsonRpcErrorResponse;

			Assert.IsTrue(result.Error != null && !String.IsNullOrWhiteSpace(result.Error.Message) && result.Error.Code == JsonRpcErrorCodes.MethodNotFound);
		}

		[TestMethod]
		public void CheckReturnedResponseId()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var id = Guid.NewGuid();

			var result = processor.Process($"{{'method':'{Method1Name}', 'id': '{id}'}}");

			Assert.AreEqual(id, Guid.Parse(result.Id));
		}

		[TestMethod]
		public void CheckReturnedResponseWithoutId()
		{
			var processor = JsonRpcProcessorProvider.CreateProcessor<TestJsonRpcService>();

			var result = processor.Process($"{{'method':'{Method1Name}' }}");

			Assert.AreEqual(result, null);
		}
	}
}
