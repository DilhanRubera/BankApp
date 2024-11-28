using Microsoft.EntityFrameworkCore;
using BankDataModels;
using System;

namespace BankDataWebService.Data
{
    public class DBManager : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*base.OnConfiguring(optionsBuilder);*/
            optionsBuilder.UseSqlite(@"Data Source = Bank.db;");
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts{get; set ;}

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Account>().HasOne(a=> a.AccountHolder).WithMany(u=> u.AccountList).HasForeignKey(a=>a.AccountHolderId);
            modelBuilder.Entity<Transaction>().HasOne(t=>t.SenderAccount).WithMany(a=>a.TransactionList).HasForeignKey(t=>t.SenderId);
            modelBuilder.Entity<Admin>();
            modelBuilder.Entity<AuditLog>();

            // List of first names and last names to randomly choose from
            var firstNames = new List<string> { "John", "Jane", "Michael", "Sarah", "Robert", "Laura" };
            var lastNames = new List<string> { "Smith", "Johnson", "Brown", "Williams", "Jones", "Miller" };
           
            var accountNames = new List<string> { "Savings", "Checkings", "Deposits", "Money Market " };

            var users = new List<User>();
            var accounts = new List<Account>();
            var transactions = new List<Transaction>();
            var random = new Random();

            // Create 6 users with random names and emails
            for (int i = 1; i <= 6; i++)
            {
                // Pick random first and last name
                var firstName = firstNames[random.Next(firstNames.Count)];
                var lastName = lastNames[random.Next(lastNames.Count)];

                // Generate email based on the selected names
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(1, 100)}@example.com";

                // Generate a random 9-digit phone number that doesn't start with 0
                var phoneNo = random.Next(100000000, 999999999);  // Ensures 9-digit phone number

                // Create a unique user
                var user = new User
                {
                    UserId = i, // User ID
                    Name = $"{firstName} {lastName}",
                    Email = email,
                    PhoneNo = phoneNo,
                    Password= "123"
                };
                users.Add(user);

                // Create 3 accounts for each user with unique 6-digit account numbers
                for (int j = 1; j <= 3; j++)
                {
                    int accountNumber;

                    do
                    {
                        accountNumber = random.Next(100000, 999999); // Six-digit account number, not starting with 0
                    }
                    while (accounts.Any(a => a.AccountNumber == accountNumber));
                    // Six-digit account number, not starting with 0
                    var account = new Account
                    {
                        AccountId = i * 10 + j,
                         // Ensure unique AccountId
                        AccountHolderId = user.UserId,
                        AccountNumber = accountNumber, // Account number is stored as an int
                        AccountBalance = random.Next(1000, 100000),
                        AccountName = accountNames[random.Next(accountNames.Count)],
                        isActivated = true,
                       // Random initial balance
                    };
                    accounts.Add(account);

                    for (int k = 1; k <= 5; k++)
                    {
                        // Pick a random receiver (excluding the current sender)
                        var receiverAccount = accounts[random.Next(accounts.Count)];

                        var transaction = new Transaction
                        {
                            TransactionId = i * 100 + j * 10 + k,
                            // Ensure unique TransactionId
                            SenderId = account.AccountId, // Current account as sender
                            ReceiverId = receiverAccount.AccountId, // Random other account as receiver
                            TransactionAmount = random.Next(50, 5000), // Random amount between 50 and 500
                            TransactionDescription = $"Payment from {account.AccountNumber} to {receiverAccount.AccountNumber}",
                            TransactionDate = DateTime.Now.AddDays(-random.Next(1, 100)),
                            SenderAccountNo = account.AccountNumber,
                            ReceiverAccountNo = receiverAccount.AccountNumber,
                            // Random date within the last 100 days
                        };
                        transactions.Add(transaction);
                    }
                }
            }

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Account>().HasData(accounts);
            modelBuilder.Entity<Transaction>().HasData(transactions);
        }
    }
}
