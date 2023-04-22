namespace Ostrean.Infrastructure.Pipeline;

public interface IPipelineBuilder<TContext, TResult>
    where TContext : IPipelineContext<TResult>
{
    IPipelineBuilder<TContext, TResult> Use(Type middlewareType);

    IPipelineBuilder<TContext, TResult> Use<TMiddleware>()
        where TMiddleware : class, IPipelineMiddleware<TContext>;

    IPipelineBuilder<TContext, TResult> UseFilter<TFilter>()
        where TFilter : class, IPipelineFilter;

    Pipeline<TContext, TResult> Build();
}