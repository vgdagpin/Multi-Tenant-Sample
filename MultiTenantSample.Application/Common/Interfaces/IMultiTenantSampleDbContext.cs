using MultiTenantSample.Domain.Entities;
using MultiTenantSample.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenantSample.Application
{
	public interface IMultiTenantSampleDbContext
	{
		#region Entities
		DbSet<Gender> Genders { get; set; }         
		DbSet<TenantPersonnel> TenantPersonnels { get; set; }         
		#endregion      
	}
}