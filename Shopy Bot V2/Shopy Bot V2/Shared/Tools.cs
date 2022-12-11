using SpookVooper.Api.Entities;
using SpookVooper.Api.Entities.Groups;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Shared.Main;
using static Shared.Tools;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text.Json;
using static Shared.Cache;

namespace Shared;
public static class Tools
{
    public static async Task<List<string>> NameToSVIDs(string name)
    {
        if (name == "Old King")
            return new List<string> { "g-oldking" };

        List<string> svids = new();

        string gsvid = await Group.GetSVIDFromNameAsync(name);
        string usvid = await User.GetSVIDFromUsernameAsync(name);

        bool isgroup = gsvid.StartsWith('g');
        bool isuser = usvid.StartsWith('u');

        if (!isgroup && !isuser) return null;
        if (isgroup && isuser)
        {
            svids.Add(usvid);
            svids.Add(gsvid);
        }
        else svids.Add(isuser ? usvid : gsvid);

        return svids;
    }

    public static async Task<List<string>> ConvertToSVIDs(string input)
    {
        if (input == null) return null;
        List<string> entities = new();
        bool isSVID = await TestSVID(input);
        if (isSVID) entities.Add(input);
        else
        {
            List<string> nameEntities = await NameToSVIDs(input);
            if (nameEntities != null) entities.AddRange(nameEntities);
        }
        return entities;
    }

    public static async Task<(bool, string)> TryName(string svid)
    {
        if (!(svid.StartsWith('u') || svid.StartsWith('g'))) return (false, null);

        string result = await GetName(svid);
        return result is not null ? (true, result) : (false, null);
    }

    public static async Task<bool> TestSVID(string svid) => (svid.StartsWith('u') || svid.StartsWith('g')) && await Contains(svid);

    public static SVIDTypes SVIDToType(string svid)
    {
        if (svid.StartsWith('u')) return SVIDTypes.User;
        if (svid.StartsWith('g')) return SVIDTypes.Group;
        return SVIDTypes.None;
    }

    public static async Task<(List<SearchReturn>, List<SearchReturn>)> SearchName(string input)
    {
        List<SearchReturn> exact = new();
        List<SearchReturn> notExact = new();

        Stream response = await client.GetStreamAsync($"https://api.spookvooper.com/Entity/Search?name={input}");
        SearchReturn[] entities = await JsonSerializer.DeserializeAsync<SearchReturn[]>(response);

        foreach (SearchReturn entity in entities)
        {
            if (string.Equals(entity.Name, input, StringComparison.OrdinalIgnoreCase))
            {
                EntityCache.TryAdd(entity.SVID, (entity.Name, entity.Credits));
                exact.Add(entity);
            }
            else notExact.Add(entity);
        }

        return (exact, notExact);
    }

    public static async Task<(List<string> exact, List<(string, string)> notExact)> SearchNameToSVIDs(string input)
    {
        List<string> exact = new();
        List<(string name, string svid)> notExact = new();

        Stream response = await client.GetStreamAsync($"https://api.spookvooper.com/Entity/Search?name={input}");
        SearchReturn[] entities = await JsonSerializer.DeserializeAsync<SearchReturn[]>(response);

        foreach (SearchReturn entity in entities)
        {
            if (string.Equals(entity.Name, input, StringComparison.OrdinalIgnoreCase))
            {
                EntityCache.TryAdd(entity.SVID, (entity.Name, entity.Credits));
                exact.Add(entity.SVID);
            }
            else notExact.Add((entity.Name, entity.SVID));
        }

        return (exact, notExact);
    }

    public static async Task<(List<(string svid, decimal balance)>, List<(string, string)> notExact)> SearchNameToBalances(string input)
    {
        List<(string svid, decimal balance)> exact = new();
        List<(string name, string svid)> notExact = new();

        Stream response = await client.GetStreamAsync($"https://api.spookvooper.com/Entity/Search?name={input}");
        SearchReturn[] entities = await JsonSerializer.DeserializeAsync<SearchReturn[]>(response);

        foreach (SearchReturn entity in entities)
        {
            if (string.Equals(entity.Name, input, StringComparison.OrdinalIgnoreCase))
            {
                EntityCache.TryAdd(entity.SVID, (entity.Name, entity.Credits));
                exact.Add((entity.SVID, entity.Credits));
            }
            else notExact.Add((entity.Name, entity.SVID));
        }

        return (exact, notExact);
    }

    public static string SVIDToTypeString(string svid)
    {
        if (svid.StartsWith('u')) return nameof(SVIDTypes.User);
        if (svid.StartsWith('g')) return nameof(SVIDTypes.Group);
        return null;
    }

    public static string SVIDTypeToString(SVIDTypes type)
    {
        return type switch
        {
            SVIDTypes.User => nameof(SVIDTypes.User),
            SVIDTypes.Group => nameof(SVIDTypes.Group),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public enum SVIDTypes
    {
        User,
        Group,
        None
    }

    public enum StringInputs
    {
        Name,
        SVID
    }

    public class SearchReturn
    {
        [JsonPropertyName("id")]
        public string SVID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("image_Url")]
        public string Pfp { get; set; }
        [JsonPropertyName("credits")]
        public decimal Credits { get; set; }
    }
}