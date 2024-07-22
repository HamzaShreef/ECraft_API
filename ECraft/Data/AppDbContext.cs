﻿using ECraft.Models;
using ECraft.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECraft.Data
{
	public class AppDbContext :
		IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
	{
        public DbSet<Craft> Crafts { get; set; }

        public DbSet<CrafterProfile> Crafters { get; set; }

        public DbSet<CrafterReview> CraftersReviews  { get; set; }

        public DbSet<UserInteraction> UserInteractions { get; set; }

        public DbSet<ProfileView> ProfileViews { get; set; }

        public DbSet<CraftAchievement> CraftAchievements { get; set; }

        public DbSet<LocationCountry> LCountries { get; set; }

        public DbSet<LocationRegion> LRegions { get; set; }

        public DbSet<LocationCity> LCities { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

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
