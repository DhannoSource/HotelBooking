using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hotel.Models;
using Hotel.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Hotel.Dto;

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
        public IActionResult GetHotels()
        {
            var lstHotels = _context.Hotel.ToList();// new List<Models.Hotel>() { new Models.Hotel() { Id = 1, Name = "New Moon", Description = "Beach Front" },
            //new Models.Hotel() {Id = 2, Name="Radisson", Description ="resort"} };
            return Ok(lstHotels);
        }
    }
}

