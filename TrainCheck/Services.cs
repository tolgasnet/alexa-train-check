using System;
using System.IO;
using Amazon.XRay.Recorder.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainCheck.Alexa;
using TrainCheck.Config;
using TrainCheck.Infrastructure;
using TrainCheck.TransportApi;

namespace TrainCheck
{
    public class Services
    {
        public static void Bind(IServiceCollection services, Action<IServiceCollection> additionalBindings)
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

            services.AddHttpClient<StandardHttpClient>();

            AWSXRayRecorder.InitializeInstance(configuration);

            additionalBindings?.Invoke(services);
        }
    }
}