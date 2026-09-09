using System;

using Microsoft.EntityFrameworkCore;

namespace MediatorSite.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }
}