using Ostrean.Infrastructure.Pipeline;

namespace SimplePipeline.Tests.Pipeline.Middlewares;

internal class Middleware3 : IPipelineMiddleware<PipelineContext>
{
    public Task Execute(PipelineContext context, Func<PipelineContext, Task> next, CancellationToken cancellationToken)
    {
        context.Result += "3";
        return next(context);
    }
}