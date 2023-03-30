using RB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.DataAccess.Repository.IRepository
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        void Update(BankAccount bankAccount);
        string Deposit(BankAccount account, double amount);
        string Transfer(BankAccount sender, BankAccount receiver, double Amount);
    }

}
