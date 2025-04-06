using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.DataContext.Context
{
    public class JegymesterDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public  DbSet<Booking> Bookings { get; set; }

        public JegymesterDbContext(DbContextOptions<JegymesterDbContext> options) : base(options)
        {

        }

    }
}
