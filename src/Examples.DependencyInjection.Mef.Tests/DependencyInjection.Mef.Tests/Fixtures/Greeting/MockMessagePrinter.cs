using System.Diagnostics;

namespace Examples.DependencyInjection.Mef.Tests.Fixtures.Greeting;

public class MockMessagePrinter : IMessagePrinter
{
    public ICollection<string> Messages { get; } = [];

    public void Print(string message)
    {
        Messages.Add(message);
        Debug.WriteLine(message);
    }
}