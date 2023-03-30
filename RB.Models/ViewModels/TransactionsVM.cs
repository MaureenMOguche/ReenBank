using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Models.ViewModels
{
    public class TransactionsVM
    {
        public ApplicationUser AccountUser { get; set; }
        public BankAccount BankAccount { get; set; }
        public IEnumerable<Transactions> TransactionsList { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
