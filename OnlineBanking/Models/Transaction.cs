using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBanking.Models
{
    public class Transaction
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public DateTime date { get; set; }

        public string ToWhom { get; set; }
        public string Memo { get; set; }
        public double amount { get; set; }

        public BankAccount from { get; set; }

        public Transaction(DateTime date, string toWhom, string memo, double amount, BankAccount from)
        {
            this.date = date;
            ToWhom = toWhom;
            Memo = memo;
            this.amount = amount;
            this.from = from;
        }

        public Transaction(Transaction t)
        {
            this.ToWhom = new string(t.ToWhom);
            this.Memo = new string(t.Memo);
            this.amount = t.amount;
            this.from = t.from;
            this.date = t.date;
        }
    }
}
