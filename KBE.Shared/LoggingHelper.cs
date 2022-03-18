using System;
using KBE.Shared.Configuration;
using KBE.Shared.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace KBE.Shared
{
	public class LoggingHelper
	{
		public void RegisterFullLogger(HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
		{
			LoggingConfiguration? loggingConfiguration = context.Configuration.GetUnValidatedConfiguration<LoggingConfiguration>();
			if (loggingConfiguration != null)
			{
				loggingConfiguration.ValidateAndThrow();
				if (loggingConfiguration.SeqServerUri != null)
					configuration
						.ReadFrom.Configuration(context.Configuration) // see https://github.com/serilog/serilog-aspnetcore for syntax
						.ReadFrom.Services(services)
						.Enrich.FromLogContext()
						.WriteTo.Console()
						.WriteTo.Seq(loggingConfiguration.SeqServerUri, apiKey: loggingConfiguration.SeqServerApiKey);
			}
		}

		public void RegisterLoggingBootstrap()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateBootstrapLogger();
		}

		public int WrapApplication(Action action)
		{
			try
			{
				Log.Information("Starting web host");

				action();

				return 0;
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Host terminated unexpectedly");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}
	}
}