using Microsoft.Extensions.DependencyInjection;
using Ostrean.Infrastructure.Pipeline.Configuration;
using SimplePipeline.Tests.Pipeline;

namespace SimplePipeline.Tests;

public class PipelineTestFixture : IDisposable
{
    private readonly IServiceCollection _serviceCollection;

    public PipelineTestFixture()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddTransient<SimplePipelineBuilder>()
                          .AddTransient<WithFiltersPipelineBuilder>()
                          .AddTransient<FailedPipelineBuilder>()
                          .AddPipelineMiddlewares(typeof(SimplePipelineBuilder).Assembly)
                          .AddPipelineMiddlewareFilters(typeof(SimplePipelineBuilder).Assembly);
    }

    public void RegisterDependencies(Action<IServiceCollection> servicesRegistration)
    {
        servicesRegistration?.Invoke(_serviceCollection);
    }

    public IServiceProvider Build()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });

        return serviceProvider;
    }

    public void Dispose()
    {
    }
}