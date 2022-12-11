using CardsAgainstDiscord.Model;
using Microsoft.EntityFrameworkCore;

namespace CardsAgainstDiscord.Data;

public class CardsDbContext : DbContext
{
    public CardsDbContext(DbContextOptions<CardsDbContext> options) : base(options)
    {
    }

    public virtual DbSet<BlackCard> BlackCards { get; protected set; } = null!;

    public virtual DbSet<WhiteCard> WhiteCards { get; protected set; } = null!;

    public virtual DbSet<Lobby> Lobbies { get; protected set; } = null!;

    public virtual DbSet<Player> Players { get; protected set; } = null!;

    public virtual DbSet<Game> Games { get; protected set; } = null!;

    public virtual DbSet<PickedCard> Picks { get; protected set; } = null!;

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<Lobby>().HasAlternateKey(l => new {l.GuildId, l.ChannelId, l.MessageId});

        model.Entity<Player>()
            .HasMany(p => p.WhiteCards)
            .WithMany(c => c.Players);

        model.Entity<Player>()
            .HasMany(p => p.PickedCards)
            .WithOne(c => c.Player)
            .HasForeignKey(c => c.PlayerId);

        model.Entity<Player>()
            .HasOne(p => p.SelectedWhiteCard).WithMany()
            .HasForeignKey(p => p.SelectedWhiteCardId);

        model.Entity<Game>()
            .HasOne(g => g.BlackCard).WithMany()
            .HasForeignKey(g => g.BlackCardId);

        model.Entity<Game>()
            .HasMany(g => g.UsedBlackCards)
            .WithMany(c => c.Games);

        model.Entity<Game>()
            .HasMany(g => g.UsedWhiteCards)
            .WithMany(g => g.Games);

        model.Entity<Game>()
            .HasMany(g => g.Players)
            .WithOne(p => p.Game)
            .HasForeignKey(p => p.GameId);

        model.Entity<Game>()
            .HasOne(g => g.Judge)
            .WithMany(p => p.JudgedGames)
            .HasForeignKey(g => g.JudgeId);

        model.Entity<PickedCard>()
            .HasOne(c => c.WhiteCard)
            .WithMany(c => c.Picks)
            .HasForeignKey(c => c.WhiteCardId);

        model.Entity<PickedCard>()
            .HasOne(c => c.Player)
            .WithMany(p => p.PickedCards)
            .HasForeignKey(c => c.PlayerId);
    }
}