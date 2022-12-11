using System.ComponentModel.DataAnnotations;

namespace Shared.Models;
public class User
{
    [Key]
    public string SVID { get; set; }
    public string Token { get; set; }
    public ulong? Valour { get; set; }
    public ulong? Discord { get; set; }
    public string ValourName { get; set; }
}