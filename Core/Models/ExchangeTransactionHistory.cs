using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ExchangeTransactionHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ExchangeTransactionId { get; set; }

        [ForeignKey("ExchangeTransactionId")]
        public ExchangeTransaction ExchangeTransaction { get; set; }

        [Required]
        public DateTime DateRecorded { get; set; } = DateTime.UtcNow;
    }
}
