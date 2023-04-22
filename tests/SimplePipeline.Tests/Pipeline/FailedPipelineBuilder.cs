using Ostrean.Infrastructure.Pipeline;
using SimplePipeline.Tests.Pipeline.Filters;
using SimplePipeline.Tests.Pipeline.Middlewares;

namespace SimplePipeline.Tests.Pipeline;

internal class FailedPipelineBuilder : PipelineBuilder<PipelineContext, string>
{
    public FailedPipelineBuilder(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override IPipelineBuilder<PipelineContext, string> PreparePipeline() =>
        this.Use<FailedMiddleware>().UseFilter<Filter1>().UseFilter<Filter2>();
}