using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{ 
    //Moziterem
    public class Room : AbstractEntity
    {
        public int Capacity { get; set; }
        //Kapcsolatok
        public List<Screening> Screenings { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
