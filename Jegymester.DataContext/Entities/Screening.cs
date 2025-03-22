using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    //Vetítések
    public class Screening : AbstractEntity
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }


        
    }
}
