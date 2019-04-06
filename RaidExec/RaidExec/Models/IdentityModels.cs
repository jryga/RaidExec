using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace RaidExec.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserAccount = new UserAccount
            {
                ApplicationUser = this
            };
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        public virtual UserAccount UserAccount { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Game> Games { get; set; }

        public DbSet<Raid> Raids { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<RaidInvite> RaidInvitations { get; set; }

        public DbSet<GuildInvite> GuildInvitations { get; set; }

        public DbSet<GroupInvite> GroupInvitations { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>()
            .HasMany<Raid>(c => c.Raids)
            .WithMany(r => r.Members)
            .Map(cs =>
            {
                cs.MapLeftKey("CharacterId");
                cs.MapRightKey("RaidId");
                cs.ToTable("CharacterRaid");
            });

            //modelBuilder.Entity<ApplicationUser>()
            //    .HasRequired(t => t.UserAccount)
            //    .WithRequiredPrincipal(t => t.ApplicationUser);

           // modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}