using KBE.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KBE.Shared.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddConfiguration<T>(this IServiceCollection serviceCollection) where T : class, IValidatable
		{
			serviceCollection.AddTransient(provider => provider
				.GetRequiredService<IConfiguration>()
				.GetValidatedConfiguration<T>());
		}
	}
}