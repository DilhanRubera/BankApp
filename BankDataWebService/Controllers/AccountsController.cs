using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankDataModels;
using BankDataWebService.Data;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DBManager _context;

        public AccountsController(DBManager context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(account);
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }

        [HttpGet]
        [Route("getAccountTransactions/{id}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAccountTransactions(int id)
        {
            var account = await _context.Accounts
                .Include(u => u.TransactionList)
                .FirstOrDefaultAsync(u => u.AccountId == id);

            if (account == null)
            {
                return NotFound();

            }

            var transactionList = account.TransactionList;



            var receivedTransactions = await _context.Transactions
            .Where(t => t.ReceiverId == account.AccountId && t.SenderId != account.AccountId)
            .ToListAsync();

            transactionList.AddRange(receivedTransactions);

            foreach (var transaction in transactionList)
            {
                Console.WriteLine(transaction);
            }
            if (transactionList.Count == 0)
            {
                return Ok("No transactions found");
            }

            var transactionDtoList = transactionList.Select(t => new TransactionDto
            {
                TransactionId = t.TransactionId,
                TransactionDescription = t.TransactionDescription,
                TransactionAmount = t.TransactionAmount,
                SenderAccountNo = t.SenderAccountNo,
                ReceiverAccountNo = t.ReceiverAccountNo,
                TransactionDate = t.TransactionDate
            }).ToList();
            
            return Ok(transactionDtoList);
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            if (account.isActivated)
            {
                return BadRequest("Account is already activated.");
            }
            account.isActivated = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            if (!account.isActivated)
            {
                return BadRequest("Account is already deactivated.");
            }
            account.isActivated = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

