using DisposableExample;

namespace DisposableDemoProject;

public static class Program
{
    private static void Main()
    {
        var demo = new Demo();
        
        // 1.
        demo.StartOpenConnections();
        
        // 2.
        demo.RunConnectionAndMemoryDemo();
    }
}