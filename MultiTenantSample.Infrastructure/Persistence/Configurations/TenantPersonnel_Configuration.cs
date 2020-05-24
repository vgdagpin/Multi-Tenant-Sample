using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MultiTenantSample.Infrastructure.Persistence.Configurations
{
	public class TenantPersonnel_Configuration : IEntityTypeConfiguration<TenantPersonnel>
	{
		public void Configure(EntityTypeBuilder<TenantPersonnel> builder)
		{
			builder.Property<int>("GenderId");

			// helper hack for FilterTenantInterceptor.ReaderExecutingAsync
			builder.HasQueryFilter(a => a.TenantId == -999);

			builder.Property(a => a.FirstName)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(a => a.MiddleName)
				.HasMaxLength(50);

			builder.Property(a => a.LastName)
				.HasMaxLength(50)
				.IsRequired();


			builder.Property(a => a.TenantId)
				.HasDefaultValueSql("CONVERT(INT, SESSION_CONTEXT(N'TenantId'))");


			builder.HasOne(a => a.GenderFk)
				.WithOne()
				.HasForeignKey<TenantPersonnel>("GenderId");

			builder.HasData(new
			{
				Id = 1,
				TenantId = 1,
				FirstName = "Hazel",
				MiddleName = "Peterson",
				LastName = "Ramos",
				Active = true,
				DOB = new DateTime(1955, 9, 1),
				PrefixId = 1,
				GenderId = 1
			});

			builder.HasData(new
			{
				Id = 2,
				TenantId = 1,
				FirstName = "Dwight",
				LastName = "Nguyen",
				Active = true,
				DOB = new DateTime(1976, 3, 7),
				PrefixId = 1,
				GenderId = 2
			});

			builder.HasData(new
			{
				Id = 3,
				TenantId = 2,
				FirstName = "Kyle",
				LastName = "Davis",
				Active = true,
				DOB = new DateTime(1969, 2, 3),
				PrefixId = 1,
				GenderId = 3
			});
		}
    }
}