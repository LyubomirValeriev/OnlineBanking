using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineBanking.Models
{
    public class BankAccount
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string IBAN { get; set; }
        public double Balance{ get; set; }
        public string Holder { get; set; }

        public ICollection<Transaction> transactions { get; set; }
    }
}