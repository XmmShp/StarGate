using EventBusNet8;
using EventBusNet8.Abstractions;
using EventBusNet8.Adapter;
using EventBusNet8.Enums;
using EventBusNet8.Fundamental;

public class Listener1
{
    public Listener1(IEventBus eventBus)
    {
        eventBus.ListenEvent("Add", null, ListenNormal);

        eventBus.ListenEvent("Add", ListenNormal);

        eventBus.ListenEvent("Add", EventKey.SomeOneButNullPrefer, ListenStatic);
        eventBus.ListenEvent("AddInter", EventKey.All, ListenInter);
        eventBus.ListenEvent("Add", EventKey.SomeOne, ListenTyped);
        eventBus.ListenEvent("Add", null, ListenPhase, EventPhase.Pre);
    }
    public EventResult ListenNormal(IEventParam param)
    {
        return EventStatus.Continue;
    }

    public static EventResult ListenStatic(IEventParam param)
    {
        Console.WriteLine($"ListenStatic {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return EventStatus.Continue;
    }

    public EventResult ListenInter(IEventParam param)
    {
        Console.WriteLine($"ListenInter {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return EventStatus.Interrupt;
    }

    public EventResult<int> ListenTyped(IEventParam param)
    {
        Console.WriteLine($"ListenTyped {param.Get<int>(0)}+{param.Get<int>(1)}");
        return param.Get<int>(0) + param.Get<int>(1);
    }

    public EventResult<int> ListenPhase(IEventParam param)
    {
        Console.WriteLine($"ListenPhase {param.Get<int>(0)}+{param.Get<int>(1)}");
        param.Set(0, 101);
        param.Set(1, 202);
        return EventStatus.Continue;
    }

}

public class Listener2
{
    public Listener2(IEventBus eventBus)
    {
        eventBus.ListenEvent("Add", ListenNormal);
        eventBus.ListenEvent("Add", ListenStatic);
        eventBus.ListenEvent("AddInter", EventKey.All, ListenInter);
        eventBus.ListenEvent("Mul", ListenTyped);
    }
    public EventResult ListenNormal(IEventParam param)
    {
        Console.WriteLine($"Listen2Normal {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return EventStatus.Continue;
    }

    public static EventResult ListenStatic(IEventParam param)
    {
        Console.WriteLine($"Listen2Static {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return EventStatus.Continue;
    }

    public EventResult ListenInter(IEventParam param)
    {
        Console.WriteLine($"I Can not ...");
        return EventStatus.Continue;
    }

    public EventResult<int> ListenTyped(IEventParam param)
    {
        Console.WriteLine($"Listen2Typed {param.Get<int>("1")}*{param.Get<int>("2")}");
        return param.Get<int>("1") * param.Get<int>("2");
    }
}

public class Provider(IEventBus eventBus)
{
    private readonly IEvent<int> _add = eventBus.AddEvent<int>("Add"), _mul = eventBus.AddEvent<int>("Mul");
    private readonly IEvent _addInter = eventBus.AddEvent("AddInter");

    public void Invoke1()
    {
        var result = _add.Invoke(1, 2);
        Console.WriteLine($"Result: {result.Return}");

        Dictionary<string, int> dic = [];
        dic["1"] = 100;
        dic["2"] = 200;
        result = _mul.Invoke(dic);
        Console.WriteLine($"Result: {result.Return}");
    }

    public void Invoke2()
    {
        _addInter.Invoke(1, 2);
    }
}

public class AsyncListener
{
    public AsyncListener(IEventBus eventBus)
    {
        eventBus.ListenEvent("Async", ListenAsync);
    }
    public EventResult<int> ListenAsync(IEventParam param)
    {
        return (114514, EventStatus.Solve);
    }
}

public class AsyncProvider(IEventBus eventBus)
{
    private readonly IEvent<int> _async = eventBus.AddEvent<int>("Async");

    public Task<EventResult<int>> InvokeAsync()
    {
        return _async.InvokeAsync();
    }
}

public class Test
{
    static async Task Main()
    {
        Console.WriteLine($"Current Thread:{Environment.CurrentManagedThreadId}");
        IEventBus bus = new EventBus();
        var provider = new Provider(bus);
        var asyncProvider = new AsyncProvider(bus);
        var listener1 = new Listener1(bus);
        var listener2 = new Listener2(bus);
        var asyncListener = new AsyncListener(bus);
        var x = asyncProvider.InvokeAsync();
        provider.Invoke1();
        provider.Invoke2();
        Console.WriteLine(await x);
    }
}

