using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace KBE.Shared.Extensions
{
	public static class HostBuilderExtension
	{
		public static IHostBuilder AddSecretAppSettingsJson(this IHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureAppConfiguration(builder =>
			{
				if (File.Exists("appsettings.secrets.json"))
				{
					builder.AddJsonFile("appsettings.secrets.json");
					Log.Logger.Information("Applied appsettings.secrets.json");
				}
				else
				{
					Log.Logger.Information("Could not configure appsettings.secrets.json -- File not found");
				}
			});
		}
	}
}