using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    //Rendelések
    public class Booking : AbstractEntity
    {
        public DateTime BuyDate { get; set; }

        //Kapcsolatok
        public User User { get; set; }
        public int UserId { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }




    }
}
