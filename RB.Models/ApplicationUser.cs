using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [ValidateNever]
        public string? ProfilePicture { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }



        //Relationships
        public int BankAccountId { get; set; }
        [ForeignKey("BankAccountId")]
        public BankAccount BankAccount { get; set; }


    }
}
