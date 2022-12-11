using System.ComponentModel.DataAnnotations;
using SpookVooper.Api.Economy;

namespace Shared.Models;
public class Transaction
{
    [Key]
    public int Count { get; set; }
    public string FromAccount { get; set; }
    public string ToAccount { get; set;  }
    public decimal Amount { get; set; }
    public string Detail { get; set; }
    public bool Force { get; set; }
    public bool? Success { get; set; }
    public ApplicableTax Tax { get; set; }
    public long Timestamp { get; set; }
}