using FluentValidation;
using MediatR;
using MekkysCakes.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace MekkysCakes.Application
{
    /// This follows the "composition root" pattern
    /// Each layer knows how to register its own services, and the Web project just calls this method.
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
