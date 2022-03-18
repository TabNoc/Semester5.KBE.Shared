using FluentValidation;
using KBE.Shared.Interfaces;

namespace KBE.Shared.Configuration
{
	public class LoggingConfiguration : IValidatable
	{
		public string? SeqServerUri { get; set; }
		public string? SeqServerApiKey { get; set; }

		public void ValidateAndThrow()
		{
			new LoggingConfigurationValidator()
				.ValidateAndThrow(this);
		}

		private class LoggingConfigurationValidator : AbstractValidator<LoggingConfiguration>
		{
			public LoggingConfigurationValidator()
			{
				When(configuration => configuration.SeqServerUri == null, () =>
					{
						RuleFor(configuration => configuration.SeqServerApiKey)
							.Null();
					})
					.Otherwise(() =>
					{
						RuleFor(configuration => configuration.SeqServerUri)
							.NotEmpty();
						RuleFor(configuration => configuration.SeqServerApiKey)
							.NotEmpty();
					});
			}
		}
	}
}