using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ExchangeRate
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
        [Column(TypeName = "decimal(18,6)")]
        public decimal Rate { get; set; }

        [Required]
        public DateTime Date { get; set; }

    }
}
