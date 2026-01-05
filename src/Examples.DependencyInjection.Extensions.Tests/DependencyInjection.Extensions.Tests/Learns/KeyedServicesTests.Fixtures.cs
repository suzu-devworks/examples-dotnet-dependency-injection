namespace Examples.DependencyInjection.Extensions.Tests.Learns;

public partial class KeyedServicesTests
{
    public interface IMessageWriter
    {
        void Write(string message);
    }

    public class MemoryMessageWriter : IMessageWriter
    {
        public void Write(string message)
        {
            throw new NotImplementedException();
        }
    }

    public class QueueMessageWriter : IMessageWriter
    {
        public void Write(string message)
        {
            throw new NotImplementedException();
        }
    }
}