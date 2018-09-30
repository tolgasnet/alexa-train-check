using System;
using System.IO;
using System.Net.Http;
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
            services.AddTransient<IAlexaSpeech, AlexaSpeech>();
            services.AddTransient<ITransportApi, TransportApi.TransportApi>();
            services.AddTransient<IStandardHttpClient, StandardHttpClient>();

            services.AddTransient<HttpClient>();
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