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
        
        public DateTime StartTime { get; set; }

        //Kapcsolatok
        
        public List<Ticket> Tickets { get; set; } 
        
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }

    }
}
