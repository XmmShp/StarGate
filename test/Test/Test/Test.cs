namespace Test;


public class Test
{
    public static int Add(int x, int y) => x + y;
    public static Delegate Listen(Func<int, int, int> func) => func;
    public static void Main()
    {
        var d = Listen(Add);
        Console.WriteLine(d.DynamicInvoke(1, 2));
    }
}

