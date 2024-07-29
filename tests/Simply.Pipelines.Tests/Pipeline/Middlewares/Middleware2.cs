namespace Simply.Pipelines.Tests.Pipeline.Middlewares;

internal class Middleware2 : IPipelineMiddleware<PipelineContext>
{
    public Task Execute(PipelineContext context, Func<PipelineContext, Task> next, CancellationToken cancellationToken)
    {
        context.Result += "2";
        return next(context);
    }
}