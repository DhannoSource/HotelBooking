using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Hotel.Models;

namespace Hotel.Data
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Hotel.Models.Hotel> Hotel { get; set; }
        public DbSet<Address> Address { get; set; }

        public DbSet<RoomBooking> RoomBooking { get; set; }

        public string DbPath { get; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options):base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Hotel.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Server=tcp:localhost,1433;Database=Hotel;User ID=sa;Password=SqlUser123$;TrustServerCertificate=True");
    }
}
