using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ExchangeRateTrend
    {
        public string CurrencyPair { get; set; }
        public DateTime Date { get; set; }
        public decimal AverageRate { get; set; }
    }
}
