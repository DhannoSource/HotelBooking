namespace Hotel.Dto
{
    public class EditBookingRequestDto
    {
        public int requestId { get; set; }
        public int UserId { get; set; }
        public int HotelId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        
    }
}
