using Microsoft.EntityFrameworkCore;
using Shared.Models;
using static Shared.Main;

namespace Shared;
public class CocaBotWebContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Register> Registers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public readonly static string ConnectionString = $@"server={config.Server};userid={config.UserID};password={config.Password};database={config.Database}";
    public readonly static MySqlServerVersion version = new("8.0.26");

    public CocaBotWebContext(DbContextOptions<CocaBotWebContext> options) : base(options)
    {

    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseMySql(ConnectionString, version, options => options.EnableRetryOnFailure());
}

public class CocaBotContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Register> Registers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public static readonly string ConnectionString = $@"server={config.Server};userid={config.UserID};password={config.Password};database={config.Database}";
    public static readonly MySqlServerVersion version = new("8.0.26");
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
        optionsBuilder.UseMySql(ConnectionString, version, options => options.EnableRetryOnFailure());
}