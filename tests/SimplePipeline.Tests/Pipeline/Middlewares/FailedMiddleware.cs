using Ostrean.Infrastructure.Pipeline;

namespace SimplePipeline.Tests.Pipeline.Middlewares;

internal class FailedMiddleware : IPipelineMiddleware<PipelineContext>
{
    public Task Execute(PipelineContext context, Func<PipelineContext, Task> next, CancellationToken cancellationToken)
    {
        throw new Exception("Failed");
    }
}