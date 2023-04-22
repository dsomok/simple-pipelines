namespace Ostrean.Infrastructure.Pipeline;

public interface IPipelineMiddleware<TContext>
{
    Task Execute(TContext context, Func<TContext, Task> next, CancellationToken cancellationToken);
}