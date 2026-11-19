using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoomHub.Api.Models;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // SQLite nu pastreaza DateTimeKind; la citire marcam datele ca UTC
        var utcConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        modelBuilder.Entity<Room>(room =>
        {
            room.ToTable("Rooms");
            room.HasKey(r => r.Id);
            room.Property(r => r.Id).ValueGeneratedNever();
            room.Property(r => r.Name).IsRequired().HasMaxLength(200);
            room.Property(r => r.Capacity).IsRequired();
        });

        modelBuilder.Entity<Booking>(booking =>
        {
            booking.ToTable("Bookings");
            booking.HasKey(b => b.Id);
            booking.Property(b => b.Id).ValueGeneratedNever();
            booking.Property(b => b.BookedBy).IsRequired().HasMaxLength(200);
            booking.Property(b => b.StartUtc).HasConversion(utcConverter);
            booking.Property(b => b.EndUtc).HasConversion(utcConverter);
            booking.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);

            booking.HasOne<Room>()
                .WithMany()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            booking.HasIndex(b => b.RoomId);
        });
    }
}
