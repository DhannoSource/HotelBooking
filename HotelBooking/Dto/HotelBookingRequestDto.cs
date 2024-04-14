namespace Hotel.Dto
{
    public class HotelBookingRequestDto
    {
        public int HotelId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
    }
}
