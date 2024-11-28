using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankDataModels;
using BankDataWebService.Data;
using Azure.Core;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBManager _context;

        public UsersController(DBManager context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Check if a user with the same name already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if (existingUser != null)
            {
                return BadRequest("A user with the same name already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        [HttpPost]
        [Route("getUserByUsername")]
        public async Task<IActionResult> GetUserByUsername([FromBody] UserLoginRequest userLoginRequest)
        {
            Console.WriteLine("Got to getUserByUSername method");

            Console.WriteLine(userLoginRequest.Username);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userLoginRequest.Username && u.Password==userLoginRequest.Password);
            if (user == null)
            {
                return BadRequest("User Not Found or Incorrect Password");
                
                
            }

            Console.WriteLine(user.Name + user.PhoneNo);
            User userObj = new User();
            userObj.Name = user.Name;
            userObj.UserId = user.UserId;
            userObj.PhoneNo = user.PhoneNo;
            userObj.Address = user.Address;
            userObj.Email = user.Email;
            userObj.AccountList = user.AccountList;
            userObj.Password = user.Password;
            userObj.ProfilePicdBytes = user.ProfilePicdBytes;
            Console.WriteLine(userObj.Name + userObj.PhoneNo + userObj.Address);
            return new ObjectResult(userObj) { StatusCode = 200 };
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (updatedUser.Name != null)
                existingUser.Name = updatedUser.Name;
            if (updatedUser.Email != null)
                existingUser.Email = updatedUser.Email;
            if (updatedUser.PhoneNo != 0)
                existingUser.PhoneNo = updatedUser.PhoneNo;
            if (updatedUser.Password != null)
                existingUser.Password = updatedUser.Password;
            if (updatedUser.Address != null)
                existingUser.Address = updatedUser.Address;

            if (!string.IsNullOrEmpty(updatedUser.ProfilePicBase64))
            {
                existingUser.ProfilePicdBytes = Convert.FromBase64String(updatedUser.ProfilePicBase64); // Convert base64 to byte[]
                Console.WriteLine(existingUser.ProfilePicdBytes);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var user = await _context.Users.FindAsync(id);
            User userObj = new User();
            userObj.Name = user.Name;
            userObj.UserId = user.UserId;
            userObj.PhoneNo = user.PhoneNo;
            userObj.Address = user.Address;
            userObj.Email = user.Email;
            userObj.AccountList = user.AccountList;
            userObj.Password = user.Password;
            userObj.ProfilePicdBytes=user.ProfilePicdBytes;
            Console.WriteLine(userObj.Name + userObj.PhoneNo + userObj.Address);
            return new ObjectResult(userObj) { StatusCode = 200 };

        }


        [HttpPut]
        [Route("addAccountForUser/{id}")]
        public async Task<ActionResult<Account>> AddAccountToUser(int id, Account account)
        {
            
            var user = await _context.Users
                .Include(u => u.AccountList) 
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (user != null)
            {
                Console.WriteLine("user exists");
            }

            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == account.AccountNumber);

            if (existingAccount != null)
            {
                return BadRequest("Account number already in use.");
            }

            if (account.AccountNumber.ToString().StartsWith("0"))
            {
                return BadRequest("Account number cannot start with zero.");
            }

            account.AccountHolderId = user.UserId;

            user.AccountList.Add(account);

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();

            return Ok("account added");
        }


        [HttpGet]
        [Route("getUserAccounts/{id}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllUserAccounts(int id)
        {
            var user = await _context.Users
                .Include(u => u.AccountList)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Retrieve the user's accounts
            var accountList = user.AccountList;

             
            
            if (accountList == null)
            {
                return Ok("No accounts found for this user.");
            }

            // Print out account details to the console
            Console.WriteLine($"Accounts for user {user.Name}:");
            foreach (var account in accountList)
            {
                Console.WriteLine("Account ID: " + account.AccountId);
                Console.WriteLine("Account Number: " + account.AccountNumber);
                Console.WriteLine("Account Name: " + account.AccountName);
                Console.WriteLine("Account Balance: " + account.AccountBalance);
                Console.WriteLine("-----------------------------");
            }

            return Ok(accountList);
        }

        [HttpPost]
        [Route("makeTransaction/{userId}")]
        public async Task<IActionResult> MakeTransaction([FromBody] TransactionRequest transactionRequest, int userId)
        {
            Console.WriteLine(transactionRequest.SenderAccountNo);
            Console.WriteLine(transactionRequest.ReceiverAccountNo);
            Console.WriteLine(transactionRequest.TransactionAmount);
            Console.WriteLine(userId);

            var senderAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountHolderId == userId
            && a.AccountNumber == transactionRequest.SenderAccountNo && a.isActivated);

            if (senderAccount == null)
            {
                return BadRequest("This account does not belong to you or is not activated");
            }
            Console.WriteLine("sender account exists and belongs to u" + senderAccount.AccountHolderId);

            if (senderAccount.AccountBalance < transactionRequest.TransactionAmount)
            {
                return BadRequest("Insufficient balance.");
            }
            Console.WriteLine("sender account has enough money");

            var receiverAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == transactionRequest.ReceiverAccountNo && a.isActivated);

            if (receiverAccount == null)
            {
                return BadRequest("Receiver account not found.");
            }
            Console.WriteLine("receiver account found");

            senderAccount.AccountBalance -= transactionRequest.TransactionAmount;
            Console.WriteLine(senderAccount.AccountBalance);

            receiverAccount.AccountBalance += transactionRequest.TransactionAmount;
            Console.WriteLine(receiverAccount.AccountBalance);

            var transaction = new Transaction
            {
                SenderAccountNo = senderAccount.AccountNumber,
                ReceiverAccountNo = receiverAccount.AccountNumber,
                TransactionAmount = transactionRequest.TransactionAmount,
                TransactionDescription = transactionRequest.TransactionDescription,
                SenderId = senderAccount.AccountId,
                ReceiverId = receiverAccount.AccountId,
                TransactionDate = DateTime.Now,
           
            };

            // Add the transaction to the database
            _context.Transactions.Add(transaction);
            Console.WriteLine("transaction added");
           /* Console.WriteLine(transaction.SenderId);
            Console.WriteLine(transaction.SenderAccountNo);
            Console.WriteLine(transaction.ReceiverAccountNo);
            Console.WriteLine(transaction.ReceiverId);*/

            senderAccount.TransactionList.Add(transaction);
       
            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Transaction successful.");

        }

        // GET: api/Users/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers(string search, string option)
        {
            IQueryable<User> query = _context.Users;

            switch (option)
            {
                case "name":
                    query = query.Where(u => u.Name.Contains(search));
                    break;
                case "accountnumber":
                    if (int.TryParse(search, out int accountNumber))
                    {
                        query = query.Where(u => u.AccountList.Any(a => a.AccountNumber == accountNumber));
                    }
                    break;
                default:
                    return BadRequest("Invalid search option.");
            }

            var users = await query.ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found matching the search criteria.");
            }

            return Ok(users);
        }

    }

}
