using ECraft.Models;
using ECraft.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECraft.Data
{
	public class AppDbContext :
		IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
	{
        public DbSet<Craft> Crafts { get; set; }

        public DbSet<Crafter> Crafters { get; set; }

        public DbSet<CraftProject> CraftProjects { get; set; }

        public DbSet<LocationCountry> Countries { get; set; }

        public DbSet<LocationState> LocationStates { get; set; }

        public DbSet<LocationCity> Cities { get; set; }

        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}
	}
}
