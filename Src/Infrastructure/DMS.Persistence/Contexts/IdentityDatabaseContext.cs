using DMS.Domain.Attributes;
using DMS.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Persistence.Contexts
{
    public class IdentityDatabaseContext:IdentityDbContext<User>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<User>().Property<DateTime?>("InsertTime");
            //builder.Entity<User>().Property<DateTime?>("UpdateTime");


            DeclareAuditField(builder);

            SetTableKey(builder);
            RenameTable(builder);

            //base.OnModelCreating(builder);
        }

        private static void DeclareAuditField(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    builder.Entity(entityType.Name).Property<DateTime?>("InsertTime");
                    builder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    builder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    builder.Entity(entityType.Name).Property<bool>("IsRemoved");
                }
            }
        }

        private static void RenameTable(ModelBuilder builder)
        {
            builder.Entity<IdentityUser<string>>().ToTable("Users", "identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "identity");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "identity");
            builder.Entity<IdentityRole<string>>().ToTable("Roles", "identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "identity");
        }

        private static void SetTableKey(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
            builder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
        }

        public override int SaveChanges()
        {
            AssignAuditField();
            return base.SaveChanges();
        }

        private void AssignAuditField()
        {
            var modifiedEntries = ChangeTracker.Entries()
                            .Where(p => p.State == EntityState.Modified ||
                                        p.State == EntityState.Added ||
                                        p.State == EntityState.Deleted);

            foreach (var entry in modifiedEntries)
            {
                var entityType = entry.Context.Model.FindEntityType(entry.Entity.GetType());

                var insertedTime = entityType.FindProperty("InsertTime");
                var updatedTime = entityType.FindProperty("UpdateTime");
                var removedTime = entityType.FindProperty("RemoveTime");
                var isRemoved = entityType.FindProperty("IsRemoved");

                if (entry.State == EntityState.Added && insertedTime != null)
                {
                    entry.Property("InsertTime").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified && updatedTime != null)
                {
                    entry.Property("UpdateTime").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Deleted && removedTime != null && isRemoved != null)
                {
                    entry.Property("RemoveTime").CurrentValue = DateTime.Now;
                    entry.Property("IsRemoved").CurrentValue = true;
                }
            }
        }
    }
} 
