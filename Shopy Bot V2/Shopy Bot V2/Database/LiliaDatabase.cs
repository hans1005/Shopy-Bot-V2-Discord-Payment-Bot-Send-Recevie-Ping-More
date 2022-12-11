using System.Data.Common;
using System.Linq;
using Lilia.Commons;
using Lilia.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Lilia.Database;

public class LiliaDatabase
{
    private DbContextOptions<LiliaDatabaseContext> options;

    public LiliaDatabase()
    {
        DbContextOptionsBuilder<LiliaDatabaseContext> optionsBuilder = new DbContextOptionsBuilder<LiliaDatabaseContext>();
        SqliteConnectionStringBuilder connStringBuilder = new SqliteConnectionStringBuilder($"Data Source=database.db;Password={JsonManager<BotConfiguration>.Read().Credentials.DbPassword}");

        optionsBuilder.UseSqlite(connStringBuilder.ToString());
        this.options = optionsBuilder.Options;
        this.Setup();
    }

    private void Setup()
    {
        Log.Logger.Information("Executing database migrations, if any");

        using LiliaDatabaseContext context = new LiliaDatabaseContext(this.options);
        while (context.Database.GetPendingMigrations().Any())
        {
            string nextMigration = context.Database.GetPendingMigrations().First();
            using LiliaDatabaseContext migrationContext = new LiliaDatabaseContext(this.options);
            migrationContext.Database.Migrate();
            migrationContext.SaveChanges();
        }

        context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL");
        context.SaveChanges();
    }

    public LiliaDatabaseContext GetContext()
    {
        LiliaDatabaseContext context = new LiliaDatabaseContext(this.options);
        context.Database.SetCommandTimeout(30);
        DbConnection conn = context.Database.GetDbConnection();
        conn.Open();

        using DbCommand com = conn.CreateCommand();
        // https://phiresky.github.io/blog/2020/sqlite-performance-tuning/
        com.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF";
        com.ExecuteNonQuery();

        return context;
    }
}