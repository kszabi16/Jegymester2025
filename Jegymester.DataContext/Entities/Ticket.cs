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

        public string TicketType { get; set; }

        //Kapcsolatok
        public Screening Screenings { get; set; }
        public int ScreeningId { get; set; }

        public User Users { get; set; }
        public int UserId { get; set; }

        public Room Room { get; set; }
        public int RoomId { get; set; }

        public Seat Seat { get; set; }
        public int SeatId { get; set; }
    }
}
