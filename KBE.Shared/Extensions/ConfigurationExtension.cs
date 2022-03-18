using KBE.Shared.Configuration;
using KBE.Shared.Exceptions;
using KBE.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace KBE.Shared.Extensions
{
	public static class ConfigurationExtension
	{
		public static NpgsqlConnectionStringBuilder GetDatabaseConnectionBuilder(this IConfiguration configuration)
		{
			DatabaseConfiguration databaseConfiguration = configuration.GetValidatedConfiguration<DatabaseConfiguration>();

			return new NpgsqlConnectionStringBuilder
			{
				Host = databaseConfiguration.Host,
				Database = databaseConfiguration.Database,
				Password = databaseConfiguration.Password,
				Username = databaseConfiguration.User,
				MaxPoolSize = databaseConfiguration.MaxPoolSize,
				Pooling = true,
				Port = databaseConfiguration.Port
			};
		}

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