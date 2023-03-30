using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Models.ViewModels
{
    public class ProfileAccountTransactionsVM
    {
        public ApplicationUser SenderUser { get; set; }
        public ApplicationUser ReceiverUser { get; set; }
        public BankAccount SenderAccount { get; set; }
        public BankAccount ReceiverAccount { get; set; }
        public IEnumerable<Transactions> TransactionsList { get; set; }
        public double DepositAmount { get; set; }
        public double TotalIncome { get; set; }
        public double TotalExpenses { get; set; }

    }
}
