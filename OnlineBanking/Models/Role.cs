using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineBanking.Models
{
    public class Role
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required, MaxLength(50), RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters of the alphabet")]
        [Display(Name = "Role")]
        public string role { get; set; }
    }
}
