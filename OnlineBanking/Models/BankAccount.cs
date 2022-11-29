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
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string IBAN { get; set; }
        public int Balance{ get; set; }
        public string Holder { get; set; }

        public List<Transaction> transactions { get; set; }
    }
}