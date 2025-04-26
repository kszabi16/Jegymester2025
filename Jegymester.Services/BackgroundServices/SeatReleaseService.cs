using Jegymester.DataContext.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Jegymester.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.Services.BackgroundServices
{
    public class SeatReleaseService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10); // 10 percenként fusson

        public SeatReleaseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<JegymesterDbContext>();

                    await FreeSeatsAfterScreeningAsync(context);
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task FreeSeatsAfterScreeningAsync(JegymesterDbContext context)
        {
            var now = DateTime.UtcNow;

            var screenings = await context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Tickets)
                .ToListAsync();

            foreach (var screening in screenings)
            {
                var screeningEndTime = screening.StartTime.AddMinutes(screening.Movie.Length);

                if (screeningEndTime <= now && !screening.Deleted)
                {
                    await ReleaseSeatsAsync(context, screening);
                    MarkScreeningAsDeleted(screening);
                }
            }

            await context.SaveChangesAsync();
        }

        private async Task ReleaseSeatsAsync(JegymesterDbContext context, Screening screening)
        {
            foreach (var ticket in screening.Tickets)
            {
                var seat = await context.Seats.FindAsync(ticket.SeatId);
                if (seat != null && seat.IsOccupied)
                {
                    seat.IsOccupied = false;
                }
            }
        }

        private void MarkScreeningAsDeleted(Screening screening)
        {
            screening.Deleted = true;
        }
    }
}
