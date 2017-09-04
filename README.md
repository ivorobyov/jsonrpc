# JsonRpc
JsonRpc 2.0 Server for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/bwj74ss7g2njhgdp/branch/master?svg=true)](https://ci.appveyor.com/project/ivorobyov/jsonrpc/branch/master) [![NuGet version](https://badge.fury.io/nu/ivorobyov.JsonRpc.svg)](https://badge.fury.io/nu/ivorobyov.JsonRpc)

The .Net Standart 2.0 library for JSON-RPC 2.0 is a contactless, easy remote procedure call protocol

### Installation

You can start using JsonRpc with our [nuget](https://www.nuget.org/packages/ivorobyov.JsonRpc/) package.

To install JsonRpc, run the following command in the Package Manager Console

```
Install-Package ivorobyov.JsonRpc
```

### Getting Started

Using JsonRpc within a console application;

* Create a console application
* Install ivorobyov.JsonRpc using NuGet

* Create a class of your service by inheriting it from **JsonRpc.JsonRpcService** and create the necessary methods whose return type should be **JsonRpc.JsonRpcResponse**. *Use the **Result(object data)** or **Result\<T\>(T data)** base class method to create a response of type **JsonRpc.JsonRpcResponse***

```c#
class Service : JsonRpc.JsonRpcService
{
  public JsonRpc.JsonRpcResponse Greeting()
  {
    return Result("Hello world!!!");
  }

  public JsonRpc.JsonRpcResponse Sum(int x, int y)
  {
    return Result(x + y);
  }
}
```



* When initializing your application, register your service and configure its methods

```c#
JsonRpc.JsonRpcProcessorProvider.RegisterProcessorFactory<Service>(o => {
  o.RegisterJsonRpcMethod("greeting", s => s.Greeting());
  o.RegisterJsonRpcMethod("sum", s => s.Sum(default(int), default(int)));
});
```

* Create a processor

```c#
var jsonRpcProcessor = JsonRpc.JsonRpcProcessorProvider.CreateProcessor<Service>();
```

* Use the methods **Process()** and **Process\<TResult\>()** to process incoming requests. Use the **ToJsonString()** method to serialize the response in the **Json** string

```c#
var greetingResponse = jsonRpcProcessor.Process("{'method':'greeting', 'id': 1}");

Console.WriteLine(greetingResponse.ToJsonString());

var sumResponse1 = jsonRpcProcessor.Process("{'method':'sum', 'params': [1,2], 'id': 2}");

Console.WriteLine(sumResponse1.ToJsonString());

var sumResponse2 = jsonRpcProcessor.Process("{'method':'sum', 'params': { 'x':3, 'y':4 }, 'id': 3}");

Console.WriteLine(sumResponse2.ToJsonString());
```

* Results 

```
{"jsonrpc":"2.0","result":"Hello world!!!","id":"1"}
{"jsonrpc":"2.0","result":3,"id":"2"}
{"jsonrpc":"2.0","result":7,"id":"3"}
```
