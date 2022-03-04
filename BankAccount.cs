namespace BankAccountAPI;
public class BankAccount
{
    public string Number { get; }
    public string Owner { get; set; }

    private List<TransactionList> allTransactions = new();

    private static int accountNumberSeed = 1234567890;
    public BankAccount(string name, double initialBalance)
    {
        Number = accountNumberSeed.ToString();

        accountNumberSeed++;

        Owner = name;

        MakeDeposit(initialBalance, "Initial balance");
    }

    public double Balance
    {
        get
        {
            var balance = 0.0;

            foreach (var item in allTransactions)
            {
                balance += item.Amount;
            }

            return balance;
        }
    }

    public void MakeDeposit(double amount, string note)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");

        var deposit = new TransactionList { Amount = amount, Notes = note };
        allTransactions.Add(deposit);
    }

    public void MakeWithdrawal(int type, double amount, string note)
    {
        ValidateInputAmount(amount);

        amount = ValidateInputType(type, amount);

        var withdrawal = new TransactionList { Amount = -amount, Notes = note };
        allTransactions.Add(withdrawal);
    }

    private static double ValidateInputType(int type, double amount)
    {
        switch (type)
        {
            case 1:
                var fees = 0.1;
                amount += ((amount / 100) * fees);
                return amount;
            case 2:
                return amount;
            default:
                throw new ArgumentException(message: "invalid type\n1.DuitNow - surcharge of RM0.10 fees for per RM100 withdrawal.\nIBG - free of charge.",
                    paramName: nameof(type));
        }
    }

    private void ValidateInputAmount(double amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");

        if (Balance - amount < 0) throw new InvalidOperationException("Not sufficient funds for this withdrawal");
    }

    /*       public enum TransactionType
           {
               Withdrawal,
               Deposit
           }*/

    const string fileHeader =
@"-----------------------------------------------------------------
Date/Time       Amount  Balance Note
------------------------------------------------------------------";
    public string AccountTransactionRecord()
    {
        var report = new System.Text.StringBuilder();

        var balance = 0.0;
        report.AppendLine(fileHeader);

        foreach (var item in allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"{item.DateTime.ToString("g")}\t{item.Amount}\t{balance}\t{item.Notes}");
        }

        File.WriteAllText("C:\\Temp\\csc.txt", Convert.ToString(report));
        return report.ToString();
    }

    private readonly Random _rnd = new();
    private int RandomNumberGenerator(int lowerBound, int upperBound)
    {
        var currentVal = _rnd.Next(lowerBound, upperBound);
        var previousVal = currentVal;
        currentVal++;

        return currentVal;
    }
}