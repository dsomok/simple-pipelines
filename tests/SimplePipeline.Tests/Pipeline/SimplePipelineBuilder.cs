using Ostrean.Infrastructure.Pipeline;
using SimplePipeline.Tests.Pipeline.Middlewares;

namespace SimplePipeline.Tests.Pipeline;

internal class SimplePipelineBuilder : PipelineBuilder<PipelineContext, string>
{
    public SimplePipelineBuilder(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override IPipelineBuilder<PipelineContext, string> PreparePipeline() =>
        this.Use<Middleware1>().Use<Middleware2>().Use<Middleware3>();

    internal const string ExpectedResult = "123";
}