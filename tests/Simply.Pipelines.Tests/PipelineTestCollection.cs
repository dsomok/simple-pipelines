using Xunit;

namespace Simply.Pipelines.Tests;

[CollectionDefinition(NAME)]
public class PipelineTestCollection : ICollectionFixture<PipelineTestFixture>
{
    public const string NAME = "Pipeline Test Collection";
}