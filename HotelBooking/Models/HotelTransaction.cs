namespace Hotel.Models
{
    public class HotelTransaction
    { 
        public int UserId { get; set; }
        public string BookingType { get; set; }
        public DateTime BookingDate { get; set; }

        public int BookingId { get; set; }
    }
}
