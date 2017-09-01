using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonRpc
{
	static class JsonRpcParametersParser
	{
		public static JsonRpcParametersParserResult Parse(JToken parameters, MethodInfo methodInfo)
		{
			var methodParameters = methodInfo.GetParameters();

			if (parameters.Count() != methodParameters.Count())
				return JsonRpcParametersParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.InvalidParams,
						$"Invalid number of method parameters. It was expected to be {methodParameters.Count()} parameters."
					)
				);

			return Parse((dynamic)parameters, methodParameters);
		}

		private static JsonRpcParametersParserResult Parse(dynamic jsonParameters, ParameterInfo[] methodParameters)
		{
			throw new NotSupportedException();
		}

		private static JsonRpcParametersParserResult Parse(JObject jsonParameters, ParameterInfo[] methodParameters)
		{
			var parameters = jsonParameters.Properties().Select(p => new { name = p.Name, value = p.Value }).ToArray();

			var paramsError =
					from mp in methodParameters
					join p in parameters
						on mp.Name equals p.name into outer
					from jp in outer.DefaultIfEmpty()
					select new
					{
						mp,
						jp,
						isValid = jp != null ? jp.value.ToObject(mp.ParameterType).GetType() == mp.ParameterType && jp.name == mp.Name.ToLower() : false
					} into resutl
					where !resutl.isValid
					select resutl.jp != null ?
					$"Invalid type of parameter \"{resutl.jp.name}\". Type \"{resutl.mp.ParameterType.Name}\" was expected"
					:
					$"Missing parameter \"{resutl.mp.Name.ToLower()}\"";

			if (paramsError.Any())
				return JsonRpcParametersParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.InvalidParams,
						String.Join("; ", paramsError)
					)
				);

			var arrayParams = methodParameters.Join(
				jsonParameters.Properties(),
				info => info.Name.ToLower(),
				p => p.Name.ToLower(),
				(info, p) => p.Value.ToObject(info.ParameterType)
			)
			.ToArray();

			return JsonRpcParametersParserResult.CreateSuccess(arrayParams);
		}

		private static JsonRpcParametersParserResult Parse(JArray jsonParameters, ParameterInfo[] methodParameters)
		{
			var paramsError = jsonParameters
				.SelectMany(
					p => methodParameters,
					(jsonParam, methodParam) =>
					{
						try
						{
							return new
							{
								jsonParam,
								methodParam,
								IsValid = jsonParam.ToObject(methodParam.ParameterType).GetType() == methodParam.ParameterType,
							};
						}
						catch
						{
							return new
							{
								jsonParam,
								methodParam,
								IsValid = false
							};
						}
					}
				)
				.Where(entry => !entry.IsValid)
				.Select(entry => $"Invalid type of parameter \"{entry.methodParam.Name}\"")
				.ToArray();

			if (paramsError.Any())
				return JsonRpcParametersParserResult.CreateFailed(
					JsonRpcError.Create(
						JsonRpcErrorCodes.InvalidParams,
						String.Join("; ", paramsError)
					)
				);

			var arrayList = new List<object>();

			for (int i = 0; i < methodParameters.Count(); i++)
				arrayList.Add(jsonParameters[i].ToObject(methodParameters[i].ParameterType));

			return JsonRpcParametersParserResult.CreateSuccess(arrayList.ToArray());
		}
	}
}

