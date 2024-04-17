using EventBusNet8;
using EventBusNet8.Abstractions;
using EventBusNet8.Enums;

public class Listener1
{
    public Listener1(IEventBus eventBus)
    {
        eventBus.ListenEvent("Add", null, new Handler(ListenNormal));
        eventBus.ListenEvent("Add", IEventBus.SomeOneButNullPrefer, new Handler(ListenStatic));
        eventBus.ListenEvent("AddInter", IEventBus.All, new Handler(ListenInter));
        eventBus.ListenEvent("Add", IEventBus.SomeOne, new Handler<int>(ListenTyped));
        eventBus.ListenEvent("Add", null, new Handler<int>(ListenPhase), EventPhase.Pre);
    }
    public IResult ListenNormal(IEventParam param)
    {
        Console.WriteLine($"ListenNormal {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return new Result(EventStatus.Continued);
    }

    public static IResult ListenStatic(IEventParam param)
    {
        Console.WriteLine($"ListenStatic {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return new Result(EventStatus.Continued);
    }

    public IResult ListenInter(IEventParam param)
    {
        Console.WriteLine($"ListenInter {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return new Result(EventStatus.Interrupted);
    }

    public IResult<int> ListenTyped(IEventParam param)
    {
        Console.WriteLine($"ListenTyped {param.Get<int>(0)}+{param.Get<int>(1)}");
        return new Result<int>(param.Get<int>(0) + param.Get<int>(1), EventStatus.Interrupted);
    }

    public IResult<int> ListenPhase(IEventParam param)
    {
        Console.WriteLine($"ListenPhase {param.Get<int>(0)}+{param.Get<int>(1)}");
        param.Set(0, 101);
        param.Set(1, 202);
        return new Result<int>(param.Get<int>(0) + param.Get<int>(1), EventStatus.Continued);
    }

}

public class Listener2
{
    public Listener2(IEventBus eventBus)
    {
        eventBus.ListenEvent("Add", null, new Handler(ListenNormal));
        eventBus.ListenEvent("Add", null, new Handler(ListenStatic));
        eventBus.ListenEvent("AddInter", IEventBus.All, new Handler(ListenInter));
        eventBus.ListenEvent("Mul", null, new Handler<int>(ListenTyped));
    }
    public IResult ListenNormal(IEventParam param)
    {
        Console.WriteLine($"Listen2Normal {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return new Result(EventStatus.Continued);
    }

    public static IResult ListenStatic(IEventParam param)
    {
        Console.WriteLine($"Listen2Static {param.Get<int>(0)}+{param.Get<int>(1)}={param.Get<int>(0) + param.Get<int>(1)}");
        return new Result(EventStatus.Continued);
    }

    public IResult ListenInter(IEventParam param)
    {
        Console.WriteLine($"I Can not ...");
        return new Result(EventStatus.Continued);
    }

    public IResult<int> ListenTyped(IEventParam param)
    {
        Console.WriteLine($"Listen2Typed {param.Get<int>("1")}*{param.Get<int>("2")}");
        return new Result<int>(param.Get<int>("1") * param.Get<int>("2"), EventStatus.Continued);
    }
}

public class Provider(IEventBus eventBus)
{
    private readonly IEvent<int> _add = eventBus.AddEvent<int>("Add"), _mul = eventBus.AddEvent<int>("Mul");
    private readonly IEvent _addInter = eventBus.AddEvent("AddInter");

    public void Invoke1()
    {
        var result = _add.Invoke(new EventParam([1, 2]));
        Console.WriteLine($"Result: {result.Return}");

        Dictionary<string, int> dic = [];
        dic["1"] = 100;
        dic["2"] = 200;
        result = _mul.Invoke(new EventParam(dic));
        Console.WriteLine($"Result: {result.Return}");
    }

    public void Invoke2()
    {
        _addInter.Invoke(new EventParam([1, 2]));
    }
}

public class Test
{
    static void Main()
    {
        IEventBus bus = new EventBus();
        var provider = new Provider(bus);
        var listener1 = new Listener1(bus);
        var listener2 = new Listener2(bus);
        provider.Invoke1();
        provider.Invoke2();
    }
}

