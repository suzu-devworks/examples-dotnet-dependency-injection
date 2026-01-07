namespace Examples.DependencyInjection.Extensions.Tests.Learns;

public partial class ScopeScenariosTests
{
    public class Entry
    {
        public string Name { get; set; } = string.Empty;

        public IObjectStore? Store { get; set; }
        public IObjectProcessor? Processor { get; set; }
        public IObjectRelay? Relay { get; set; }
    }

    public interface IObjectStore
    {
        Task<Entry> GetNextAsync();
        Task<bool> MarkAsync(Entry next);
    }

    public interface IObjectProcessor
    {
        Task ProcessAsync(Entry next);
    }

    public interface IObjectRelay
    {
        Task RelayAsync(Entry next);
    }

    public class ObjectStore : IObjectStore
    {
        private int _counter = 0;

        public Task<Entry> GetNextAsync()
        {
            var entry = new Entry { Name = $"Entry: {_counter}" };
            entry.Store = this;
            return Task.FromResult(entry);
        }

        public Task<bool> MarkAsync(Entry next)
        {
            return Task.FromResult(true);
        }
    }

    public class ObjectProcessor : IObjectProcessor
    {
        public Task ProcessAsync(Entry next)
        {
            next.Processor = this;
            return Task.CompletedTask;
        }
    }

    public class ObjectRelay : IObjectRelay
    {
        public Task RelayAsync(Entry next)
        {
            next.Relay = this;
            return Task.CompletedTask;
        }
    }

}