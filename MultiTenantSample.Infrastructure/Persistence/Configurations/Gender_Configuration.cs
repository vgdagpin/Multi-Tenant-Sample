using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSample.Infrastructure.Persistence.Configurations
{
	public class Gender_Configuration : IEntityTypeConfiguration<Gender>
	{
		public void Configure(EntityTypeBuilder<Gender> builder)
		{
			builder.Property(a => a.Name)
				.HasMaxLength(25)
				.IsRequired();

			builder.HasData(new Gender
			{
				Id = 1,
				Name = "Female"
			});

			builder.HasData(new Gender
			{
				Id = 2,
				Name = "Male"
			});

			builder.HasData(new Gender
			{
				Id = 3,
				Name = "Male"
			});
		}
	}
}