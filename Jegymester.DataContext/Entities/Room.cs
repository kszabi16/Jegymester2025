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
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }

        //Kapcsolatok
       public List<Screening> Screenings { get; set; }
    }
}
