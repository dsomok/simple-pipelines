namespace Simply.Pipelines;

public interface IPipelineFilter
{
    Task OnMiddlewareExecuting<TContext>(IPipelineMiddleware<TContext> middleware, TContext context);

    Task OnMiddlewareExecuted<TContext>(IPipelineMiddleware<TContext> middleware, TContext context);

    Task OnMiddlewareError<TContext>(IPipelineMiddleware<TContext> middleware, TContext context, Exception ex);
}