namespace Hotel.Models
{
    public class Hotel
    {
        public Hotel()
        {
        }

        public Hotel(int hotelId, string name, string description)
        {
            this.Id = hotelId;
            this.Name = name;
            this.Description = description;
            
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Address?  Address  { get; set; }

        public int AddressId { get; set; }
        public bool HasPool { get; set; }
        public bool HasGym { get; set; }
        public int Stars { get; set; }
        public int UserRating { get; set; }
        public int NoOfRooms { get; set; }

    }

}