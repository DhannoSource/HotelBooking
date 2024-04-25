namespace Transaction.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string BookingType { get; set; }
        public DateTime BookingDate { get; set; }

        public int BookingId { get; set; }
    }
}
