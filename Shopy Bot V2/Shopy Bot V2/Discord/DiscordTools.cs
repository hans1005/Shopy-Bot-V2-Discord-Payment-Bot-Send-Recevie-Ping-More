using System.Threading.Tasks;
using SpookVooper.Api.Entities;
using Shared;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using User = Shared.Models.User;

namespace Discord;
public class DiscordTools
{
    public static async Task<string> DiscordToSVID(ulong id, CocaBotWebContext db)
    {
        string DbSVID = await db.Users
            .Where(x => x.Discord == id) // Same as: WHERE Discord = id
            .Select(x => x.SVID) // Same as: SELECT SVID
            .SingleOrDefaultAsync(); // Where it executes

        if (DbSVID is not null) return DbSVID;

        string DiscordSVID = await SpookVooper.Api.Entities.User.GetSVIDFromDiscordAsync(id);
        return DiscordSVID != "u-2a0057e6-356a-4a49-b825-c37796cb7bd9" ? DiscordSVID : null;
    }

    public static async Task<User> DiscordToUserToken(ulong id, CocaBotWebContext db) => 
               await db.Users
                       .Where(x => x.Discord == id) // Same as: WHERE Discord = id
                       .Select(x => new User() { SVID = x.SVID, Token = x.Token }) // Same as: SELECT SVID, Token
                       .SingleOrDefaultAsync(); // Where it executes
}

