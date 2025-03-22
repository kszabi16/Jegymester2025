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
        DbSet<Movie> Movies { get; set; }
        DbSet<Screening> Screenings { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Ticket> Tickets { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Booking> Bookings { get; set; }

        public JegymesterDbContext(DbContextOptions<JegymesterDbContext> options) : base(options)
        {

        }

    }
}
