using Shared;
using SpookVooper.Api.Economy;
using Transaction = SpookVooper.Api.Economy.Transaction;
using DBTransaction = Shared.Models.Transaction;
using static Shared.Main;

await GetConfig<DefaultConfig>();

Console.WriteLine("Starting Logger...");

// Create transaction hub object
TransactionHub tHub = new();

// Hook transaction event to method
tHub.OnTransaction += HandleTransaction;

// Prevent process death
await Task.Delay(Timeout.Infinite);

static async void HandleTransaction(Transaction transaction)
{
    var db = new CocaBotContext();

    DBTransaction dBTransaction = new()
    {
        Amount = transaction.Amount,
        FromAccount = transaction.FromAccount,
        ToAccount = transaction.ToAccount,
        Detail = transaction.Detail,
        Force = transaction.Force,
        Success = transaction.Result.Succeeded,
        Tax = transaction.Tax,
        Timestamp = transaction.Timestamp
    };

    await db.Transactions.AddAsync(dBTransaction);
    await db.SaveChangesAsync();
}