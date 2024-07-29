namespace Simply.Pipelines.Tests.Pipeline;

internal class PipelineContext : IPipelineContext<string>
{
    public string Result { get; set; } = string.Empty;

    public Task<bool> IsValid() => Task.FromResult(true);
    public Task<string> GetResult() => Task.FromResult(Result);
}