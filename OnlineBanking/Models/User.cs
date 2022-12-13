using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace OnlineBanking.Models
{
    public class User
    {
        

        //Unique ID Required
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required, MaxLength(50), RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters of the alphabet")]
        [Display(Name = "Customer FirstName")]
        public string UserFirstName { get; set; }

        [Required, MaxLength(50), RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters of the alphabet")]
        [Display(Name = "Customer LastName")]
        public string UserLastName { get; set; }

        [RegularExpression(@"^0{1}[\d]{9}$", ErrorMessage = "Only numbers")]
        public int Age { get; set; }

        public string email { get; set; }

        [Required, MaxLength(50), RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters of the alphabet")]
        [Display(Name = "Username")]
        public string UserUsername { get; set; }

      
        public string password { get; set; }
        public string verificationCode { get; set; }
        public bool Active { get; set; }

        public Role role { get; set; }

        public BankAccount? bankAccount
        {
            get; set;
        }


        //[MaxLength(50), RegularExpression(@"^\b[\dA-Za-z\s\-\\]+\b$", ErrorMessage = "Only letters and numbers")]
        //public string Address { get; set; }

        //[MaxLength(40), RegularExpression(@"^\b[A-Za-z\s]+$", ErrorMessage = "Please only use names.")]
        //public string City { get; set; }

     
    }
}
