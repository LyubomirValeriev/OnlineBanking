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
        public string Description { get; set; }
    }
}
