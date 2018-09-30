using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace TrainCheck.Infrastructure
{
    public static class ServiceContainer
    {
        public static bool IsXRayEnabled() => NonLocal.Contains(_environmentName);
        private static string _environmentName = "local";
        private static ServiceProvider _serviceProvider;

        private static readonly IEnumerable<string> NonLocal = new List<string>
        {
            "dev",
            "test",
            "prod"
        };

        public static T GetOrCreate<T>(Action<IServiceCollection> additionalBindings = null)
        {
            _environmentName = Environment.GetEnvironmentVariable("Env_EnvironmentName");

            if (_serviceProvider == null)
            {
                _serviceProvider = BuildProvider(additionalBindings);
            }
            return _serviceProvider.GetRequiredService<T>();
        }

        private static ServiceProvider BuildProvider(Action<IServiceCollection> additionalBindings)
        {
            var services = new ServiceCollection();

            Services.Bind(services, additionalBindings);

            return services.BuildServiceProvider();
        }
    }
}