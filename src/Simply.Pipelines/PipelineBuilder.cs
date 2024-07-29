using Microsoft.Extensions.DependencyInjection;

namespace Simply.Pipelines;

public abstract class PipelineBuilder<TContext, TResult>(
    IServiceProvider serviceProvider
) : IPipelineBuilder<TContext, TResult>
    where TContext : IPipelineContext<TResult>
{
    private LinkedList<IPipelineMiddleware<TContext>> _middlewares = [];
    private LinkedList<IPipelineFilter> _filters = [];


    protected abstract IPipelineBuilder<TContext, TResult> PreparePipeline();


    public IPipelineBuilder<TContext, TResult> Use<TMiddleware>() 
        where TMiddleware : class, IPipelineMiddleware<TContext>
    {
        var middleware = serviceProvider.GetService(typeof(TMiddleware)) as TMiddleware;
        _middlewares.AddLast(middleware);

        return this;
    }

    public IPipelineBuilder<TContext, TResult> Use(Type middlewareType)
    {
        if (!typeof(IPipelineMiddleware<TContext>).IsAssignableFrom(middlewareType))
        {
            throw new ArgumentException($"Type {middlewareType} cannot be used as a middleware because it must implement {typeof(IPipelineMiddleware<TContext>).Name} interface");
        }

        var middleware = serviceProvider.GetService(middlewareType) as IPipelineMiddleware<TContext>;
        _middlewares.AddLast(middleware);

        return this;
    }

    public IPipelineBuilder<TContext, TResult> UseFilter<TFilter>() 
        where TFilter : class, IPipelineFilter
    {
        var filter = (IPipelineFilter)serviceProvider.GetRequiredService<TFilter>();
        _filters.AddLast(filter);

        return this;
    }


    public Pipeline<TContext, TResult> Build()
    {
        _middlewares = [];
        _filters = [];

        PreparePipeline();

        return new Pipeline<TContext, TResult>(_middlewares, _filters);
    }
}