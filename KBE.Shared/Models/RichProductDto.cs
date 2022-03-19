using System;
using System.Collections.Generic;
using FluentValidation;
using KBE.Shared.Interfaces;

namespace KBE.Shared.Models
{
	public class RichProductDto : ThinProductDto, IValidatable
	{
		public int Amount { get; set; }
		public List<string> Comments { get; set; } = null!;
		public DateTimeOffset DeliveryDate { get; set; }
		public TimeSpan DeliveryTime { get; set; }
		public string Location { get; set; } = null!;
		public double Mehrwertsteuer { get; set; }

		public override void ValidateAndThrow()
		{
			new RichProductDtoValidator()
				.ValidateAndThrow(this);
		}

		public class RichProductDtoValidator : AbstractValidator<RichProductDto>
		{
			public RichProductDtoValidator()
			{
				Include(new ThinProductDtoValidator());

				RuleFor(configuration => configuration.Amount)
					.NotNull();
				RuleFor(configuration => configuration.Comments)
					.NotEmpty();
				RuleFor(configuration => configuration.DeliveryDate)
					.NotEmpty();
				RuleFor(configuration => configuration.DeliveryTime)
					.NotNull();
				RuleFor(configuration => configuration.Location)
					.NotEmpty();
				RuleFor(configuration => configuration.Mehrwertsteuer)
					.NotNull();
			}
		}
	}
}