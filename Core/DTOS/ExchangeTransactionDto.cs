using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS
{
    public class ExchangeTransactionDto
    {
        public string UserName { get; set; }

        public string FromCurrencyCode { get; set; }

        public string ToCurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
