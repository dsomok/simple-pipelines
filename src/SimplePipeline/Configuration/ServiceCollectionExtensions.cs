using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Ostrean.Infrastructure.Pipeline.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelineMiddlewares(this IServiceCollection services, Assembly middlewaresAssembly) 
        => services.AddTransientImplementations(typeof(IPipelineMiddleware<>), middlewaresAssembly);

    public static IServiceCollection AddPipelineMiddlewareFilters(this IServiceCollection services, Assembly filtersAssembly) 
        => services.AddTransientImplementations(typeof(IPipelineFilter), filtersAssembly);

    private static IServiceCollection AddTransientImplementations(
        this IServiceCollection services, Type interfaceType, Assembly assembly)
    {
        var implementations = (
            from assemblyType in assembly.GetTypes()
            let requestedInterfaces = (
                from typeInterface in assemblyType.GetInterfaces()
                where typeInterface == interfaceType ||
                      (typeInterface.IsGenericType &&
                       typeInterface.GetGenericTypeDefinition() == interfaceType)
                select typeInterface
            )
            where requestedInterfaces.Any()
            select assemblyType
        ).ToList();

        implementations.ForEach(type => services.AddTransient(type));

        return services;
    }
}