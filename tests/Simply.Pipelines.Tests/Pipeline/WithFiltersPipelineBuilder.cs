using Simply.Pipelines.Tests.Pipeline.Filters;
using Simply.Pipelines.Tests.Pipeline.Middlewares;

namespace Simply.Pipelines.Tests.Pipeline;

internal class WithFiltersPipelineBuilder(
    IServiceProvider serviceProvider
) : PipelineBuilder<PipelineContext, string>(serviceProvider)
{
    protected override IPipelineBuilder<PipelineContext, string> PreparePipeline() =>
        this.Use<Middleware1>()
            .Use<Middleware2>()
            .Use<Middleware3>()
            .UseFilter<Filter1>()
            .UseFilter<Filter2>();

    internal const string EXPECTED_RESULT = "(<1((<2(((<3>)>))>)))";
}