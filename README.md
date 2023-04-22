| This documentation was generated by the GPT-4 model |
|-----------------------------------------------------|

<br/>

# Pipeline Library

This library provides a simple and efficient way to create and execute middleware pipelines in .NET. It supports middleware, filters, and custom context types.

## Getting Started
This library consists of a few key interfaces and classes that work together to create a pipeline of middleware with optional filters.

## Interfaces

### IPipelineBuilder

The `IPipelineBuilder` interface defines the methods for constructing a pipeline.

```csharp
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
```

### IPipelineContext

The `IPipelineContext` interface defines the methods for managing the result of the pipeline.

```csharp
public interface IPipelineContext<TResult>
{
    Task<bool> IsValid();
    Task<TResult> GetResult();
}
```

### IPipelineFilter

The `IPipelineFilter` interface defines the methods for handling events during the middleware execution.

```csharp
public interface IPipelineFilter
{
    Task OnMiddlewareExecuting<TContext>(IPipelineMiddleware<TContext> middleware, TContext context);

    Task OnMiddlewareExecuted<TContext>(IPipelineMiddleware<TContext> middleware, TContext context);

    Task OnMiddlewareError<TContext>(IPipelineMiddleware<TContext> middleware, TContext context, Exception ex);
}
```

### IPipelineMiddleware

The `IPipelineMiddleware` interface defines the methods for processing the pipeline context.

```csharp
public interface IPipelineMiddleware<TContext>
{
    Task Execute(TContext context, Func<TContext, Task> next, CancellationToken cancellationToken);
}
```

## Pipeline<TContext, TResult>

The `Pipeline<TContext, TResult>` class represents a sequence of operations (middlewares) to be executed in a specific order. The pipeline processes a given context and produces a result of type TResult. The pipeline also works with optional filters that can be used to add cross-cutting concerns, such as logging or error handling.

As a user, you will typically use the `Execute` method to run the pipeline with a specific context. The pipeline takes care of executing the middlewares in order, handling any exceptions, and interacting with the filters as needed. You don't need to worry about the internals of how the pipeline manages middlewares and filters, as it is abstracted away by the class.

## PipelineBuilder<TContext, TResult>

The `PipelineBuilder<TContext, TResult>` class provides a fluent interface for creating and configuring a pipeline. As a user, you will typically extend this abstract class to create your custom pipeline builder. The custom pipeline builder is responsible for defining the middlewares and filters used in the pipeline.

You will use the `Use` and `UseFilter` methods in your custom pipeline builder to add middlewares and filters to the pipeline. The `Build` method is then used to construct a `Pipeline<TContext, TResult>` instance, which can be executed with the desired context.

Here is an example of how you might use a custom pipeline builder:

1. Create a custom pipeline builder by extending the `PipelineBuilder<TContext, TResult>` class.
2. Implement the `PreparePipeline` method to configure the middlewares and filters you want to use.
3. Instantiate your custom pipeline builder and use the `Build` method to create a `Pipeline<TContext, TResult>` instance.
4. Execute the pipeline with a specific context using the `Execute` method.

This approach provides a clean, maintainable way to create and manage complex pipelines, making it easy to add, remove, or modify middlewares and filters as needed.

## Example Usage

```csharp
public class CustomPipelineBuilder : PipelineBuilder<CustomContext, CustomResult>
{
    public CustomPipelineBuilder(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override IPipelineBuilder<CustomContext, CustomResult> PreparePipeline()
    {
        return this
            .Use<CustomMiddleware1>()
            .Use<CustomMiddleware2>()
            .UseFilter<CustomFilter>();
    }
}

public class CustomContext : IPipelineContext<CustomResult>
{
    // Implementation
}

public class CustomResult
{
    // Custom result properties and methods
}

public class CustomMiddleware1 : IPipelineMiddleware<CustomContext>
{
    public async Task Execute(CustomContext context, Func<CustomContext, Task> next, CancellationToken cancellationToken)
    {
        // Middleware logic before next middleware
        await next(context);
        // Middleware logic after next middleware
    }
}

public class CustomMiddleware2 : IPipelineMiddleware<CustomContext>
{
    public async Task Execute(CustomContext context, Func<CustomContext, Task> next, CancellationToken cancellationToken)
    {
        // Middleware logic before next middleware
        await next(context);
        // Middleware logic after next middleware
    }
}

public class CustomFilter : IPipelineFilter
{
    public async Task OnMiddlewareExecuting<TContext>(IPipelineMiddleware<TContext> middleware, TContext context)
    {
    // Logic to run before a middleware is executed
    }

    public async Task OnMiddlewareExecuted<TContext>(IPipelineMiddleware<TContext> middleware, TContext context)
    {
        // Logic to run after a middleware has executed
    }

    public async Task OnMiddlewareError<TContext>(IPipelineMiddleware<TContext> middleware, TContext context, Exception ex)
    {
        // Logic to run if an exception occurs during the execution of a middleware
    }
}
```

```csharp
// Usage in the application
var serviceProvider = new ServiceCollection().BuildServiceProvider();
var pipelineBuilder = new CustomPipelineBuilder(serviceProvider);
var pipeline = pipelineBuilder.Build();

var context = new CustomContext();
var result = await pipeline.Execute(context);

// Process the result
```

In this example, we define a custom pipeline builder, context, result, middlewares, and a filter. The custom pipeline builder is responsible for constructing the pipeline with the specified middlewares and filter. The custom context and result are specific to your application and should implement the `IPipelineContext<TResult>` interface.

The custom middlewares define the logic for processing the context, and the custom filter defines the logic for handling events during middleware execution. Finally, the pipeline is built and executed with the custom context, and the result is processed.

## Dependency Injection

In this example, we will demonstrate how to integrate the pipeline library with dependency injection using the Microsoft.Extensions.DependencyInjection package.

First, update your `Startup` class or any other place where you configure your services to use the provided extension methods:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddPipelineMiddlewares(typeof(CustomMiddleware1).Assembly);
    services.AddPipelineMiddlewareFilters(typeof(CustomFilter).Assembly);
    services.AddSingleton<CustomPipelineBuilder>();
}
```

Now you can inject the custom pipeline builder and use it in your application as shown in the previous example. For example, in an ASP.NET Core controller:

```csharp
public class HomeController : Controller
{
    private readonly CustomPipelineBuilder _pipelineBuilder;

    public HomeController(CustomPipelineBuilder pipelineBuilder)
    {
        _pipelineBuilder = pipelineBuilder;
    }

    public async Task<IActionResult> Index()
    {
        var context = new CustomContext();
        var pipeline = _pipelineBuilder.Build();
        var result = await pipeline.Execute(context);

        // Process the result and return the appropriate view or response

        return View(result);
    }
}
```

In this example, we use the `AddPipelineMiddlewares` and `AddPipelineMiddlewareFilters` extension methods to register all middlewares and filters within the specified assemblies. This simplifies the registration process and avoids the need to register each middleware and filter individually.