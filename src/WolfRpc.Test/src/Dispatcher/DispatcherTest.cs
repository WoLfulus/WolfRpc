using WolfRpc.Abstractions;
using WolfRpc.Tests.Shared;
using WolfRpc.Tests.Services;
using WolfRpc.Services;
using Newtonsoft.Json;

namespace WolfRpc.Tests;

public class DispatcherTest
{
    [Fact]
    public void TestSerialization()
    {
        var call = new ServiceRequest(
            Identifier.From("WolfRpc.Tests.Shared.IMyService"),
            Identifier.From("Task<string> Reverse(string)"),
            new object[] {
                "Hello World"
            }
        );

        var serialized = JsonConvert.SerializeObject(call, Formatting.Indented, new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        });

    }

    [Fact]
    public async void TestDispatcher()
    {
        var dispatcher = new Dispatcher();
        dispatcher.AddService<IMyService, MyService>();

        var call = new ServiceRequest(
            Identifier.From("WolfRpc.Tests.Shared.IMyService"), 
            Identifier.From("Task<string> Reverse(string)"), 
            new object[] {
                "Hello World" 
            }
        );
        var result = await dispatcher.Process(call);

        Assert.Equal(call.Id, result.Id);
        Assert.Equal("dlroW olleH", result.Get<string>());
    }
}