namespace Ostrean.Infrastructure.Pipeline;

public interface IPipelineContext<TResult>
{
    Task<bool> IsValid();
    Task<TResult> GetResult();
}