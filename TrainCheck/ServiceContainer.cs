using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainCheck.Alexa;
using TrainCheck.Config;
using TrainCheck.TransportApi;

namespace TrainCheck
{
    public static class ServiceContainer
    {
        private static ServiceProvider _serviceProvider;

        public static T GetOrCreate<T>()
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = BuildProvider();
            }
            return _serviceProvider.GetService<T>();
        }

        private static ServiceProvider BuildProvider()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
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
        }
    }
}