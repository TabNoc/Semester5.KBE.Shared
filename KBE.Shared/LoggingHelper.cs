using System;
using System.Reflection;
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
			LoggerConfiguration loggerConfiguration = configuration
				.ReadFrom.Configuration(context.Configuration) // see https://github.com/serilog/serilog-aspnetcore for syntax
				.ReadFrom.Services(services)
				.Enrich.WithProperty("Application", GetAssemblyName())
				.Enrich.FromLogContext()
				.WriteTo.Console();

			Log.Logger.Information("Current Assembly: {AssemblyName}", GetAssemblyName());

			TrySafeRegisteringSeq(context, loggerConfiguration);
		}

		public void RegisterLoggingBootstrap()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.WithProperty("Application", GetAssemblyName())
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

				Log.Information("Application terminated gracefully");

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

		private static void ApplySeqConfiguration(LoggerConfiguration loggerConfiguration, LoggingConfiguration loggingConfiguration)
		{
			Log.Logger.Information("Found LoggingConfiguration -> checking");
			loggingConfiguration.ValidateAndThrow();

			if (loggingConfiguration.SeqServerUri != null)
			{
				Log.Logger.Information("Found valid LoggingConfiguration -> registering Seq as Sink");
				loggerConfiguration
					.WriteTo.Seq(loggingConfiguration.SeqServerUri, apiKey: loggingConfiguration.SeqServerApiKey);
			}
			else
			{
				Log.Logger.Warning("Found valid LoggingConfiguration, but no Server was configured");
			}
		}

		private static string GetAssemblyName()
		{
			return Assembly.GetEntryAssembly()
				?
				.GetName()
				.Name ?? "Unable to obtain AssemblyName";
		}

		private static void TrySafeRegisteringSeq(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
		{
			LoggingConfiguration? loggingConfiguration = context.Configuration.GetUnValidatedConfiguration<LoggingConfiguration>();

			if (loggingConfiguration != null)
				ApplySeqConfiguration(loggerConfiguration, loggingConfiguration);
			else
				Log.Logger.Warning("Found no LoggingConfiguration -> will not register Seq Sink");
		}
	}
}