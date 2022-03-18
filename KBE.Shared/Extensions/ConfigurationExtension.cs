using KBE.Shared.Exceptions;
using KBE.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace KBE.Shared.Extensions
{
	public static class ConfigurationExtension
	{
		public static T? GetUnValidatedConfiguration<T>(this IConfiguration configuration)
		{
			return configuration
				.GetSection(typeof(T).Name)
				.Get<T>();
		}

		public static T GetValidatedConfiguration<T>(this IConfiguration configuration) where T : IValidatable
		{
			T? configurationObject = configuration.GetUnValidatedConfiguration<T>();

			if (configurationObject == null)
				throw new ConfigurationMissingException<T>();

			configurationObject.ValidateAndThrow();

			return configurationObject;
		}
	}
}