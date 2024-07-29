using Simply.Pipelines.Tests.Pipeline.Filters;
using Simply.Pipelines.Tests.Pipeline.Middlewares;

namespace Simply.Pipelines.Tests.Pipeline;

internal class FailedPipelineBuilder(
    IServiceProvider serviceProvider
) : PipelineBuilder<PipelineContext, string>(serviceProvider)
{
    protected override IPipelineBuilder<PipelineContext, string> PreparePipeline() =>
        this.Use<FailedMiddleware>().UseFilter<Filter1>().UseFilter<Filter2>();
}