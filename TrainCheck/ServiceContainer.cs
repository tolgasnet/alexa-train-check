using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.XRay.Recorder.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainCheck.Alexa;
using TrainCheck.Config;
using TrainCheck.TransportApi;

namespace TrainCheck
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
            return _serviceProvider.GetService<T>();
        }

        private static ServiceProvider BuildProvider(Action<IServiceCollection> additionalBindings)
        {
            var services = new ServiceCollection();
            ConfigureServices(services, additionalBindings);

            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services,
            Action<IServiceCollection> additionalBindings)
        {
            services.AddScoped<IAlexaSpeech, AlexaSpeech>();
            services.AddScoped<ITransportApi, TransportApi.TransportApi>();

            services.AddTransient<AlexaSkill>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<TrainStationSettings>(configuration, "TrainStations");
            services.AddSingleton<TransportApiSettings>(configuration, "TransportApi",
                t => t.AppKey = configuration.GetValue<string>("Env_TransportApi_AppKey"));

            AWSXRayRecorder.InitializeInstance(configuration);

            additionalBindings?.Invoke(services);
        }
    }
}