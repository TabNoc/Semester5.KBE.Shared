using System;
using FluentValidation;
using KBE.Shared.Interfaces;
using KBE.Shared.Models.Enum;

namespace KBE.Shared.Models
{
	public class ThinProductDto : IValidatable
	{
		public string Description { get; set; } = null!;
		public string Name { get; set; } = null!;
		public double Price { get; set; }
		public ProductCategory ProductCategory { get; set; }
		public Guid ProductId { get; set; }

		public virtual void ValidateAndThrow()
		{
			new ThinProductDtoValidator()
				.ValidateAndThrow(this);
		}

		public class ThinProductDtoValidator : AbstractValidator<ThinProductDto>
		{
			public ThinProductDtoValidator()
			{
				RuleFor(configuration => configuration.Description)
					.NotEmpty();
				RuleFor(configuration => configuration.Name)
					.NotEmpty();
				RuleFor(configuration => configuration.Price)
					.NotNull();
				RuleFor(configuration => configuration.ProductCategory)
					.NotNull()
					.IsInEnum();
				RuleFor(configuration => configuration.ProductId)
					.NotEmpty();
			}
		}
	}
}