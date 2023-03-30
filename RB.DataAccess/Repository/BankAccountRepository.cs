using RB.DataAccess.Repository.IRepository;
using RB.Models;
using RB.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.DataAccess.Repository
{
    public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
    {
        private readonly ReenBankContext _db;
        public BankAccountRepository(ReenBankContext db) : base(db)
        {
            _db = db;
        }

        public void Update(BankAccount bankAccount)
        {
            _db.BankAccounts.Update(bankAccount);
        }

        public string Transfer(BankAccount sender, BankAccount receiver, double Amount)
        {
            var receiverFromDb = _db.BankAccounts.Find(receiver.Id);
            if (receiverFromDb == null)
            {
                return "Invalid Account";
            }

            if (Amount > sender.Balance)
            {
                return "Insufficient";
            }
            else
            {
                var senderPerson = _db.ApplicationUsers.FirstOrDefault(x => x.BankAccountId == sender.Id);
                var receiverPerson = _db.ApplicationUsers.FirstOrDefault(x => x.BankAccount.AccountNumber == receiver.AccountNumber);
                var receiverAccount = _db.BankAccounts.FirstOrDefault(x => x.AccountNumber == receiver.AccountNumber);
                
                
                sender.Balance -= Amount;
                receiverAccount.Balance += Amount;

                Transactions senderT = new()
                {
                    Amount = Amount,
                    TransactionDate = DateTime.Now,
                    From = senderPerson.Name,
                    To = receiverPerson.Name,
                    TransactionType = TransactionType.Debit,
                    BankAccountId = sender.Id,
                    BankAccount = sender,
                };

                _db.Transactions.Add(senderT);

                Transactions receiverT = new()
                {
                    Amount = Amount,
                    TransactionDate = DateTime.Now,
                    From = receiverPerson.Name,
                    To = senderPerson.Name,
                    TransactionType = TransactionType.Credit,
                    BankAccountId = receiver.Id,
                    BankAccount = receiver,
                };
                _db.Transactions.Add(receiverT);
                _db.SaveChanges();
                
                return "success";
            }
            
        }

        public string Deposit(BankAccount account, double amount)
        {
            account.Balance += amount;

            Transactions transactions = new()
            {
                Amount = amount,
                TransactionDate = DateTime.Now,
                BankAccountId = account.Id,
                BankAccount = account,
                TransactionType = TransactionType.Credit,
                From = "Self",
                To = "Self",
            };
            _db.Transactions.Add(transactions);
            _db.SaveChanges();
            
            return "success";
        }
    }
}
