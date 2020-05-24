using MultiTenantSample.Domain.Entities;
using MultiTenantSample.Domain.Entities.Base;
using MultiTenantSample.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace MultiTenantSample.Infrastructure.Persistence
{
	public class MultiTenantSampleDbContext : DbContext, IMultiTenantSampleDbContext
	{
		#region Entities
		public DbSet<Gender> Genders {get;set;} 
		public DbSet<TenantPersonnel> TenantPersonnels {get;set;} 
		#endregion


		public MultiTenantSampleDbContext(DbContextOptions<MultiTenantSampleDbContext> dbContextOpt) : base(dbContextOpt)
		{

		}
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}