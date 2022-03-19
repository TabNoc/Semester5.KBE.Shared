using System;
using FluentValidation;
using KBE.Shared.Interfaces;

namespace KBE.Shared.Models
{
	public class ProductStorageDto : IValidatable
	{
		public int Amount { get; set; }
		public TimeSpan DeliveryTime { get; set; }
		public string Location { get; set; } = null!;
		public Guid ProductId { get; set; }

		public void ValidateAndThrow()
		{
			new ProductStorageDtoValidator()
				.ValidateAndThrow(this);
		}

		public class ProductStorageDtoValidator : AbstractValidator<ProductStorageDto>
		{
			public ProductStorageDtoValidator()
			{
				RuleFor(configuration => configuration.Amount)
					.NotNull();
				RuleFor(configuration => configuration.DeliveryTime)
					.NotEmpty();
				RuleFor(configuration => configuration.Location)
					.NotEmpty();
				RuleFor(configuration => configuration.ProductId)
					.NotEmpty();
			}
		}
	}
}