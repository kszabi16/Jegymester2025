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
        public int Row { get; set; }
        public int Column { get; set; }
        public int Price { get; set; }

        //Kapcsolatok
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
       
    }
}
