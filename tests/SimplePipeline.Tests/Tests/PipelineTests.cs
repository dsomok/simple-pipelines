using System.Diagnostics;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimplePipeline.Tests.Pipeline;
using SimplePipeline.Tests.Pipeline.Filters;
using Xunit;
using Xunit.Abstractions;

namespace SimplePipeline.Tests.Tests;

[Collection(PipelineTestCollection.NAME)]
public class PipelineTests : IClassFixture<PipelineTestFixture>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _outputHelper;

    public PipelineTests(PipelineTestFixture testFixture, ITestOutputHelper outputHelper)
    {
        _serviceProvider = testFixture.Build();
        _outputHelper = outputHelper;
    }

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
        _outputHelper.WriteLine($"Executed {nameof(SimplePipelineBuilder)} in {timer.ElapsedTicks} ticks");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be(SimplePipelineBuilder.ExpectedResult);
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
        _outputHelper.WriteLine($"Executed {nameof(WithFiltersPipelineBuilder)} in {timer.ElapsedTicks} ticks");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be(WithFiltersPipelineBuilder.ExpectedResult);
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
        _outputHelper.WriteLine($"Executed {nameof(WithFiltersPipelineBuilder)} in {timer.ElapsedTicks} ticks");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be(WithFiltersPipelineBuilder.ExpectedResult);
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