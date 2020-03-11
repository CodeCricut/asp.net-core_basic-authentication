using basic_authentication_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace basic_authentication_api.Domain
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
	}
}
