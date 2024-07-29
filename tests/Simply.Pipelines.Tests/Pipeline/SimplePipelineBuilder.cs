using Simply.Pipelines.Tests.Pipeline.Middlewares;

namespace Simply.Pipelines.Tests.Pipeline;

internal class SimplePipelineBuilder(
    IServiceProvider serviceProvider
) : PipelineBuilder<PipelineContext, string>(serviceProvider)
{
    protected override IPipelineBuilder<PipelineContext, string> PreparePipeline() =>
        this.Use<Middleware1>().Use<Middleware2>().Use<Middleware3>();

    internal const string EXPECTED_RESULT = "123";
}