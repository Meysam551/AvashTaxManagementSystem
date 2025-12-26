
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ATMS.ApplicationService;

public static class ApplicationConfigurationServiceInstaller
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly());
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationConfigurationServiceInstaller).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationConfigurationServiceInstaller).Assembly));

        // ✅ AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        return services;
    }
}
