namespace Simply.Pipelines;

public class Pipeline<TContext, TResult> where TContext : IPipelineContext<TResult>
{
    private readonly LinkedList<IPipelineMiddleware<TContext>> _middlewares;
    private readonly LinkedList<IPipelineFilter> _filters;

    internal Pipeline(
        LinkedList<IPipelineMiddleware<TContext>> middlewares, 
        LinkedList<IPipelineFilter> filters
    )
    {
        if (middlewares == null || middlewares.Count == 0)
        {
            throw new ArgumentException("Pipeline should contain steps", nameof(middlewares));
        }

        _middlewares = middlewares;
        _filters = filters;
    }

    public async Task<TResult> Execute(TContext context, CancellationToken cancellationToken = default)
    {
        var currentStep = _middlewares.Last;
        Func<TContext, Task> previous = ctx => Task.CompletedTask;
        Func<TContext, Task> pipelineAction;
        
        do
        {
            var middleware = currentStep!.Value;
            var executeAction = new Func<TContext, Func<TContext, Task>, Task>(
                (ctx, next) => ExecuteMiddleware(middleware, ctx, next, cancellationToken)
            );

            var next = new Func<TContext, Task>(previous);

            pipelineAction = ctx => executeAction(ctx, next);

            previous = pipelineAction;
            currentStep = currentStep.Previous;
        } while (currentStep != null);

        await pipelineAction(context);

        if (!await context.IsValid())
        {
            throw new Exception("Pipeline context resulted in failed state");
        }

        return await context.GetResult();
    }

    private async Task ExecuteMiddleware(
        IPipelineMiddleware<TContext> middleware,
        TContext context,
        Func<TContext, Task> next,
        CancellationToken cancellationToken
    )
    {
        for (var filter = _filters.First; filter != null; filter = filter.Next)
        {
            await filter.Value.OnMiddlewareExecuting(middleware, context);
        }

        try
        {
            await middleware.Execute(context, next, cancellationToken);
        }
        catch (Exception ex)
        {
            for (var filter = _filters.Last; filter != null; filter = filter.Previous)
            {
                await filter.Value.OnMiddlewareError(middleware, context, ex);
            }

            throw;
        }
        finally
        {
            for (var filter = _filters.Last; filter != null; filter = filter.Previous)
            {
                await filter.Value.OnMiddlewareExecuted(middleware, context);
            }
        }
    }
}