using GasStationMs.Dal.Entities;
using GasStationMs.Dal.Entities.Configurations;
using GasStationMs.Dal.PresetData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GasStationMs.Dal
{
    public class GasStationContext : DbContext
    {
        public static readonly ILoggerFactory ConsoleLoggerFactory =
            new LoggerFactory().AddConsole(LogLevel.Information);

        public GasStationContext()
        {
        }

        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Car> Cars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                .UseSqlServer("Server=.\\SQLEXPRESS; Initial Catalog=GasStationMs;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FuelConfiguration());
            modelBuilder.ApplyConfiguration(new CarConfiguration());

            modelBuilder.Entity<Fuel>().HasData
            (
                FuelPreset.GetPresetFuels()
            );

            modelBuilder.Entity<Car>().HasData
            (
                CarPreset.GetPresetCars()
            );
        }

        public string Hello()
        {
            return "Hello";
        }
    }
}