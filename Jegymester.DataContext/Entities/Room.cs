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
        public required string Name { get; set; }
        public int Capacity { get; set; }
        //Kapcsolatok
        public List<Seat> Seats { get; set; } = new List<Seat>();
        public List<Screening> Screenings { get; set; } = new List<Screening>();    
       
    }
}
