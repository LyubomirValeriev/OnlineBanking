using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBanking.Models
{
    public class Transaction
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime date { get; set; }

        public string ToWhom { get; set; }
        public string Memo { get; set; }
        public double amount { get; set; }

        public BankAccount from { get; set; }

        
    }
}
