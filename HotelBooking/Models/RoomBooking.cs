namespace Hotel.Models
{
    public class RoomBooking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int HotelId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
