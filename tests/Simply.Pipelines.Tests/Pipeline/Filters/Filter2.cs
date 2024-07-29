namespace Simply.Pipelines.Tests.Pipeline.Filters;

internal class Filter2 : IPipelineFilter
{
    public static bool IsExceptionCaptured = false;

    public Task OnMiddlewareExecuting<TContext>(IPipelineMiddleware<TContext> middleware, TContext context)
    {
        if (context is PipelineContext pipelineContext)
        {
            pipelineContext.Result += "<";
        }

        return Task.CompletedTask;
    }

    public Task OnMiddlewareExecuted<TContext>(IPipelineMiddleware<TContext> middleware, TContext context)
    {
        if (context is PipelineContext pipelineContext)
        {
            pipelineContext.Result += ">";
        }

        return Task.CompletedTask;
    }

    public Task OnMiddlewareError<TContext>(IPipelineMiddleware<TContext> middleware, TContext context, Exception ex)
    {
        IsExceptionCaptured = true;
        return Task.CompletedTask;
    }
}