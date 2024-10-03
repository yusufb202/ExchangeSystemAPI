namespace Core.Models
{
    public class UserActivityReport
    {
        public int Id { get; set; } // Primary key
        public int UserId { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalAmountExchanged { get; set; }
        public string MostTradedCurrency { get; set; }
    }
}
    