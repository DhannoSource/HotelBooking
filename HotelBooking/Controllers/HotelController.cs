using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hotel.Models;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Hotel.Dto;
using System.Linq;
using System.ComponentModel;
using Azure;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public HotelController(HotelDbContext context)
        {
            _context = context;
        }

        //Get Hotels
        [HttpGet("hotels")]
        public IActionResult SearchHotels(string city, string country, string noOfGuests, string checkInDate, string checkOutDate)
        {

            ResponseDto response;
            try
            {
                List<int> ids = _context.Address.ToList()
              .Where(x => (x.City.ToLower() == city.ToLower() && x.Country.ToLower() == country.ToLower()))
              .Select(x => x.Id)
              .ToList();

               var lstHotels = _context.Hotel.ToList().Where(x => ids.Contains((int)x.Address?.Id));
                string message;
                if(lstHotels.Any() )
                {
                    message = "Records found.";
                }
                else
                {
                    message = "No records found.";
                }
            response = new ResponseDto() 
                                { Success = true ,
                                    Message = message,
                                    Payload = lstHotels
                                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ResponseDto()
                {
                    Success = false,
                    Message = ex.Message,
                    Payload = null
                };
                return Ok(response);
            }

          
        }

        //Create Booking
        [HttpPost("createBooking")]
        public IActionResult CreateBooking([FromBody] HotelBookingRequestDto request)
        {
            ResponseDto response;
            try
            {
                RoomBooking roomBooking = new RoomBooking()
                {
                    UserId = request.UserId,
                    HotelId = request.HotelId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                };
                _context.RoomBooking.Add(roomBooking);
                _context.SaveChanges();

                response = new ResponseDto()
                {
                    Success = true,
                    Message = "Booking Successful.",
                    Payload = null,
                };

                return Ok(response);
            }
            catch (Exception ex)
            { 
              response = new ResponseDto()
              {
                  Success = false,
                  Message = ex.Message,
                  Payload = null
              };
               return Ok(response);
            }

        }

        //Edit Booking
        [HttpPost("editBooking")]
        public IActionResult EditBooking([FromBody] EditBookingRequestDto request)
        {
            ResponseDto response;
            try
            {
                RoomBooking editBooking = new RoomBooking()
                {
                    UserId = request.UserId,
                    Id = request.requestId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    HotelId = request.HotelId,
                };

                    var result = _context.RoomBooking.SingleOrDefault(b => b.Id == request.requestId);
                    if (result != null)
                    {
                        result.StartDate = request.StartDate;
                        result.EndDate = request.EndDate;
                        _context.SaveChanges();

                        response = new ResponseDto()
                        {
                            Success = true,
                            Message = "Booking Update Successful.",
                            Payload = null,
                        };

                        return Ok(response);
                    }
                    else
                    {
                        response = new ResponseDto()
                        {
                            Success = false,
                            Message = "Booking Not Found.",
                            Payload = null,
                        };
                        return Ok(response);
                }
               
            }
            catch (Exception ex)
            {
                response = new ResponseDto()
                {
                    Success = false,
                    Message = ex.Message,
                    Payload = null
                };
                return Ok(response);
            }
        }

    }

}

