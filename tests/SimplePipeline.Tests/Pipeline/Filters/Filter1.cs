using Ostrean.Infrastructure.Pipeline;

namespace SimplePipeline.Tests.Pipeline.Filters;

internal class Filter1 : IPipelineFilter
{
    private string _preSymbol = "";
    private string _postSymbol = "";

    public static bool IsExceptionCaptured = false;

    public Task OnMiddlewareExecuting<TContext>(IPipelineMiddleware<TContext> middleware, TContext context)
    {
        if (context is PipelineContext pipelineContext)
        {
            // Verifies that there is a same instance of filter for each middleware
            _preSymbol += "(";
            pipelineContext.Result += _preSymbol;
        }

        return Task.CompletedTask;
    }

    public Task OnMiddlewareExecuted<TContext>(IPipelineMiddleware<TContext> middleware, TContext context)
    {
        if (context is PipelineContext pipelineContext)
        {
            // Verifies that there is a same instance of filter for each middleware
            _postSymbol += ")";
            pipelineContext.Result += _postSymbol;
        }

        return Task.CompletedTask;
    }

    public Task OnMiddlewareError<TContext>(IPipelineMiddleware<TContext> middleware, TContext context, Exception ex)
    {
        IsExceptionCaptured = true;
        return Task.CompletedTask;
    }
}