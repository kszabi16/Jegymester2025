using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    //Jegyek
    public class Ticket : AbstractEntity
    {
        public decimal Price { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ScreeningTime { get; set; }

        //Kapcsolatok
        public Screening Screenings { get; set; }
        public int ScreeningId { get; set; }

        public User Users { get; set; }
        public int? UserId { get; set; }

        public Seat Seat { get; set; }
        public int SeatId { get; set; }
    }
    public enum TicketType
    {
        Normal = 1,
        Student = 2,
        Senior = 3,
        Child = 4,
        VIP = 5
    }
    public static class TicketPricing
    {
        public static decimal GetPrice(TicketType ticketType)
        {
            return ticketType switch
            {
                TicketType.Normal => 3000m,
                TicketType.Student => 2500m,
                TicketType.Senior => 2000m,
                TicketType.Child => 1500m,
                TicketType.VIP => 5000m,
                _ => throw new ArgumentOutOfRangeException(nameof(ticketType), "Invalid ticket type") //bármilyen más érték, ami nem illik az előző ágakra
            };
        }
    }

}
