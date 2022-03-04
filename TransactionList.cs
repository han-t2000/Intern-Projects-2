namespace BankAccountAPI;
public class TransactionList
{
    public double Amount { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Notes { get; set; }
}
