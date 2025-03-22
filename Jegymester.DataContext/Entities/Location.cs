using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jegymester.DataContext.Entities
{
    public class Location : AbstractEntity
    {
        public string Country {get; set;}
        public string City { get; set; }

        //Kapcsolatok
        public List<Room> Rooms { get; set; }
    }
}
