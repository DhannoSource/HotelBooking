using Hotel.Models;
using System.Collections.Generic;

namespace Hotel.Repositories
{
    public interface IHotelRespository
    {
        Task<IEnumerable<Models.Hotel>> GetAllHotels();
        Task<IEnumerable<Models.Hotel>> GetHotelsByBookingDetails(string city, string country, string noOfGuests, string checkInDate, string checkOutDate);
        void CreateBooking(RoomBooking booking);
        Task EditBooking(RoomBooking booking);

        Task CancelBooking(RoomBooking booking);
    }
}
