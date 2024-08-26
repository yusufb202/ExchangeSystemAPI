using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ExchangeTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [MaxLength(100)]
        public string FromCurrencyName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ToCurrencyName { get; set; }

        [Required]
        public int FromCurrencyId { get; set; }

        [ForeignKey("FromCurrencyId")]
        public Currency FromCurrency { get; set; }

        [Required]
        public int ToCurrencyId { get; set; }

        [ForeignKey("ToCurrencyId")]
        public Currency ToCurrency { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal ExchangeRate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    }
}
