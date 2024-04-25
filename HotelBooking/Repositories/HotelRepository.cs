using Hotel.Data;
using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Repositories
{
    public class HotelRepository : IHotelRespository
    {
        private readonly HotelDbContext _hotelDbContext;

        public HotelRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        public Task CancelBooking(RoomBooking booking)
        {
            throw new NotImplementedException();
        }

        public void CreateBooking(RoomBooking booking)
        {
            _hotelDbContext.RoomBooking.Add(booking);
            _hotelDbContext.SaveChanges();
        }

        public Task EditBooking(RoomBooking booking)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Models.Hotel>> GetAllHotels()
        {
            return await _hotelDbContext.Hotel.Include(h => h.Address).ToListAsync();
        }
            
        public async Task<IEnumerable<Models.Hotel>> GetHotelsByBookingDetails(string city, string country, string noOfGuests, string checkInDate, string checkOutDate)
        {
            List<int> ids = _hotelDbContext.Address.ToList()
              .Where(x => (x.City.ToLower() == city.ToLower() && x.Country.ToLower() == country.ToLower()))
              .Select(x => x.Id)
              .ToList();

            return await _hotelDbContext.Hotel.Include(h => h.Address)
                .Where(h =>ids.Contains(h.AddressId)).ToListAsync();
                
        }
    }
}
