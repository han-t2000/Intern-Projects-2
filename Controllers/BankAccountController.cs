using Microsoft.AspNetCore.Mvc;

namespace BankAccountAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankAccountController : ControllerBase
{
    private static List<BankAccount> AccountList = new()
    {
        new("Alice", 2000),
        new("Bryan", 600),
        new("Connor", 1000)
    };

    [HttpGet]
    public ActionResult<List<BankAccount>> Get()
    {
        return Ok(AccountList);
    }

    [HttpPost]
    [Route("withdraw")]
    public ActionResult<BankAccount> Withdraw(string account, int withdrawalType, double amount, string notes)
    {
        var accountNumber = AccountList.Find(item => item.Number.Equals(account, StringComparison.InvariantCultureIgnoreCase));

        if (accountNumber == null) return NotFound();

        accountNumber.MakeWithdrawal(withdrawalType, amount, notes);
        accountNumber.AccountTransactionRecord();
        return Ok(accountNumber);
    }

    [HttpPost]
    [Route("deposit")]
    public ActionResult<BankAccount> Deposit(string account, double amount, string notes)
    {
        var accountNumber = AccountList.Find(item => item.Number.Equals(account, StringComparison.InvariantCultureIgnoreCase));

        if (accountNumber == null) return NotFound();

        accountNumber.MakeDeposit(amount, notes);
        accountNumber.AccountTransactionRecord();
        return Ok(accountNumber);
    }

    [HttpDelete]
    [Route("delete")]
    public ActionResult<BankAccount> Delete(string account)
    {
        var accountNumber = AccountList.Find(item => item.Number.Equals(account, StringComparison.InvariantCultureIgnoreCase));

        if (accountNumber == null) return NotFound();

        AccountList.Remove(accountNumber);
        return NoContent();

    }
}
