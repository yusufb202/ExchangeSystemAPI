using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FromCurrencyId { get; set; }

        [ForeignKey("FromCurrencyId")]
        public Currency FromCurrency { get; set; }

        [Required]
        public int ToCurrencyId { get; set; }

        [ForeignKey("ToCurrencyId")]
        public Currency ToCurrency { get; set; }

        [Required]
        public decimal Amount { get; set; } 

        [Required]
        public DateTime Date { get; set; } 
    }
}
