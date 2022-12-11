using Lilia.Database.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Lilia.Database;

public class LiliaDatabaseContextFactory : IDesignTimeDbContextFactory<LiliaDatabaseContext>
{
    public LiliaDatabaseContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<LiliaDatabaseContext> optionsBuilder = new DbContextOptionsBuilder<LiliaDatabaseContext>();
        SqliteConnectionStringBuilder connStringBuilder =
            new SqliteConnectionStringBuilder(
                $"Data Source=database.db;Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}");

        optionsBuilder.UseSqlite(connStringBuilder.ToString());
        LiliaDatabaseContext ctx = new LiliaDatabaseContext(optionsBuilder.Options);
        ctx.Database.SetCommandTimeout(30);
        return ctx;
    }
}

public class LiliaDatabaseContext : DbContext
{
    public DbSet<DbGuild> Guilds { get; set; }
    public DbSet<DbUser> Users { get; set; }

    public LiliaDatabaseContext(DbContextOptions<LiliaDatabaseContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}