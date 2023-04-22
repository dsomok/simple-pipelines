using Xunit;

namespace SimplePipeline.Tests;

[CollectionDefinition(NAME)]
internal class PipelineTestCollection : ICollectionFixture<PipelineTestFixture>
{
    public const string NAME = "Pipeline Test Collection";
}