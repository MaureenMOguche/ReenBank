using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RB.Models;

namespace RB.DataAccess;

public class ReenBankContext : IdentityDbContext<IdentityUser>
{
    public ReenBankContext(DbContextOptions<ReenBankContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transactions> Transactions { get; set; }
}
