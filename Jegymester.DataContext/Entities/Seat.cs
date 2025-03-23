using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jegymester.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jegymester.DataContext.Entities
{
    //Ülések
    public class Seat : AbstractEntity
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}