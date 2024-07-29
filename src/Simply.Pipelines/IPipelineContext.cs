namespace Simply.Pipelines;

public interface IPipelineContext<TResult>
{
    Task<bool> IsValid();
    Task<TResult> GetResult();
}