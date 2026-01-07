
using System.Reflection;
using ATMS.Shared.Dtos;
using ErrorOr;
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

        services.AddTransient<IRequestHandler<CreateDocumentCoverCommand, Guid>,
            CreateDocumentCoverCommandHandler>();

        services.AddTransient<IRequestHandler<GetDocumentCoversQuery, ErrorOr<IReadOnlyList<DocumentCoverDto>>>,
            GetDocumentCoversQueryHandler>();

        return services;
    }
}
