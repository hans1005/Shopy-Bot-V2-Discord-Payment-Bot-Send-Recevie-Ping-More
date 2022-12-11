using System.ComponentModel.DataAnnotations;

namespace Shared.Models;
public class Register
{
    [Key]
    public string VerifKey { get; set; }
    public ulong Discord { get; set; }
}