using System.Diagnostics;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Simply.Pipelines.Tests.Pipeline;
using Simply.Pipelines.Tests.Pipeline.Filters;
using Xunit;
using Xunit.Abstractions;

namespace Simply.Pipelines.Tests.Tests;

[Collection(PipelineTestCollection.NAME)]
public class PipelineTests(
    PipelineTestFixture testFixture,
    ITestOutputHelper outputHelper
) : IClassFixture<PipelineTestFixture>
{
    private readonly IServiceProvider _serviceProvider = testFixture.Build();

    [Fact]
    public async Task SimplePipeline_ExecutedSuccessfully()
    {
        // Arrange
        var context = new PipelineContext();
        var pipelineBuilder = _serviceProvider.GetRequiredService<SimplePipelineBuilder>();
        var pipeline = pipelineBuilder.Build();

        // Act
        var timer = Stopwatch.StartNew();
        var result = await pipeline.Execute(context);
        outputHelper.WriteLine($"Executed {nameof(SimplePipelineBuilder)} in {timer.ElapsedTicks} ticks");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be(SimplePipelineBuilder.EXPECTED_RESULT);
    }

    [Fact]
    public async Task PipelineWithDecoratedMiddlewares_ExecutedSuccessfully()
    {
        // Arrange
        var context = new PipelineContext();
        var pipelineBuilder = _serviceProvider.GetRequiredService<WithFiltersPipelineBuilder>();
        var pipeline = pipelineBuilder.Build();

        // Act
        var timer = Stopwatch.StartNew();
        var result = await pipeline.Execute(context);
        outputHelper.WriteLine($"Executed {nameof(WithFiltersPipelineBuilder)} in {timer.ElapsedTicks} ticks");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be(WithFiltersPipelineBuilder.EXPECTED_RESULT);
    }

    [Fact]
    public async Task PipelineWithDecoratedMiddlewares_BuildMultipleTimes_ExecutedSuccessfully()
    {
        // Arrange
        var context = new PipelineContext();
        var pipelineBuilder = _serviceProvider.GetRequiredService<WithFiltersPipelineBuilder>();
        pipelineBuilder.Build();
        pipelineBuilder.Build();
        var pipeline = pipelineBuilder.Build();

        // Act
        var timer = Stopwatch.StartNew();
        var result = await pipeline.Execute(context);
        outputHelper.WriteLine($"Executed {nameof(WithFiltersPipelineBuilder)} in {timer.ElapsedTicks} ticks");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be(WithFiltersPipelineBuilder.EXPECTED_RESULT);
    }

    [Fact]
    public async Task FailedPipelineWithDecoratedMiddlewares_ShouldBeCaptured()
    {
        // Arrange
        var context = new PipelineContext();
        var pipelineBuilder = _serviceProvider.GetRequiredService<FailedPipelineBuilder>();
        var pipeline = pipelineBuilder.Build();

        // Act
        var pipelineExecution = async () => await pipeline.Execute(context);

        // Assert
        await pipelineExecution.Should().ThrowAsync<Exception>();
        Filter1.IsExceptionCaptured.Should().BeTrue();
        Filter2.IsExceptionCaptured.Should().BeTrue();
    }
}