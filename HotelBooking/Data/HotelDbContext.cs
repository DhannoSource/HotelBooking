using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Hotel.Models;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hotel.Data
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Hotel.Models.Hotel> Hotel { get; set; }
        public DbSet<Address> Address { get; set; }

        public DbSet<RoomBooking> RoomBooking { get; set; }

        public string DbPath { get; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options)
         : base(options)
        {
            var dbCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (dbCreater != null)
            {
                // Create Database 
                if (!dbCreater.CanConnect())
                {
                    dbCreater.Create();
                }

                // Create Tables
                if (!dbCreater.HasTables())
                {
                    dbCreater.CreateTables();
                }
            }
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer(@"Server=tcp:localhost,1433;Database=Hotel;User ID=sa;Password=SqlUser123$;TrustServerCertificate=True");

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Server=HotelSqlServerDb;Database=Hotel;User ID=sa;Password=SqlUser123$;TrustServerCertificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasData(new Address
            {
                Id = 1,
                AddreeLine1 = "Seminyak",
                AddressLine2 = "Bali",
                City = "Bali",
                Country = "Indonesia",
                PostalCode = "72349"
            },
            new Address { Id = 2, AddreeLine1 = "Nariman Point", AddressLine2 = "South Mumbai", City = "Mumbai", Country = "India", PostalCode = "4473836" },
            new Address { Id = 3, AddreeLine1 = "Orchard Rd", AddressLine2 = "Sommerset", City = "Singapore", Country = "Singapore", PostalCode = "633578" });

           modelBuilder.Entity<Hotel.Models.Hotel>().HasData(new Hotel.Models.Hotel
            {
                Id= 1,
                Name= "Mariott",
                AddressId = 1,
                 HasGym=true,
                 HasPool=true,
                 UserRating=5,
                 Stars=5,
                 NoOfRooms = 30
            }, 
            new Hotel.Models.Hotel {
                Id = 2,
                Name = "Hilton",
                AddressId  =2,
                HasGym = true,
                HasPool = true,
                UserRating = 5,
                Stars = 5,
                NoOfRooms = 30
            },
            new Hotel.Models.Hotel
            {
                Id= 3,
                Name = "Pan Pacific",
                AddressId = 3,
                HasGym = true,
                HasPool = true,
                UserRating = 5,
                Stars = 5,
                NoOfRooms = 30
            });
        }

    }
}
