using Ostrean.Infrastructure.Pipeline;
using SimplePipeline.Tests.Pipeline.Filters;
using SimplePipeline.Tests.Pipeline.Middlewares;

namespace SimplePipeline.Tests.Pipeline;

internal class WithFiltersPipelineBuilder : PipelineBuilder<PipelineContext, string>
{
    public WithFiltersPipelineBuilder(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override IPipelineBuilder<PipelineContext, string> PreparePipeline() =>
        this.Use<Middleware1>()
            .Use<Middleware2>()
            .Use<Middleware3>()
            .UseFilter<Filter1>()
            .UseFilter<Filter2>();

    internal const string ExpectedResult = "(<1((<2(((<3>)>))>)))";
}