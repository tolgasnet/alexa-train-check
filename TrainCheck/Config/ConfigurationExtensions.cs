using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrainCheck.Config
{
	public static class ConfigurationExtensions
	{
		public static T AddSingleton<T>(
			this IServiceCollection services, 
			IConfiguration configuration,
			string name,
			Action<T> additionalBindings = null) where T : AppSetting, new()
		{
			var settings = new T();

			configuration.Bind(name, settings);

			if (additionalBindings != null)
			{
				additionalBindings(settings);
			}

			settings.ValidateForNulls();

			services.AddSingleton(settings);

			return settings;
		}
	}
}