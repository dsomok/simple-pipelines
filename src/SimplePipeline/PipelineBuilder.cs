using Microsoft.Extensions.DependencyInjection;

namespace Ostrean.Infrastructure.Pipeline;

public abstract class PipelineBuilder<TContext, TResult> : IPipelineBuilder<TContext, TResult>
    where TContext : IPipelineContext<TResult>
{
    private LinkedList<IPipelineMiddleware<TContext>> _middlewares;
    private LinkedList<IPipelineFilter> _filters;

    private readonly IServiceProvider _serviceProvider;


    protected PipelineBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _middlewares = new LinkedList<IPipelineMiddleware<TContext>>();
        _filters = new LinkedList<IPipelineFilter>();
    }

    
    protected abstract IPipelineBuilder<TContext, TResult> PreparePipeline();


    public IPipelineBuilder<TContext, TResult> Use<TMiddleware>() 
        where TMiddleware : class, IPipelineMiddleware<TContext>
    {
        var middleware = _serviceProvider.GetService(typeof(TMiddleware)) as TMiddleware;
        _middlewares.AddLast(middleware);

        return this;
    }

    public IPipelineBuilder<TContext, TResult> Use(Type middlewareType)
    {
        if (!typeof(IPipelineMiddleware<TContext>).IsAssignableFrom(middlewareType))
        {
            throw new ArgumentException($"Type {middlewareType} cannot be used as a middleware because it must implement {typeof(IPipelineMiddleware<TContext>).Name} interface");
        }

        var middleware = _serviceProvider.GetService(middlewareType) as IPipelineMiddleware<TContext>;
        _middlewares.AddLast(middleware);

        return this;
    }

    public IPipelineBuilder<TContext, TResult> UseFilter<TFilter>() 
        where TFilter : class, IPipelineFilter
    {
        var filter = (IPipelineFilter)_serviceProvider.GetRequiredService<TFilter>();
        _filters.AddLast(filter);

        return this;
    }


    public Pipeline<TContext, TResult> Build()
    {
        _middlewares = new LinkedList<IPipelineMiddleware<TContext>>();
        _filters = new LinkedList<IPipelineFilter>();

        PreparePipeline();

        return new Pipeline<TContext, TResult>(_serviceProvider, _middlewares, _filters);
    }
}