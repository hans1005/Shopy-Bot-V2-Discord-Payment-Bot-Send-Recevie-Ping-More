using System.Text.Json;
using System.Text.Json.Serialization;
using CardsAgainstDiscord.Model;
using Microsoft.EntityFrameworkCore;

namespace CardsAgainstDiscord.Data;

public class CardsImportingService : BackgroundService
{
    private readonly IDbContextFactory<CardsDbContext> _factory;
    private readonly ILogger<CardsImportingService> _logger;

    public CardsImportingService(ILogger<CardsImportingService> logger, IDbContextFactory<CardsDbContext> factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var context = await _factory.CreateDbContextAsync(stoppingToken);

        // Only import new cards if there are no cards in the database
        if (await context.WhiteCards.AnyAsync(stoppingToken) ||
            await context.BlackCards.AnyAsync(stoppingToken)) return;

        var source = await File.ReadAllTextAsync(@"Data/cards.json", stoppingToken);
        var cards = JsonSerializer.Deserialize<CardsAgainstJsonSource>(source)!;

        var white = cards.WhiteCards.Select(s => new WhiteCard {Text = s});
        var black = cards.BlackCards.Select(s => new BlackCard {Text = s.Text, Picks = s.Picks});

        await context.WhiteCards.AddRangeAsync(white, stoppingToken);
        await context.BlackCards.AddRangeAsync(black, stoppingToken);
        await context.SaveChangesAsync(stoppingToken);
    }

#nullable disable

    private class BlackCardSource
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("pick")]
        public int Picks { get; set; }
    }

    private class CardsAgainstJsonSource
    {
        [JsonPropertyName("white")]
        public IEnumerable<string> WhiteCards { get; set; }

        [JsonPropertyName("black")]
        public IEnumerable<BlackCardSource> BlackCards { get; set; }
    }

#nullable restore
}