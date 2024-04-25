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
using System.Diagnostics.Metrics;
using Hotel.Repositories;
using RabbitMQ.Client;
using System.Text;
using Hotel.RabbitMQ;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelDbContext _context;
        private readonly IHotelRespository _hotelRespository;
        private readonly IRabbitMQProducer _rabitMQProducer;

        public HotelController(IHotelRespository hotelRespository, IRabbitMQProducer rabbitMQProducer)
        {
            _hotelRespository = hotelRespository;
            _rabitMQProducer = rabbitMQProducer;
        }


        //Get Hotels
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            ResponseDto response;
            try
            {
                string message;

                var lstHotels = await _hotelRespository.GetAllHotels();

                if (lstHotels.Any())
                {
                    message = "Records found.";
                }
                else
                {
                    message = "No records found.";
                }
                response = new ResponseDto()
                {
                    Success = true,
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
        //Get Hotels
        [HttpGet("hotels/{city}/{country}/{noOfGuests}/{checkInDate}/{checkOutDate}")]
        public async Task<IActionResult> SearchHotels(string city, string country, string noOfGuests, string checkInDate, string checkOutDate)
        {

            ResponseDto response;
            try
            {
              var lstHotels = await _hotelRespository.GetHotelsByBookingDetails(city,country,noOfGuests,checkInDate,checkOutDate);
                string message;
                if (lstHotels.Any())
                {
                    message = "Records found.";
                }
                else
                {
                    message = "No records found.";
                }
                response = new ResponseDto()
                {
                    Success = true,
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
                //_context.RoomBooking.Add(roomBooking);
                //_context.SaveChanges();
                _hotelRespository.CreateBooking(roomBooking);
                HotelTransaction hotelTransaction = new HotelTransaction()
                {
                    BookingDate = DateTime.Now,
                    BookingId = roomBooking.Id,
                    BookingType = "Hotel",
                    UserId = request.UserId,
                };
                //send the inserted product data to the queue and consumer will listening this data from queue
                _rabitMQProducer.SendHotelBookingtMessage(hotelTransaction);
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

        //Edit Booking
        [HttpPost("cancelBooking")]
        public IActionResult CancelBooking([FromBody] CancelBookingRequestDto request)
        {
            ResponseDto response;
            try
            {
                RoomBooking editBooking = new RoomBooking()
                {
                    UserId = request.UserId,
                    Id = request.requestId,
                };

                var result = _context.RoomBooking.SingleOrDefault(b => b.Id == request.requestId && b.UserId == request.UserId);
                if (result != null)
                {
                    _context.RoomBooking.Remove(result);
                    _context.SaveChanges();

                    response = new ResponseDto()
                    {
                        Success = true,
                        Message = "Booking cancelled successfully.",
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

        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {
            // TOOO: Reuse and close connections and channel, etc, 
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "user",
                                             routingKey: integrationEvent,
                                             basicProperties: null,
                                             body: body);
        }

    }

}

