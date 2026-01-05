using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Examples.DependencyInjection.Extensions.Tests.Learns;

public partial class MultipleConstructorDiscoveryRulesTests
{
    public class UnresolvableParameterService
    {
        public UnresolvableParameterService()
        {
        }

        public UnresolvableParameterService(ILogger<UnresolvableParameterService> logger)
        {
            Logger = logger;
        }

        public UnresolvableParameterService(FooService fooService, BarService barService)
        {
            Foo = fooService;
            Bar = barService;
        }

        public ILogger<UnresolvableParameterService>? Logger { get; }

        public FooService? Foo { get; }

        public BarService? Bar { get; }
    }

    public class FooService
    {

    }

    public class BarService
    {

    }

    public class AmbiguousParametersService
    {
        public AmbiguousParametersService()
        {
        }

        public AmbiguousParametersService(ILogger<AmbiguousParametersService> logger)
        {
            Logger = logger;
        }

        [ActivatorUtilitiesConstructor]
        public AmbiguousParametersService(IOptions<ExampleOptions> options)
        {
            Options = options.Value;
        }

        public ILogger<AmbiguousParametersService>? Logger { get; }

        public ExampleOptions? Options { get; }
    }

    public class DisambiguatesParametersService
    {
        public DisambiguatesParametersService(
            ILogger<DisambiguatesParametersService> logger,
            IOptions<ExampleOptions> options)
        {
            Logger = logger;
            Options = options.Value;
        }

        public ILogger<DisambiguatesParametersService>? Logger { get; }

        public ExampleOptions? Options { get; }
    }

    public class ExampleOptions
    {

    }

}