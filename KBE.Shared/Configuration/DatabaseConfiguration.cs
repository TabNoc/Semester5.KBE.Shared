using FluentValidation;
using KBE.Shared.Interfaces;

namespace KBE.Shared.Configuration
{
	public class DatabaseConfiguration : IValidatable
	{
		public string Host { get; set; }
		public string Database { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public int MaxPoolSize { get; set; }
		public int Port { get; set; }

		public void ValidateAndThrow()
		{
			new DatabaseConfigurationValidator()
				.ValidateAndThrow(this);
		}

		private class DatabaseConfigurationValidator : AbstractValidator<DatabaseConfiguration>
		{
			public DatabaseConfigurationValidator()
			{
				RuleFor(configuration => configuration.Host)
					.NotEmpty();
				RuleFor(configuration => configuration.Database)
					.NotEmpty();
				RuleFor(configuration => configuration.User)
					.NotEmpty();
				RuleFor(configuration => configuration.Password)
					.NotEmpty();
				RuleFor(configuration => configuration.MaxPoolSize)
					.GreaterThan(0);
				RuleFor(configuration => configuration.Port)
					.GreaterThan(0);
			}
		}
	}
}