using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string TransactionType { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        public string? Description { get; set; }
        public string? CreditType { get; set; }



        //Relationships
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId")]
        public BankAccount BankAccount { get; set; }
    }
}
