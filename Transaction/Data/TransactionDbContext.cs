using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Transaction.Data
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<Models.Transaction> Transaction { get; set; }


        public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Server=HotelSqlServerDb;Database=Transaction;User ID=sa;Password=SqlUser123$;TrustServerCertificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Transaction>().HasData(new Models.Transaction
            {
                Id = Guid.NewGuid(),
                BookingType = "Hotel",
                BookingDate = DateTime.Now,
                BookingId = 1,
                UserId = 3
            });
        }
    }
}
