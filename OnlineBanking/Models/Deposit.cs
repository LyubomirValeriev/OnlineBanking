
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBanking.Models
{
    public class Deposit
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public DateTime date { get; set; }

        public double amount { get; set; }

        public BankAccount from { get; set; }

        public Deposit(DateTime date, double amount, BankAccount from)
        {
            this.date = date;
            this.amount = amount;
            this.from = from;
        }

        public Deposit(Deposit d)
        {
            this.amount = d.amount;
            this.from = d.from;
            this.date = d.date;
        }

        public Deposit() { }
    }
}
