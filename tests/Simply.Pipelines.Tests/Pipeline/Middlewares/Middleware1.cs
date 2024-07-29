namespace Simply.Pipelines.Tests.Pipeline.Middlewares;

internal class Middleware1 : IPipelineMiddleware<PipelineContext>
{
    public Task Execute(PipelineContext context, Func<PipelineContext, Task> next, CancellationToken cancellationToken)
    {
        context.Result += "1";
        return next(context);
    }
}